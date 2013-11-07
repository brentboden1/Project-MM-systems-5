using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LobbyService.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service2" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service2.svc or Service2.svc.cs at the Solution Explorer and start debugging.
    public class Service2 : IService2
    {
        #region Initialisation

        private int value;
        private DataClasses1DataContext dc;
        private static List<DTO.PlayerLobby> gamelobbies = new List<DTO.PlayerLobby>();
        public Service2()
        {
            dc = new DataClasses1DataContext();
        }

        #endregion

        #region Private Methods

        private int getLobbyId()
        {
            var lobby = from p in dc.PlayerLobbies select p;
            value = 0;

            for (int i = 0; i < lobby.Count() + 1; i++)
            {
                foreach (var item in lobby)
                {
                    if (!(item.LobbyId == i))
                    {
                        value = i;
                    }
                }
            }

            return value;
        }

        #endregion

        #region Public Methods

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

                Lobby alobby = new Lobby();
                alobby.LobbyName = host.PlayerName;
                alobby.LobbyId = pl.LobbyId;

                

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
                        select new { pl.LobbyId, r.PlayerId, r.PlayerName };
            /*var rooms = from r in dc.PlayerLobbies
                          where r.IsWaitingForPlayers == true
                          select r;*/

            List<DTO.PlayerLobby> lobbies = new List<DTO.PlayerLobby>();

            foreach (var item in rooms)
            {
                DTO.PlayerLobby l = new DTO.PlayerLobby() { HostPlayer = new DTO.Player() { PlayerId = (int)item.PlayerId, PlayerName = item.PlayerName }, LobbyId = new DTO.Lobby() { LobbyId = item.LobbyId } };
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

        public void JoinLobbyRoom(DTO.Player pl, int hostID)
        {
            var plName = from p in dc.Players where p.PlayerId == hostID select p;
            int lobbyId = (from l in dc.Lobbies where l.LobbyName == pl.PlayerName select l.LobbyId).FirstOrDefault();

            try
            {
                PlayerLobby plobby = new PlayerLobby();
                plobby.HostPlayer = hostID;
                plobby.LobbyId = lobbyId;
                plobby.PlayerId = (int)pl.PlayerId;

                dc.PlayerLobbies.InsertOnSubmit(plobby);
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                
            }
        }

        public List<DTO.Player> ShowPlayersInLobbyRoom()
        {
            var pl = from p in dc.Lobbies 
                     join r in dc.PlayerLobbies
                     on p.LobbyId equals r.LobbyId 
                     join l in dc.Players
                     on p.LobbyId equals l.PlayerId
                     where r.IsWaitingForPlayers == true 
                     select new {l.PlayerName, l.PlayerId };

            List<DTO.Player> players = new List<DTO.Player>();
            
            foreach (var item in pl)
            {
                players.Add(new DTO.Player() { PlayerId = item.PlayerId, PlayerName = item.PlayerName });
            }

            return players;
        }

        #endregion

        #endregion
    }
}
