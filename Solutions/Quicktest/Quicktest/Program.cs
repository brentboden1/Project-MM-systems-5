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
            DTO.MonopolyEngine.SingleGame TestGame = new DTO.MonopolyEngine.SingleGame(TestLobby);
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
            for (int i = 0; i < TestGame.publicState.IsBought.Length; i++)
            {
                Console.Write("{0} ::  ",i);
                Console.WriteLine(TestGame.publicState.IsBought[i].ToString());
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
        }
        
    }
}
