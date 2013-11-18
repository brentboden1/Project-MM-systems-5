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
        public bool EnableBuy { get; set; }
        public bool DieCast { get; set; }
        private bool _setupComplete = false;
        public bool SetupComplete { get { return _setupComplete; } }
        #endregion
        #region Numbers
        private byte _playernum;
        public byte PlayerNum { get { return _playernum; } }
        private int _revisionnumber;        
        public int RevisionNumber { get { return _revisionnumber; } }        
        public int TurnNumber { get; set; }  
        public byte ActiveGamePlayer { get; set; }
        #endregion        
        #region Lists
        public bool[] IsBought;
        public Nullable<byte>[] Ownership;
        private List<HouseCardData> _localcarddata;
        public List<HouseCardData> LocalCardData { get { return _localcarddata; } }
        private List<GamePlayer> _playerlist;
        public List<GamePlayer> PlayerList { get { return _playerlist; } }
        public List<string> ChatLog { get; set; }
        public List<int> lastDieRoll { get; set; }
        #endregion 
        #region generalInfo
        public GameFunctions.TurnState CurrentPhase { get; set; }
        public Player ActivePlayer { get; set; }
        public string ActiveTileName { get; set; }
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
                _playerlist.Add(new GamePlayer(player, i,this));
                i++;
            }
            _playernum = (byte)PlayerList.Count;
            IsBought = new bool[28];
            Ownership = new Nullable<byte>[28];
            fillLocalDB();
            setup();
        }

        private void setup()
        {
            _revisionnumber = 0;
            _setupComplete = true;
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

        

        public void Update()
        {
            _revisionnumber++;

        }

        public override string ToString()
        {
            return base.ToString();
        }
        #endregion
        #endregion
    }
        #endregion
}