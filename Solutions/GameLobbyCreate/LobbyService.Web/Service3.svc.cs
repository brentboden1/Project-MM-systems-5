using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LobbyService.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service3" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service3.svc or Service3.svc.cs at the Solution Explorer and start debugging.
    public class Service3 : IService3
    {
        public void DoWork()
        {
        }

        private DataClasses1DataContext dc;
        public Service3()
        {
            dc = new DataClasses1DataContext();
        }

        private static Random _Dice = new Random();
        public static int Dice()
        {
            int result = 0;

            result = _Dice.Next(1, 6);

            return result;
        }

        private static void checkDoubleDice(Player pl)
        {
            if (pl.DiceOne == pl.DiceTwo)
            {
                if ((bool)pl.OnceDouble)
                {
                    if ((bool)pl.TwiceDouble)
                    {
                        pl.Jail = true;
                    }
                    pl.TwiceDouble = true;
                }
                pl.OnceDouble = true;
            }
            else
            {
                pl.Jail = false;
                pl.OnceDouble = false;
                pl.TwiceDouble = false;
            }
        }

        private void CopyPlayerProperties(Player item, Player p)
        {
            p.DiceEyes = item.DiceEyes;
            p.DiceOne = item.DiceOne;
            p.DiceTwo = item.DiceTwo;
            p.IsDiceRolling = item.IsDiceRolling;
            p.PlayerId = item.PlayerId;
            p.PlayerName = item.PlayerName;
            p.AlreadExist = item.AlreadExist;
            p.OnceDouble = item.OnceDouble;
            p.TwiceDouble = item.TwiceDouble;
            p.Jail = item.Jail;
        }

        public void RollDice(int id)
        {
            var pl = from player in dc.Players select player;

            Player p = new Player();


            foreach (var item in pl)
            {
                if ((int)item.PlayerId == id)
                {
                    item.IsDiceRolling = true;
                    item.DiceOne = Dice();
                    item.DiceTwo = Dice();

                    checkDoubleDice(item);

                    item.DiceEyes = item.DiceOne + item.DiceTwo;

                    CopyPlayerProperties(item, p);

                    dc.Players.DeleteOnSubmit(item);
                    dc.Players.InsertOnSubmit(p);
                    dc.SubmitChanges();
                }
            }
        }

        public List<DTO.Player> ShowDiceRoll()
        {
            var p = from pl in dc.Players 
                    where pl.PlayerId == 1
                    select pl;

            List<DTO.Player> players = new List<DTO.Player>();

            foreach (var item in p)
            {
                DTO.Player pl = new DTO.Player() { PlayerId = (int)item.PlayerId, PlayerName = item.PlayerName, AlreadExist = (bool)item.AlreadExist, DiceEyes = (int)item.DiceEyes, DiceOnceDouble = (bool)item.OnceDouble, DiceOne = (int)item.DiceOne, DiceTwiceDouble = (bool)item.TwiceDouble, DiceTwo = (int)item.DiceTwo, IsDiceRolling = (bool)item.IsDiceRolling, Jail = (bool)item.Jail };

                players.Add(pl);
            }

            return players;
        }
    }
}
