using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace LobbyService.Web.DTO.MonopolyEngine
{
    [DataContract]
    public class SingleGame
    {
        public List<Player> MyPlayers { get; set; }
        private GameState _privateState;
        [DataMember]
        public GameState publicState
        {
            get
            {
                return _privateState;
            }
        }

        public SingleGame(DTO.PlayerLobby activeLobby)
        {
            MyPlayers = new List<Player>();
            MyPlayers = activeLobby.Player;
        }
        public void StartGame()
        {
            _privateState = new GameState(MyPlayers);
        }

        public void Dice()
        {
            GameFunctions.castPlayerDie(publicState.ReturnPlayerByOrder(publicState.ActiveGamePlayer), this);
        }


    }
}