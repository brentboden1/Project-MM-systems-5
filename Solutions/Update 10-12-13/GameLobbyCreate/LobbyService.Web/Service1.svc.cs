using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LobbyService.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        //Intitialisation
        #region Initialisation()
        //Inistialisation of all the private values
        private int lobbyvalue;                                                         //lobbyvalue is used to determine the lobbyId
        private int playervalue;                                                        //playervalue is used to determine the playerId          

        //The gamelobby list is static so all clients get to see te same lobbies
        private static List<DTO.PlayerLobby> gamelobbies = new List<DTO.PlayerLobby>(); 
  
        private static DTO.MonopolyEngine.SingleGame NewGame;

        //Member of the dataclasses so we can query for results of the SQL database
        private DataClasses1DataContext dc;
        public Service1()
        {
            dc = new DataClasses1DataContext();
        }

        private Nullable<int> location;

        #endregion

        //Private methods
        #region private methods()

        //All private methods for the players (clients)
        #region Player()

        //This method generetes the ID for the player
        private int GenerateID()
        {
            //Get players from database
            var player = from p in dc.Players select p;
            playervalue = 0;
            
            //see if there is a free space
            for (int i = 0; i < player.Count() + 1; i++)
            {
                foreach (var item in player)
                {
                    if (!(item.PlayerId == i))
                    {
                        playervalue = i;
                    }
                }
            }

            return playervalue;
        }

        //Check if the playername already exist
        private bool CheckIfPlayerAlreadyExists(string p)
        {
            //get all player from the database
            var player = from play in dc.Players select play;

            bool result = false;

            //see if there is already someone with the name of the new player
            foreach (var item in player)
            {
                if (string.Equals(p, item.PlayerName))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        //Convert a sql database player to a DTO player
        private DTO.Player ConvertToPlayer(Player p)
        {
            DTO.Player pl = new DTO.Player();
            pl.PlayerId = p.PlayerId;
            pl.PlayerName = p.PlayerName;
            return pl;
        }

        //get player by name
        private string GetPlayerName(int playerId)
        {
            //if playerid from database is the given playerID => select that player
            string pName = (from p in dc.Players where p.PlayerId == playerId select p.PlayerName).First();
            return pName;
        }

        #endregion

        //All private methods for the lobbies (playerlobbies)
        #region Lobby()

        //Method to generate the lobby ID
        private int getLobbyId()
        {
            //get all lobbies from the database
            var lobby = from p in dc.PlayerLobbies select p;
            lobbyvalue = 0;

            //see if there is a free space
            for (int i = 0; i < lobby.Count() + 1; i++)
            {
                foreach (var item in lobby)
                {
                    if (!(item.LobbyId == i))
                    {
                        lobbyvalue = i;
                    }
                }
            }

            return lobbyvalue;
        }

        //Joinlobby method
        private void EnterLobby(DTO.Player pl, string lobbyName)
        {
            //get first lobby whose lobbyname exceeds the string lobbyname and select the lobbyID
            var l = (from d in dc.Lobbies where d.LobbyName == lobbyName select d.LobbyId).First();

            if (lobbyName != null)
            {
                //get the gamelobby where the id is the value of l
                var lob = (from p in gamelobbies where p.LobbyId.LobbyId == (int)l select p).First();

                if (lob != null)
                {
                    if (lob.StartGame != true)
                    {
                        //add the player to the lobby
                        lob.Player.Add(new DTO.Player() { PlayerId = pl.PlayerId, PlayerName = pl.PlayerName, AlreadExist = pl.AlreadExist });
                    }
                }
            }
        }

        //Method to see if the max players of an gamelobby are exceeded
        private void CheckIfMaxPlayerExceeded(DTO.Player host)
        {
            //get first lobby whose lobbyname exceeds the host playername and select the lobby
            var lobby = (from p in gamelobbies where p.LobbyId.LobbyName == host.PlayerName select p).First();
            //check if playercount is 4
            if (lobby.Player.Count() == 4)
            {
                lobby.StartGame = true;
            }
            else
            {
                lobby.StartGame = false;
            }
        }

        #endregion

        //All private methods for the Game itself
        #region Game()

        //method to convert an iEnumerable of database playerlobbies to an DTO playerlobby
        private static DTO.PlayerLobby ConvertToDTOPlayerLobby(DTO.Player Host, IEnumerable<DTO.PlayerLobby> lobby)
        {
            DTO.PlayerLobby plLobby = new DTO.PlayerLobby();
            foreach (var item in lobby)
            {
                if (item.HostPlayer == Host)
                {
                    plLobby.HostPlayer = item.HostPlayer;
                    plLobby.IsAwaitingForPlayers = item.IsAwaitingForPlayers;
                    plLobby.LobbyId = item.LobbyId;
                    plLobby.Player = item.Player;
                    plLobby.StartGame = item.StartGame;
                }
            }
            return plLobby;
        }

        #endregion

        #endregion

        //Public Methods
        #region Public Methods()

        //Player creation methods
        #region PlayerCreation()

        //addplayer method => adds the player to the database, only a name must be given
        public void AddPlayer(string playerName)
        {
            Player p = new Player();
            p.PlayerName = playerName;
            p.PlayerId = (int)GenerateID(); //get unique id from code in the private method generateID and parse it to an integer value
            p.AlreadExist = false; //This is done static, because the method dont works so well

            //save all the changes in the database
            dc.Players.InsertOnSubmit(p);
            dc.SubmitChanges();

        }

        //get all the players in the database and add it to a list
        public List<DTO.Player> GetPlayers()
        {
            //select all players from the database
            var pl = from p in dc.Players select p;

            //make a list of DTO players
            List<DTO.Player> alist = new List<DTO.Player>();

            foreach (var item in pl)
            {
                //add every player from database to the list with the method ConvertToPlayer
                alist.Add(ConvertToPlayer(item));
            }
            return alist;
        }

        //get the player by its id
        public DTO.Player GetPlayerById(int id)
        {
            //get all players which its id is the same as the id that is given
            var pl = from p in dc.Players where p.PlayerId == id select p;
            
            //make an dto player
            DTO.Player player = new DTO.Player();

            foreach (var item in pl)
            {
                //convert the database player to an DTO player
                player = ConvertToPlayer(item);
            }

            return player;
        }

        //get the player by its name
        public DTO.Player GetPlayerByName(string name)
        {
            //get player which playername is the same as the name that is given to the method
            var player = (from p in dc.Players where p.PlayerName == name select p).Single();
            //convert that player to an DTO player
            return ConvertToPlayer(player);
        }

        #endregion

        //Lobby methods
        #region Create Lobby()

        //Lobby Creation
        public void CreateLobby(DTO.Player host)
        {
            try
            {
                //make a playerlobby and set the hostplayer member to th playerid of the host, ist lobbyid is been generated
                //its playerid is set to the playerid of the host, isawatingforplayers is true and startgame false
                PlayerLobby pl = new PlayerLobby();
                pl.HostPlayer = (int)host.PlayerId;
                pl.LobbyId = getLobbyId();
                pl.PlayerId = (int)host.PlayerId;
                pl.IsWaitingForPlayers = true; //to determine which playerlobby is still awaiting for players
                pl.StartGame = false; //to indicate if the game is started or not

                //make a new lobby with its name is the name of the host and its id is the same as the playerlobby's id
                Lobby alobby = new Lobby();
                alobby.LobbyName = host.PlayerName;
                alobby.LobbyId = pl.LobbyId;

                //add the lobby to the static playerlobby list
                gamelobbies.Add(new DTO.PlayerLobby(host) { LobbyId = new DTO.Lobby() { LobbyId = alobby.LobbyId, LobbyName = alobby.LobbyName }, IsAwaitingForPlayers = true, StartGame = false });

                //save all changes
                dc.Lobbies.InsertOnSubmit(alobby);
                dc.PlayerLobbies.InsertOnSubmit(pl);
                dc.SubmitChanges();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        //Show the playerslobbies
        #region ShowPlayLobbies()

        //get all the lobbies whose bool IsAwaitingForPlayers is false
        public List<DTO.PlayerLobby> GatAvailablePlayLobbies()
        {
            //get the hostplayer and the lobbyID
            var rooms = from p in dc.PlayerLobbies
                        where p.PlayerId == p.HostPlayer
                        select new { p.LobbyId, p.HostPlayer };

            //make a list of DTO playerlobbies
            List<DTO.PlayerLobby> lobbies = new List<DTO.PlayerLobby>();

            foreach (var item in rooms)
            {
                //add all lobbies in rooms to the list of DTO playerlobbies
                DTO.PlayerLobby l = new DTO.PlayerLobby(new DTO.Player() { PlayerId = (int)item.HostPlayer, PlayerName = GetPlayerName((int)item.HostPlayer) }) { LobbyId = new DTO.Lobby() { LobbyId = item.LobbyId } };
                lobbies.Add(l);
            }

            return lobbies;
        }

        #endregion

        //Get player methods
        #region GetPlayer()

        //get player by name
        public DTO.Player GetPlayer(string Name)
        {
            //get the first player with its name the same as the given name
            var pl = (from p in dc.Players
                      where p.PlayerName == Name
                      select p).First();

            return new DTO.Player() { PlayerId = pl.PlayerId, PlayerName = pl.PlayerName };
        }

        //get player by id
        public DTO.Player GetPlayerId(int id)
        {
            //select the first playerID that equeals the hostplayers id and the id that is given
            var pl = (from p in dc.Players
                      join pla in dc.PlayerLobbies
                      on p.PlayerId equals pla.HostPlayer
                      where p.PlayerId == id
                      select p).First();

            return new DTO.Player() { PlayerId = pl.PlayerId, PlayerName = pl.PlayerName };
        }

        #endregion

        //delete methods
        #region Delete()

        //deletes all the players
        public void DeleteAllPlayers()
        {
            //get all players from database
            var players = from p in dc.Players select p;
            if (players != null)
            {
                foreach (var item in players)
                {
                    dc.Players.DeleteOnSubmit(item);
                }
                dc.SubmitChanges();
            }
        }

        //deletes all the lobbies
        public void DeleteAllLobbies()
        {
            //clears first the gamelobbies
            gamelobbies.Clear();
            //get all lobbies
            var lobby = from l in dc.Lobbies select l;
            //get all playerlobbies
            var lobbies = from p in dc.PlayerLobbies select p;
            //deletes all the playerlobbies
            if (lobbies != null)
            {
                foreach (var item in lobbies)
                {
                    dc.PlayerLobbies.DeleteOnSubmit(item);
                }
            }
            //deletes all the lobbies
            if (lobby != null)
            {
                foreach (var item in lobby)
                {
                    dc.Lobbies.DeleteOnSubmit(item);
                }
            }
            dc.SubmitChanges();
        }
        
        //delete a single player
        public void DeletePlayer(DTO.Player pl)
        {
            Player p = new Player();
            p.PlayerId = pl.PlayerId;
            p.PlayerName = pl.PlayerName;

            dc.Players.DeleteOnSubmit(p);
            dc.SubmitChanges();
        }

        //delete a single playerlobby
        public void DeletePlayerLobby(DTO.PlayerLobby pl)
        {
            //get the wanted playerlobby out of the game lobby list
            var play = (from  p in gamelobbies where p.HostPlayer.PlayerId == pl.HostPlayer.PlayerId select p).First();
            //delete this lobby
            gamelobbies.Remove(play);

            //get the playerlobby to delete
            PlayerLobby playlobby = new PlayerLobby();
            playlobby.LobbyId = pl.LobbyId.LobbyId;
            playlobby.HostPlayer = pl.HostPlayer.PlayerId;

            dc.PlayerLobbies.DeleteOnSubmit(playlobby);
            dc.SubmitChanges();
        }

        #endregion

        //Joining lobbies
        #region Join()

        //join method
        public void JoinLobbyRoom(DTO.Player pl, DTO.Player Host)
        {
            //select the lobbyid from de lobbies in the database where the lobbyname is the same as the hosts playername
            int lobbyId = (from l in dc.Lobbies where l.LobbyName == Host.PlayerName select l.LobbyId).First();
              //  CheckIfMaxPlayerExceeded(Host);

                PlayerLobby plobby = new PlayerLobby();
                plobby.HostPlayer = Host.PlayerId;
                plobby.LobbyId = lobbyId;
                plobby.PlayerId = (int)pl.PlayerId;
                plobby.IsWaitingForPlayers = true;
                plobby.StartGame = false;
            //let the player pl join the lobby
                EnterLobby(pl, Host.PlayerName);

                dc.PlayerLobbies.InsertOnSubmit(plobby);
                dc.SubmitChanges();
            
        }

        //show all players in the lobbyroom
        public List<DTO.Player> ShowPlayersInLobbyRoom(int host)
        {
            //select the playerid of the host
            var pl = (from p in dc.Players where p.PlayerId == (int)host select p.PlayerId).First();

            //select the players its playerids and playernames
            var player = from o in dc.PlayerLobbies
                         join p in dc.Players
                         on o.PlayerId equals p.PlayerId
                         where o.HostPlayer == pl
                         select new { p.PlayerId, p.PlayerName };

            List<DTO.Player> players = new List<DTO.Player>();

            foreach (var item in player)
            {
                //add all players to the list
                players.Add(new DTO.Player() { PlayerId = item.PlayerId, PlayerName = item.PlayerName });
            }

            return players;
        }

        #endregion

        //Update methods
        #region Update()

        //show the amount of players in a lobby
        public int CheckPlayerCount(string lobby)
        {
            //select the lobbyid
            var l = (from d in dc.Lobbies where d.LobbyName == lobby select d.LobbyId).Single();
            //select the gamelobbyid which is the same id as l
            var gamelob = (from p in gamelobbies where p.LobbyId.LobbyId == (int)l select p).Single();
            if (gamelob != null)
            {
                int a = 0;
                for (int i = 0; i < gamelob.Player.Count(); i++)
                {
                    a++;
                }
                return a;
            }
            else
            {
                return 0;
            }
        }

        //send a game update => only the dices who has been rolled
        public List<int> GetGameUpdate(DTO.Player host)
        {
                List<int> Dice = new List<int>();
            //let the client dice
                NewGame.Dice();
                var dice = from p in NewGame.publicState.lastDieRoll select p;
                foreach (var item in dice)
                {
                    //get the dices
                    Dice.Add(Convert.ToInt32(item));
                }
                return Dice;          
        }

        //get the playerlocation
        public int GetPlayerLocation(DTO.Player Host, DTO.Player Player)
        {
            foreach (var item in NewGame.publicState.PlayerList)
            {
                if (NewGame.publicState.ActivePlayer.PlayerId == Player.PlayerId)
                {
                    if (item.Location != 0)
                    {
                        location = item.Location;
                    }
                }
                
            }
            return location.Value;
        }

        //get an update from the gamestate
        public DTO.MonopolyEngine.GameState GetStateUpdate(DTO.Player player)
        {
            //if the playerid is the same id as that from the activeplayer => send an update
            if (NewGame.Started)
            {
                if (NewGame.publicState.ActivePlayer.PlayerId == player.PlayerId)
                {
                    return NewGame.publicState;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        //get the active player's id
        public int GetUpdate(DTO.Player host)
        {
            var lst = (from p in gamelobbies where p.HostPlayer.PlayerId == host.PlayerId select p).First();

            return NewGame.publicState.ActivePlayer.PlayerId;
        }

        /*//OLD METHOD
        public DTO.MonopolyEngine.GameState Brent(DTO.Player me)
        {
            DTO.MonopolyEngine.GameState tmp = NewGame.publicState;

            DTO.MonopolyEngine.StateToClient state = new DTO.MonopolyEngine.StateToClient();
            return null;//NewGame.publicState;
        }*/

        //NEW METHOD TOMPEETERS 10/12/2013
        DTO.MonopolyEngine.StateToClient IService1.GetState(DTO.Player me)
        {
            DTO.MonopolyEngine.GameState tmp = NewGame.publicState;

            DTO.MonopolyEngine.StateToClient state = new DTO.MonopolyEngine.StateToClient();

           // state.ActivePlayer = tmp.ActivePlayer;
            state.ActiveTileName = tmp.ActiveTileName;
            state.CurrentPhase = tmp.CurrentPhase;
            state.DieCast = tmp.DieCast;
            state.EnableBuy = tmp.EnableBuy;
            state.lastDieRoll = tmp.lastDieRoll;
            state.PlayerTradeRequested = tmp.PlayerTradeRequested;
            state.PropertyTradeDirection = tmp.PropertyTradeDirection;
            state.PropertyTradeRequested = tmp.PropertyTradeRequested;
            state.TurnNumber = tmp.TurnNumber;
            //state.PlayerList = tmp.PlayerList;
            state.ActivePlayer = new DTO.MonopolyEngine.GamePlayerToClient();
            foreach(var p in tmp.PlayerList)
            {
                if(p.MyPlayer.PlayerId == me.PlayerId)
                {
                    state.ActivePlayer.Cash = p.Cash;
                    state.ActivePlayer.HasEscapePrison = p.HasEscapePrison;
                    state.ActivePlayer.IsActive = p.IsActive;
                    state.ActivePlayer.IsPlaying = p.IsPlaying;
                    state.ActivePlayer.IsPrison = p.IsPrison;
                    state.ActivePlayer.Location = p.Location;
                    state.ActivePlayer.PrisonTime = p.PrisonTime;
                }
            }
            
            return state;//NewGame.publicState;
        }

        #endregion

        //Start game
        #region GameStart()

        //Start the game
        public void StartGame(DTO.Player Host)
        {
            //select the lobbyid from the playerlobby which hostplayer is Host
            int lst = (from l in dc.PlayerLobbies where l.HostPlayer == Host.PlayerId select l.LobbyId).First();
            //select the lobbyname of the lobby with the same name as lst
            var lobName = (from p in dc.Lobbies where p.LobbyId == lst select p.LobbyName).First();
            //select all playerlobbies from the database whose hostplayer id is the same as the Host's playerID
            var update = (from l in dc.PlayerLobbies where l.HostPlayer == Host.PlayerId select l).ToList();

            //Make a static playerlobby to test it all
               DTO.PlayerLobby pl = new DTO.PlayerLobby(Host) { LobbyId = new DTO.Lobby() { LobbyId = lst, LobbyName = lobName.ToString() }, IsAwaitingForPlayers = true, StartGame = false };

            foreach (var item in update)
            {
                //the lobby isn't awaiting for players anymore
                item.IsWaitingForPlayers = false;
                //the game is started
                item.StartGame = true;


                //static to see if the count is fout
                while (pl.Player.Count != 4)
                {
                    //add static players until player count is four
                    AddPlayer("hello");
                    pl.Player.Add(new DTO.Player() { PlayerName = "Hello", AlreadExist = false, PlayerId = GenerateID() });
                }
            }

            //static add the playerlobby pl
            gamelobbies.Add(pl);
            //select the lobbyid from the gamelobby
            var lobby = from d in gamelobbies where d.LobbyId.LobbyId == lst select d;

            foreach (var item in lobby)
            {
                //set the bools right
                item.IsAwaitingForPlayers = false;
                item.StartGame = true;
            }

            //intitiates the game
            #region Game init()
            //get the gamelobby
            var gamelob = (from p in gamelobbies where p.HostPlayer.PlayerId == Host.PlayerId select p).First();
            //make a playerlobby of the host and the lobby
            DTO.PlayerLobby plLobby = ConvertToDTOPlayerLobby(Host, lobby);
            NewGame = new DTO.MonopolyEngine.SingleGame();
            //Update the players of the game
            NewGame.UpdatePlayers(plLobby);
            //stat the game
            NewGame.StartGame();

            #endregion

            dc.SubmitChanges();
        }

        #endregion

        //Game functions
        #region MonopolyFunctions()

        //buytile function
        public void BuyTile()
        {
            if (NewGame.Started)
            {
                NewGame.BuyActiveTile();
            }
        }

        public void EndOfTurn()
        {
            if (NewGame.Started)
            {
                NewGame.Turnchange();
            }
        }

        //rolldice function
        public void RollDice()
        {
            if (NewGame.Started)
            {
                NewGame.Dice();
            }
        }

        #endregion

        #endregion
    }
}
