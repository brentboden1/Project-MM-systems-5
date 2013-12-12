using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicktest.DTO.MonopolyEngine
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
        public byte PrisonTime { get; set; }
        public bool IsPrison { get; set; }
        public bool IsActive { get; set; }        
        public bool IsPlaying { get; set; }
        public bool HasEscapePrisonCh { get; set; }
        public bool HasEscapePrisonCo { get; set; }
        private int _cash;
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
