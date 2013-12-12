using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace LobbyService.Web.DTO.MonopolyEngine
{
    [DataContract]
    public class GameState
    {

        #region variables
        private DataClasses1DataContext dc;

        #region Bools
        [DataMember]
        public bool EnableBuy { get; set; }
        [DataMember]
        public bool DieCast { get; set; }
        private bool _setupComplete = false;
        [DataMember]
        public bool SetupComplete { get { return _setupComplete; } }
        #endregion

        #region Numbers
        private byte _playernum;
        [DataMember]
        public byte PlayerNum { get { return _playernum; } }
        private int _revisionnumber;
        [DataMember]
        public int RevisionNumber { get { return _revisionnumber; } }
        [DataMember]
        public int TurnNumber { get; set; }
        [DataMember]
        public byte ActiveGamePlayer { get; set; }
        [DataMember]
        public byte PropertyTradeRequested { get; set; }
        [DataMember]
        public GameFunctions.Direction PropertyTradeDirection { get; set; }
        #endregion

        #region Lists        
        public bool[] IsBought;
        public Nullable<byte>[] Ownership;
        private List<HouseCardData> _localcarddata;

        [DataMember]
        public List<HouseCardData> LocalCardData { get { return _localcarddata; } }

        private List<GamePlayer> _playerlist;
        [DataMember]
        public List<GamePlayer> PlayerList { get { return _playerlist; } }

        public List<string> ChatLog { get; set; }

        [DataMember]
        public List<int> lastDieRoll { get; set; }

        #endregion

        #region generalInfo
        [DataMember]
        public GameFunctions.TurnState CurrentPhase { get; set; }
        [DataMember]
        public Player ActivePlayer { get; set; }
        [DataMember]
        public string ActiveTileName { get; set; }
        [DataMember]
        public Player PlayerTradeRequested { get; set; }

        #endregion
        #endregion
        #region functions
        #region private/constructor
        public GameState(List<Player> Players)
        {
            dc = new DataClasses1DataContext();
            _playerlist = new List<GamePlayer>();
            ChatLog = new List<string>();
            byte i = 0;
            foreach (var player in Players)
            {
                _playerlist.Add(new GamePlayer(player, i, this));
                i++;
            }
            _playernum = (byte)PlayerList.Count;
            IsBought = new bool[28];
            Ownership = new Nullable<byte>[28];
            fillLocalDB();
            setup();
            GameFunctions.startingOutfit(this);
        }

        private void setup()
        {
            _revisionnumber = 0;

            for (int i = 0; i < IsBought.Length; i++)
            {
                IsBought[i] = false;
            }
            for (int i = 0; i < Ownership.Length; i++)
            {
                Ownership[i] = null;
            }
            ActivePlayer = ReturnPlayerByOrder(0).MyPlayer;
            ActiveGamePlayer = 0;
            GameFunctions.randomStarterDie(this);
            PropertyTradeRequested = 0;
            PlayerTradeRequested = null;
            PropertyTradeDirection = GameFunctions.Direction.nulled;
            _setupComplete = true;
        }
        private void fillLocalDB()
        {
            _localcarddata = new List<HouseCardData>();
            var house = (from h in dc.HouseCardDatas
                         select h);
            foreach (var item in house)
            {
                _localcarddata.Add(item);
            }
        }
        private void Update()
        {
            _revisionnumber++;

        }
        #endregion
        #region publicMethods
        #region GetInfo
        public GamePlayer ReturnPlayerByOrder(byte order)
        {
            GamePlayer outputplayer = null;
            foreach (var gplayer in PlayerList)
            {
                if (gplayer.OrderNumber == order)
                {
                    outputplayer = gplayer;
                }
            }
            return outputplayer;
        }
        public GamePlayer ReturnPlayerByBasePlayer(Player player)
        {
            GamePlayer outputplayer = null;
            foreach (var gplayer in PlayerList)
            {
                if (gplayer.MyPlayer == player)
                {
                    outputplayer = gplayer;
                }
            }
            return outputplayer;
        }
        public Player checkPropertyOwner(int ID)
        {
            Player p = null;
            GamePlayer temp = null;
            try
            {
                Nullable<byte> test = Ownership[ID];
                byte test2 = test.Value;
                temp = ReturnPlayerByOrder(test2);
                p = temp.MyPlayer;
            }
            catch (Exception)
            {

            }
            return p;
        }
        #endregion
        #region GameEffect
        public void NewCommunal(GamePlayer gplayer)
        {

        }

        public void NewChance(GamePlayer gplayer)
        {

        }





        public override string ToString()
        {
            return base.ToString();
        }
        #endregion
        #endregion
        #endregion
    }
}