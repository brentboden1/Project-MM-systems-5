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
        public DataClasses1DataContext publicDC { get { return dc; } }
        private int _revisionnumber;
        //nuttig
        public int RevisionNumber { get { return _revisionnumber; } }
        //nuttig
        public int TurnNumber { get; set; }
        //nuttig
        public Player ActivePlayer { get; set; }
        public byte ActiveGamePlayer { get; set; }
        //nuttig
        [DataMember]
        public List<int> lastDieRoll { get; set; }
        public bool DieCast { get; set; }
        private bool _setupComplete = false;
        public bool SetupComplete { get { return _setupComplete; } }
        //nuttig
        public GameFunctions.TurnState CurrentPhase { get; set; }
        private List<GamePlayer> _playerlist;
        public List<GamePlayer> PlayerList { get { return _playerlist; } }
        private byte _playernum;
        public byte PlayerNum { get { return _playernum; } }
        public List<string> ChatLog { get; set; }
        public string ActiveTileName { get; set; }
        public bool[] IsBought;
        public Nullable<byte>[] Ownership;
        #endregion
        #region functions
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
        public void GiveRandomLocationTest()
        {
            Random r = new Random();
            for (int i = 0; i < PlayerList.Count; i++)
            {
                PlayerList[i].Location = (byte)r.Next(1,39);
            }
        }
        public void NewCommunal(GamePlayer gplayer)
        {

        }

        public void NewChance(GamePlayer gplayer)
        {

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

        public void Update()
        {
            _revisionnumber++;

        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
        #endregion
}