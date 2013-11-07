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
        private DataClasses1DataContext dc;
        public Service1()
        {
            dc = new DataClasses1DataContext();
        }

        public void DoWork()
        {
        }

        public void AddPlayer(DTO.Player pl)
        {
            Player p = new Player();
            p.PlayerId = (int)pl.PlayerId;
            p.PlayerName = pl.PlayerName;
            p.AlreadExist = false;

            bool b = false;

           b = CheckIfPlayerAlreadyExists(p);

           if (b)
           {
               dc.Players.DeleteOnSubmit(p);
               dc.SubmitChanges();
           }
           if (!b)
           {
               dc.Players.InsertOnSubmit(p);
               dc.SubmitChanges();
           }

        }

        private bool CheckIfPlayerAlreadyExists(Player p)
        {
            var player = from play in dc.Players select play;
            bool result = false;
            foreach (var item in player)
            {
                if (p.PlayerName == item.PlayerName)
                {
                    result = true;
                    p.AlreadExist = true;
                }
                else
                {
                    result = false;
                    p.AlreadExist = false;
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

        List<DTO.Player> IService1.GetPlayers()
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
    }
}
