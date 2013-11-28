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
            //TestGame.Dice();
            foreach (var item in TestGame.publicState.lastDieRoll)
            {
                Console.WriteLine(item.ToString());	
            }
            Console.WriteLine(TestGame.publicState.ActiveGamePlayer.ToString());
            Console.WriteLine(TestGame.publicState.ActivePlayer.PlayerName);
            Console.WriteLine(TestGame.publicState.PlayerList[0].Location.ToString());
            if (TestGame.ActiveTest(TestHost))
            {
                Console.WriteLine("TEST 1 TRUE");
            }
            if (!TestGame.ActiveTest(TestPlayer1))
            {
                Console.WriteLine("TEST 2 TRUE");
            }
            Console.WriteLine(TestGame.publicState.PlayerList[0].Cash.ToString());
            TestGame.Dice();
            TestGame.Dice();
            foreach (var item in TestGame.publicState.lastDieRoll)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine(TestGame.publicState.PlayerList[0].Location.ToString());
                
            Console.ReadLine();   
            
        }
    }
}
