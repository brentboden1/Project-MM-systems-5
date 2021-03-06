﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicktest.DTO.MonopolyEngine
{
    public class GameState
    {

        #region variables
        private DataClasses1DataContext dc;
        #region Bools
        public bool EnableBuy { get; set; }
        public bool DieCast { get; set; }
        private bool _setupComplete = false;
        public bool SetupComplete { get { return _setupComplete; } }
        public bool ChanceChoice { get; set; }
        #endregion
        #region Numbers
        private byte _playernum;
        public byte PlayerNum { get { return _playernum; } }
        private int _revisionnumber;
        public int RevisionNumber { get { return _revisionnumber; } }
        public int TurnNumber { get; set; }
        public int PropertyTradeValue { get; set; }
        public byte ActiveGamePlayer { get; set; }
        public byte PropertyTradeRequested { get; set; }
        public GameFunctions.Direction PropertyTradeDirection { get; set; }
        #endregion
        #region Lists
        public bool[] IsBought;
        public Nullable<byte>[] Ownership;
        private List<HouseCardData> _localcarddata;
        public List<HouseCardData> LocalCardData { get { return _localcarddata; } }
        private List<EventCardData> _localkansdata;
        public List<EventCardData> LocalKansData { get { return _localkansdata; } }
        private List<EventCardData> _localfondsdata;
        public List<EventCardData> LocalFondsData { get { return _localfondsdata; } }
        private List<GamePlayer> _playerlist;
        public List<GamePlayer> PlayerList { get { return _playerlist; } }
        public List<string> ChatLog { get; set; }
        public List<int> lastDieRoll { get; set; }
        #endregion
        #region generalInfo
        public GameFunctions.TurnState CurrentPhase { get; set; }
        public Player ActivePlayer { get; set; }
        public string ActiveTileName { get; set; }
        public Player PlayerTradeRequested { get; set; }
        private string _eventnotification;
        private List<string> _notificationlog = new List<string>();
        public string EventNotification { get { return _eventnotification; } set { _eventnotification = value; _notificationlog.Add(value); } }
        public List<string> Notificationlog { get { return _notificationlog; } }
        public EventCardData ChancePrison { get; set; }
        public EventCardData CommPrison { get; set; }
        private EventCardData _activeEventCard;
        public EventCardData ActiveEventCard { get { return _activeEventCard; } }
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
            ActiveTileName = "Start";
            ChanceChoice = false;
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
            _localkansdata = new List<EventCardData>();
            var kans = (from k in dc.EventCardDatas
                        where k.Type == "kans"
                        select k);
            foreach (var item in kans)
            {
                _localkansdata.Add(item);
            }
            _localfondsdata = new List<EventCardData>();
            var fonds = (from f in dc.EventCardDatas
                         where f.Type == "fonds"
                         select f);
            foreach (var item in fonds)
            {
                _localfondsdata.Add(item);
            }
            ChancePrison = (from c in LocalKansData
                            where c.ID == 17
                            select c).FirstOrDefault();
            CommPrison = (from o in LocalFondsData
                          where o.ID == 0
                          select o).FirstOrDefault();
            _localfondsdata.Shuffle();
            _localkansdata.Shuffle();          
                         
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
            EventCardData temp = LocalFondsData[0];
            this.EventNotification = gplayer.MyPlayer.PlayerName + " trekt een algemeen fonds kaart";
            this.EventNotification = gplayer.MyPlayer.PlayerName + " : " + temp.Description;
            this._activeEventCard = temp;
            GameFunctions.HandleCardEvent(gplayer, LocalFondsData[0]);            
            LocalFondsData.Remove(temp);
            LocalFondsData.Add(temp);
            
        }

        public void NewChance(GamePlayer gplayer)
        {
            EventCardData temp = LocalKansData[0];
            this.EventNotification = gplayer.MyPlayer.PlayerName + " trekt een kans kaart";
            this.EventNotification = gplayer.MyPlayer.PlayerName + " : " + temp.Description;
            this._activeEventCard = temp;
            GameFunctions.HandleCardEvent(gplayer, LocalKansData[0]);            
            LocalKansData.Remove(temp);
            LocalKansData.Add(temp);
            
        }
        public void ModifyPrisonCard(bool kans, bool inset)
        {
            if (kans)
            {
                if (inset)
                {
                    _localkansdata.Add(ChancePrison);
                }
                else
                {
                    _localkansdata.Remove(ChancePrison);
                }
            }
            else
            {
                if (inset)
                {
                    _localfondsdata.Add(CommPrison);
                }
                else
                {
                    _localfondsdata.Remove(CommPrison);
                }
            }
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
