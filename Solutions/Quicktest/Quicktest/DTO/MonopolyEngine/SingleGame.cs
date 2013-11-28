using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicktest.DTO.MonopolyEngine
{
    public class SingleGame
    {
        private bool _started;
        public bool Started { get { return _started; } }
        public List<Player> MyPlayers { get; set; }
        private GameState _privateState;
        //[DataMember]
        public GameState publicState { get { return _privateState; } }


        public SingleGame(PlayerLobby activeLobby)
        {
            _started = false;
            MyPlayers = new List<Player>();
            MyPlayers = activeLobby.Player;
        }
        public void UpdatePlayers(PlayerLobby activeLobby)
        {
            if (!Started)
            {
                MyPlayers = activeLobby.Player;
            }

        }
        public void StartGame()
        {
            _privateState = new GameState(MyPlayers);
            _started = true;
        }

        public void Dice()
        {
            GameFunctions.castPlayerDie(publicState.ReturnPlayerByOrder(publicState.ActiveGamePlayer), this);
        }

        public bool ActiveTest(Player testPlayer)
        {
            return GameFunctions.IsActivePlayer(testPlayer, this.publicState);
        }

        public void BuyActiveTile()
        {
            GameFunctions.BuyPropertyFromBank(this.publicState);
        }

        public void MorguageToggle(Player P, int ID)
        {
            GameFunctions.ToggleMorguage(P, this.publicState, ID);
        }
    }
}
