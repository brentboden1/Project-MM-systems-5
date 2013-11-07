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

        private int value;

        private DataClasses1DataContext dc;
        public Service1()
        {
            dc = new DataClasses1DataContext();
        }

        #endregion

        #region private methods()

        private int GenerateID()
        {
            var player = from p in dc.Players select p;
            value = 0;

            for (int i = 0; i < player.Count() + 1; i++)
            {
                foreach (var item in player)
                {
                    if (!(item.PlayerId == i))
                    {
                        value = i;
                    }
                }
            }

            return value;
        }

        private bool CheckIfPlayerAlreadyExists(string p)
        {
            var player = from play in dc.Players select play;
            bool result = false;
            foreach (var item in player)
            {
                if(string.Equals(p, item.PlayerName))
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

        #region Public Methods()

        public void DoWork()
        {
        }

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
    }
}
