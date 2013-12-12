using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicktest
{
    class Program
    {
        static void Main(string[] args)
        {
            DTO.Lobby Lobby = new DTO.Lobby() { LobbyId = 1, LobbyName = "activelobby" };
            DTO.Player TestHost = new DTO.Player() { AlreadExist = false, PlayerId = 11, PlayerName = "john" };
            DTO.Player TestPlayer1 = new DTO.Player() { AlreadExist = false, PlayerId = 23, PlayerName = "Mary" };
            DTO.Player TestPlayer2 = new DTO.Player() { AlreadExist = false, PlayerId = 42, PlayerName = "Hendry" };
            DTO.Player TestPlayer3 = new DTO.Player() { AlreadExist = false, PlayerId = 56, PlayerName = "Sandra" };
            DTO.PlayerLobby TestLobby = new DTO.PlayerLobby(TestHost);
            TestLobby.Player.Add(TestPlayer1);
            TestLobby.Player.Add(TestPlayer2);
            TestLobby.Player.Add(TestPlayer3);
            DTO.MonopolyEngine.SingleGame TestGame = new DTO.MonopolyEngine.SingleGame();
            TestGame.UpdatePlayers(TestLobby);
            TestGame.StartGame();
            Console.WriteLine(TestGame.publicState.ActivePlayer.PlayerName);
            TestGame.Dice();
            foreach (var item in TestGame.publicState.lastDieRoll)
            {
                Console.WriteLine(item.ToString());	
            }
            Console.WriteLine(TestGame.publicState.ReturnPlayerByOrder(TestGame.publicState.ActiveGamePlayer).Location.ToString());
            Console.WriteLine(TestGame.publicState.ActiveTileName);
            Console.WriteLine(TestGame.publicState.EnableBuy.ToString());
            if (TestGame.publicState.EnableBuy)
            {
                TestGame.BuyActiveTile();
            }
            Console.WriteLine(TestGame.publicState.ReturnPlayerByOrder(TestGame.publicState.ActiveGamePlayer).Cash.ToString());
            foreach (var item in TestGame.publicState.ReturnPlayerByOrder(TestGame.publicState.ActiveGamePlayer).PlayerProperty)
            {
                Console.WriteLine(item.ID);
                Console.WriteLine(TestGame.publicState.LocalCardData[item.ID].Name);
                //Console.WriteLine(item.Morguage.ToString());
                //TestGame.MorguageToggle(TestGame.publicState.ActivePlayer, item.ID);
                //Console.WriteLine(item.Morguage.ToString());
            }
            Console.WriteLine(TestGame.publicState.ReturnPlayerByOrder(TestGame.publicState.ActiveGamePlayer).Cash.ToString());
            

            TestGame.Turnchange();
            StandardTurn(TestGame);
            TestGame.Turnchange();
            StandardTurn(TestGame);
            TestGame.Turnchange();
            StandardTurn(TestGame);
            TestGame.Turnchange();
            StandardTurn(TestGame);
            //int t = 0;
            //while (t < 40)
            //{
            //    TestGame.Turnchange();
            //    StandardTurn(TestGame);
            //    t++;
            //}
            DTO.MonopolyEngine.GamePlayer temp = TestGame.publicState.ReturnPlayerByOrder(TestGame.publicState.ActiveGamePlayer);
            if (temp.PlayerProperty.Count > 0)
            {
                TestGame.RequestTrade(TestLobby.Player[1], (byte)temp.PlayerProperty[0].ID, false, 5000);
                Console.WriteLine(TestGame.publicState.PropertyTradeRequested.ToString());
                TestGame.AcceptTrade();
                Console.WriteLine(TestGame.publicState.PropertyTradeRequested.ToString());
                Console.Write(temp.MyPlayer.PlayerName);
                Console.Write(" : ");
                Console.WriteLine(temp.Cash.ToString());
                foreach (var item in temp.PlayerProperty)
                {
                    foreach (var item2 in temp.MyState.LocalCardData)
                    {
                        if (item.ID == item2.ID)
                        {
                            Console.WriteLine(item2.Name);
                        }
                    }
                }

                DTO.MonopolyEngine.GamePlayer temp2 = temp.MyState.ReturnPlayerByBasePlayer(TestLobby.Player[1]);
                Console.Write(temp2.MyPlayer.PlayerName);
                Console.Write(" : ");
                Console.WriteLine(temp2.Cash.ToString());
                foreach (var item in temp2.PlayerProperty)
                {
                    foreach (var item2 in temp.MyState.LocalCardData)
                    {
                        if (item.ID == item2.ID)
                        {
                            Console.WriteLine(item2.Name);
                        }
                    }
                }
            }

            TestGame.Quit(TestGame.publicState.ActivePlayer);
            TestGame.Turnchange();
            //foreach (var item in TestGame.publicState.PlayerList)
            //{
            //    Console.WriteLine(item.Cash.ToString());
            //}

            //for (int i = 0; i < TestGame.publicState.IsBought.Length; i++)
            //{
            //    Console.Write("{0} ::  ",i);
            //    Console.WriteLine(TestGame.publicState.IsBought[i].ToString());
            //}
            foreach (var item in TestGame.publicState.Notificationlog)
            {
                Console.WriteLine(item);
            }
            //Console.WriteLine(TestGame.publicState.ActivePlayer.PlayerName);

            Console.ReadLine();
            
        }
        public static void showDB(DTO.MonopolyEngine.SingleGame TestGame)
        {
            
            foreach (var item in TestGame.publicState.LocalCardData)
            {
                Console.Write(item.BuyCost.ToString());
                Console.Write("  :  ");
                Console.Write(item.ID.ToString());
                Console.Write("  :  ");
                Console.WriteLine(item.Name);
            }
        }
        public static void StandardTurn(DTO.MonopolyEngine.SingleGame TestGame)
        {
            Console.WriteLine(TestGame.publicState.ActivePlayer.PlayerName);
            TestGame.Dice();
            if (TestGame.publicState.lastDieRoll != null)
            {
                foreach (var item in TestGame.publicState.lastDieRoll)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            else
            {
                Console.WriteLine("tojail");
            }
            Console.WriteLine(TestGame.publicState.ReturnPlayerByOrder(TestGame.publicState.ActiveGamePlayer).Location.ToString());
            Console.WriteLine(TestGame.publicState.ActiveTileName);
            Console.WriteLine(TestGame.publicState.EnableBuy.ToString());
            if (TestGame.publicState.EnableBuy)
            {
                TestGame.BuyActiveTile();
            }
            Console.WriteLine(TestGame.publicState.ReturnPlayerByOrder(TestGame.publicState.ActiveGamePlayer).Cash.ToString());
            foreach (var item in TestGame.publicState.ReturnPlayerByOrder(TestGame.publicState.ActiveGamePlayer).PlayerProperty)
            {
                Console.WriteLine(item.ID);
                Console.WriteLine(TestGame.publicState.LocalCardData[item.ID].Name);
                //Console.WriteLine(item.Morguage.ToString());
                //TestGame.MorguageToggle(TestGame.publicState.ActivePlayer, item.ID);
                //Console.WriteLine(item.Morguage.ToString());
            }
            Console.WriteLine(TestGame.publicState.ReturnPlayerByOrder(TestGame.publicState.ActiveGamePlayer).Cash.ToString());
        }
        
    }
}
