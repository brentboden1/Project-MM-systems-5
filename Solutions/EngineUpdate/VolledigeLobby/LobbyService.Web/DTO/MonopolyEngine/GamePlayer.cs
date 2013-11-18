using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace LobbyService.Web.DTO.MonopolyEngine
{
    
    public class GamePlayer
    {
        private GameState _mystate;
        public GameState MyState { get { return _mystate; } }
        private Player _myplayer;
        public Player MyPlayer { get { return _myplayer; } }
        private byte _ordernumber;
        public byte OrderNumber { get { return _ordernumber; } }
        private byte _location;
        public byte Location
        {
            get { return _location; }
            set
            {
                if (value > 39)
                {
                    value = (byte)(value - 39);
                    GameFunctions.StandardCash(this);
                }
                _location = value;
            }
        }
        public bool IsPrison { get; set; }
        public bool IsActive { get; set; }
        public bool IsEscapePrison { get; set; }
        public bool IsPlaying { get; set; }
        public int Cash { get; set; }
        public List<OwnedProperty> PlayerProperty { get; set; }
        public GamePlayer(Player P, byte order,GameState State)
        {
            _mystate = State;
            PlayerProperty = new List<OwnedProperty>();
            _myplayer = P;
            _ordernumber = order;

        }
    }
}