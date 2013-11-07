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
        public void DoWork()
        {
        }

        private DataClasses1DataContext dc;
        public Service2()
        {
            dc = new DataClasses1DataContext();
        }

        public void CreateLobby(DTO.Player host, int lobby)
        {
            try
            {
                PlayerLobby pl = new PlayerLobby();
                pl.HostPlayer = (int)host.PlayerId;
                pl.LobbyId = lobby;
                pl.PlayerId = (int)host.PlayerId;
                pl.IsWaitingForPlayers = true;

                Lobby alobby = new Lobby();
                alobby.LobbyName = host.PlayerName;
                alobby.LobbyId = lobby;


                dc.Lobbies.InsertOnSubmit(alobby);
                dc.PlayerLobbies.InsertOnSubmit(pl);
                dc.SubmitChanges();
            }
            catch (Exception)
            {
            }
        }

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

        public DTO.Player GetPlayer(string name)
        {
            var pl = (from p in dc.Players
                      where p.PlayerName == name
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
            var lobbies = from p in dc.PlayerLobbies select p;
            if (lobbies != null)
            {
                foreach (var item in lobbies)
                {
                    dc.PlayerLobbies.DeleteOnSubmit(item);
                }
                dc.SubmitChanges();
            }
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
    }
}
