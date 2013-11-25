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
        #region Initialisation()

        private int lobbyvalue;
        private int playervalue;
        private bool gameStart;
        private static List<DTO.PlayerLobby> gamelobbies = new List<DTO.PlayerLobby>();
        private static DTO.MonopolyEngine.SingleGame newGame;
        private DataClasses1DataContext dc;
        public Service1()
        {
            dc = new DataClasses1DataContext();
        }

        #endregion

        #region private methods()

        #region Player()

        private int GenerateID()
        {
            var player = from p in dc.Players select p;
            playervalue = 0;

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

        private bool CheckIfPlayerAlreadyExists(string p)
        {
            var player = from play in dc.Players select play;
            bool result = false;
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

        private DTO.Player ConvertToPlayer(Player p)
        {
            DTO.Player pl = new DTO.Player();
            pl.PlayerId = p.PlayerId;
            pl.PlayerName = p.PlayerName;
            return pl;
        }

        #endregion

        #region Lobby()

        private int getLobbyId()
        {
            var lobby = from p in dc.PlayerLobbies select p;
            lobbyvalue = 0;

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

        private void EnterLobby(DTO.Player pl, string lobbyName)
        {
            var l = (from d in dc.Lobbies where d.LobbyName == lobbyName select d.LobbyId).Single();

            if (lobbyName != null)
            {
                var lob = (from p in gamelobbies where p.LobbyId.LobbyId == (int)l select p).Single();

                if (lob != null)
                {
                    if (lob.StartGame != true)
                    {
                        lob.Player.Add(new DTO.Player() { PlayerId = pl.PlayerId, PlayerName = pl.PlayerName, AlreadExist = pl.AlreadExist });
                    }
                }
            }
        }

        private void CheckIfMaxPlayerExceeded(DTO.Player host)
        {
            var lobby = (from p in gamelobbies where p.LobbyId.LobbyName == host.PlayerName select p).Single();
            if (lobby.Player.Count() == 4)
            {
                lobby.StartGame = true;
            }
            else
            {
                lobby.StartGame = false;
            }
        }

        private string GetPlayerName(int playerId)
        {
            string pName = (from p in dc.Players where p.PlayerId == playerId select p.PlayerName).First();
            return pName;
        }

        #endregion

        #region Game()

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

        #region Public Methods()

        public void DoWork()
        {
        }

        #region PlayerCreation()

        public void AddPlayer(string playerName)
        {
            Player p = new Player();
            p.PlayerName = playerName;
            p.PlayerId = (int)GenerateID();
            p.AlreadExist = false;//CheckIfPlayerAlreadyExists(playerName);

            dc.Players.InsertOnSubmit(p);
            dc.SubmitChanges();

        }

        public List<DTO.Player> GetPlayers()
        {
            var pl = from p in dc.Players select p;

            List<DTO.Player> alist = new List<DTO.Player>();

            foreach (var item in pl)
            {
                alist.Add(ConvertToPlayer(item));
            }
            return alist;
        }

        public DTO.Player GetPlayerById(int id)
        {
            var pl = from p in dc.Players where p.PlayerId == id select p;

            DTO.Player player = new DTO.Player();

            foreach (var item in pl)
            {
                player = ConvertToPlayer(item);
            }

            return player;
        }

        public DTO.Player GetPlayerByName(string name)
        {
            var player = (from p in dc.Players where p.PlayerName == name select p).Single();
            return ConvertToPlayer(player);
        }

        #endregion

        #region Create Lobby()

        public void CreateLobby(DTO.Player host)
        {
            try
            {
                PlayerLobby pl = new PlayerLobby();
                pl.HostPlayer = (int)host.PlayerId;
                pl.LobbyId = getLobbyId();
                pl.PlayerId = (int)host.PlayerId;
                pl.IsWaitingForPlayers = true;
                pl.StartGame = false;

                Lobby alobby = new Lobby();
                alobby.LobbyName = host.PlayerName;
                alobby.LobbyId = pl.LobbyId;

                gamelobbies.Add(new DTO.PlayerLobby(host) { LobbyId = new DTO.Lobby() { LobbyId = alobby.LobbyId, LobbyName = alobby.LobbyName }, IsAwaitingForPlayers = true, StartGame = false });

                dc.Lobbies.InsertOnSubmit(alobby);
                dc.PlayerLobbies.InsertOnSubmit(pl);
                dc.SubmitChanges();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region ShowPlayLobbies()

        public List<DTO.PlayerLobby> GatAvailablePlayLobbies()
        {
            var rooms = from r in dc.Players
                        join pl in dc.PlayerLobbies
                        on r.PlayerId equals pl.HostPlayer
                        where pl.IsWaitingForPlayers == true
                        select new { pl.LobbyId, pl.HostPlayer};
            /*var rooms = from r in dc.PlayerLobbies
                          where r.IsWaitingForPlayers == true
                          select r;*/


            List<DTO.PlayerLobby> lobbies = new List<DTO.PlayerLobby>();

            foreach (var item in rooms)
            {
                DTO.PlayerLobby l = new DTO.PlayerLobby(new DTO.Player() { PlayerId = (int)item.HostPlayer, PlayerName = GetPlayerName((int)item.HostPlayer) }) { LobbyId = new DTO.Lobby() { LobbyId = item.LobbyId } };
                /* DTO.PlayerLobby l = new DTO.PlayerLobby();
                 l.HostPlayer = new DTO.Player() { PlayerId = (int)item.HostPlayer};
                 l.LobbyId = new DTO.Lobby() { LobbyId = (int)item.LobbyId };
                 l.IsAwaitingForPlayers = (bool)item.IsWaitingForPlayers;*/
                lobbies.Add(l);
            }

            return lobbies;
        }

        #endregion

        #region GetPlayer()

        public DTO.Player GetPlayer(string Name)
        {
            var pl = (from p in dc.Players
                      where p.PlayerName == Name
                      select p).Single();

            return new DTO.Player() { PlayerId = pl.PlayerId, PlayerName = pl.PlayerName };
        }

        public DTO.Player GetPlayerId(int id)
        {
            var pl = (from p in dc.Players
                      join pla in dc.PlayerLobbies
                      on p.PlayerId equals pla.HostPlayer
                      where p.PlayerId == id
                      select p).Single();

            return new DTO.Player() { PlayerId = pl.PlayerId, PlayerName = pl.PlayerName };
        }

        #endregion

        #region Delete()

        public void DeleteAllPlayers()
        {
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

        public void DeleteAllLobbies()
        {
            gamelobbies.Clear();
            var lobby = from l in dc.Lobbies select l;
            var lobbies = from p in dc.PlayerLobbies select p;
            if (lobbies != null)
            {
                foreach (var item in lobbies)
                {
                    dc.PlayerLobbies.DeleteOnSubmit(item);
                }
            }
            if (lobby != null)
            {
                foreach (var item in lobby)
                {
                    dc.Lobbies.DeleteOnSubmit(item);
                }
            }
            dc.SubmitChanges();
        }

        public void DeletePlayer(DTO.Player pl)
        {
            Player p = new Player();
            p.PlayerId = pl.PlayerId;
            p.PlayerName = pl.PlayerName;

            dc.Players.DeleteOnSubmit(p);
            dc.SubmitChanges();
        }

        public void DeletePlayerLobby(DTO.PlayerLobby pl)
        {
            PlayerLobby playlobby = new PlayerLobby();
            playlobby.LobbyId = pl.LobbyId.LobbyId;
            playlobby.HostPlayer = pl.HostPlayer.PlayerId;

            dc.PlayerLobbies.DeleteOnSubmit(playlobby);
            dc.SubmitChanges();
        }

        #endregion

        #region Join()

        public void JoinLobbyRoom(DTO.Player pl, DTO.Player Host)
        {
            // var plName = (from p in dc.Players where p.PlayerId == hostID select p.PlayerName).FirstOrDefault();
            int lobbyId = (from l in dc.Lobbies where l.LobbyName == Host.PlayerName select l.LobbyId).Single();

            try
            {
                CheckIfMaxPlayerExceeded(Host);

                PlayerLobby plobby = new PlayerLobby();
                plobby.HostPlayer = Host.PlayerId;
                plobby.LobbyId = lobbyId;
                plobby.PlayerId = (int)pl.PlayerId;
                plobby.IsWaitingForPlayers = true;
                plobby.StartGame = false;

                EnterLobby(pl, Host.PlayerName);

                dc.PlayerLobbies.InsertOnSubmit(plobby);
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public List<DTO.Player> ShowPlayersInLobbyRoom(int host)
        {
            /*  var pl = from p in dc.Lobbies
                       join r in dc.PlayerLobbies
                       on p.LobbyId equals r.LobbyId
                       join l in dc.Players
                       on r.LobbyId equals l.PlayerId
                       where r.IsWaitingForPlayers == true
                       select new { l.PlayerName, l.PlayerId };*/

            var pl = (from p in dc.Players where p.PlayerId == (int)host select p.PlayerId).First();

            var player = from o in dc.PlayerLobbies
                         join p in dc.Players
                         on o.PlayerId equals p.PlayerId
                         where o.HostPlayer == pl
                         select new { p.PlayerId, p.PlayerName };




            /* var pl = from p in dc.PlayerLobbies
                       join r in dc.Players
                       on p.PlayerId equals r.PlayerId
                       where p.LobbyId == host
                       select r;*/
            List<DTO.Player> players = new List<DTO.Player>();

            foreach (var item in player)
            {
                players.Add(new DTO.Player() { PlayerId = item.PlayerId, PlayerName = item.PlayerName });
            }

            return players;
        }

        #endregion

        #region Update()

        public List<int> GetGameUpdate(DTO.Player host)
        {
            var lst = (from l in gamelobbies where l.HostPlayer.PlayerId == host.PlayerId select l).First();

            List<int> Dice = new List<int>();

            if (lst.StartGame)
            {
                var dice = from p in newGame.publicState.lastDieRoll select p;
                foreach (var item in dice)
                {
                    Dice.Add(Convert.ToInt32(item));

                }
            }

            return Dice;
        }

        public int CheckPlayerCount(string lobby)
        {

            var l = (from d in dc.Lobbies where d.LobbyName == lobby select d.LobbyId).Single();
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

        #endregion

        #region GameStart()

        public void StartGame(DTO.Player Host)
        {
            
            int lst = (from l in dc.PlayerLobbies where l.HostPlayer == Host.PlayerId select l.LobbyId).First();
     //       var play = from p in gamelobbies where p.LobbyId.LobbyId == lst select p;
            var lobName = from p in dc.Lobbies where p.LobbyId == lst select p;

            var update = (from l in dc.PlayerLobbies where l.LobbyId == lst select l);

            //static
            DTO.PlayerLobby pl = new DTO.PlayerLobby(Host) { LobbyId = new DTO.Lobby() { LobbyId = lst, LobbyName = lobName.ToString() }, IsAwaitingForPlayers = true, StartGame = false };

            foreach (var item in update)
            {

                item.IsWaitingForPlayers = false;
                
                item.StartGame = true;
                //static
                if (pl.Player.Count != 4)
                {
                    AddPlayer("hello");
                    pl.Player.Add(new DTO.Player() { PlayerName = "Hello" + pl.Player.Count, AlreadExist = false, PlayerId = GenerateID() });
                }
            }

            //static
            gamelobbies.Add(pl);

            var lobby = from d in gamelobbies where d.LobbyId.LobbyId == lst select d;

            foreach (var item in lobby)
            {
                item.IsAwaitingForPlayers = false;
                item.StartGame = true;
            }

            #region Game init()

            DTO.PlayerLobby plLobby = ConvertToDTOPlayerLobby(Host, lobby);
            newGame = new DTO.MonopolyEngine.SingleGame(plLobby);
            newGame.StartGame();

            #endregion

            dc.SubmitChanges();
        }

        #endregion

        #region MonopolyFunctions()

        public void BuyTile()
        {
        }

        #endregion

        #endregion


        public int GetUpdate(DTO.Player host)
        {
            var lst = (from p in gamelobbies where p.HostPlayer.PlayerId == host.PlayerId select p).Single();

            return newGame.publicState.RevisionNumber;
        }
    }
}
