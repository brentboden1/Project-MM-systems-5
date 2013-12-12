using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace LobbyService.Web.DTO.MonopolyEngine
{
    [DataContract]
    public class GamePlayer
    {
        private GameState _mystate;
        [DataMember]
        public GameState MyState { get { return _mystate; } }
        private Player _myplayer;
        [DataMember]
        public Player MyPlayer { get { return _myplayer; } }
        private byte _ordernumber;
        [DataMember]
        public byte OrderNumber { get { return _ordernumber; } }
        private byte _location;
        [DataMember]
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
        [DataMember]
        public byte PrisonTime { get; set; }
        [DataMember]
        public bool IsPrison { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsPlaying { get; set; }
        [DataMember]
        public bool HasEscapePrisonCh { get; set; }
        [DataMember]
        public bool HasEscapePrisonCo { get; set; }
        private int _cash;
        [DataMember]
        public int Cash
        {
            get { return _cash; }
            set
            {
                if (value < 0)
                {
                    GameFunctions.OnPotentialBankruptcy(this);
                }
                //GameFunctions.updateLogCash(_cash, value, this);
                _cash = value;
            }
        }
        [DataMember]
        public List<OwnedProperty> PlayerProperty { get; set; }
        
        public GamePlayer(Player P, byte order, GameState State)
        {
            _mystate = State;
            PlayerProperty = new List<OwnedProperty>();
            _myplayer = P;
            _ordernumber = order;
            _location = 0;
            PrisonTime = 0;
            IsPrison = false;
            HasEscapePrisonCh = false;
            HasEscapePrisonCo = false;
        }
    }
}