using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LobbyService.Web.DTO.MonopolyEngine
{
    public class GameFunctions
    {

        #region Enums
        public enum TurnState
        {
            BeginPhase,
            BuyPhase,
            TradePhase,
            WaitPhase,
            EndPhase
        }
        public enum Direction
        {
            nulled,
            indir,
            outdir
        }
        #endregion
        public static void BoardEffect(GamePlayer gplayer, GameState State)
        {
            switch (gplayer.Location)
            {
                case 0:
                    StandardCash(gplayer);
                    State.ActiveTileName = "Start";
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                case 2:
                case 17:
                case 33:
                    State.NewCommunal(gplayer);
                    State.ActiveTileName = "Algemeen Fonds";
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                case 7:
                case 22:
                case 36:
                    State.NewChance(gplayer);
                    State.ActiveTileName = "Kans";
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                case 10:
                    State.ActiveTileName = "Op Bezoek";
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                case 20:
                    State.ActiveTileName = "Gratis Parkeren";
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                case 30:
                    State.ActiveTileName = "Naar Gevangenis";
                    toJail(gplayer);
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                case 4:
                    State.ActiveTileName = "Inkomsten Belasting";
                    gplayer.Cash -= 4000;
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                case 38:
                    State.ActiveTileName = "Extra Belastingen";
                    gplayer.Cash -= 2000;
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                default:
                    int temp = gplayer.Location;
                    var house = (from h in State.LocalCardData
                                 where h.Position == temp
                                 select h).FirstOrDefault();
                    State.ActiveTileName = house.Name;
                    CardEffect(house.ID, State, gplayer);
                    break;
            }
        }
        #region PropertyEffects
        public static void CardEffect(int localID, GameState State, GamePlayer gPlayer)
        {
            var house = (from h in State.LocalCardData
                         where h.ID == localID
                         select h).FirstOrDefault();
            if (State.IsBought[localID] == false)
            {
                State.EnableBuy = true;
                State.CurrentPhase = TurnState.BuyPhase;
            }
            else
            {
                GamePlayer owner = State.ReturnPlayerByOrder((byte)State.Ownership[localID]);
                if (owner != gPlayer)
                {
                    PayRent(house, gPlayer, owner);
                }
                State.CurrentPhase = TurnState.WaitPhase;
            }
        }
        private static void PayRent(HouseCardData house, GamePlayer payingPlayer, GamePlayer landLord)
        {
            int cashToPay = 0;
            int diceout = 0;
            foreach (var item in payingPlayer.MyState.lastDieRoll)
            {
                diceout += item;
            }
            OwnedProperty prop = (from h in landLord.PlayerProperty
                                  where h.ID == house.ID
                                  select h).FirstOrDefault();
            if (!prop.Morguage)
            {
                switch (house.Type)
                {
                    case "Standard":
                        switch (prop.HouseLevel)
                        {
                            case 0:
                                if (prop.SetLevel == 1)
                                {
                                    cashToPay = house.Rent0 * 2;
                                }
                                else
                                {
                                    cashToPay = house.Rent0;
                                }
                                break;
                            case 1:
                                cashToPay = house.Rent1;
                                break;
                            case 2:
                                cashToPay = house.Rent2;
                                break;
                            case 3:
                                cashToPay = house.Rent3;
                                break;
                            case 4:
                                cashToPay = house.Rent4;
                                break;
                            case 5:
                                cashToPay = house.Rent5;
                                break;
                            default:
                                break;
                        }
                        break;
                    case "Station":
                        switch (prop.SetLevel)
                        {
                            case 0:
                                cashToPay = house.Rent0;
                                break;
                            case 1:
                                cashToPay = house.Rent1;
                                break;
                            case 2:
                                cashToPay = house.Rent2;
                                break;
                            case 3:
                                cashToPay = house.Rent3;
                                break;
                            default:
                                break;
                        }
                        break;
                    case "NutsVoorziening":
                        if (prop.SetLevel == 1)
                        {
                            cashToPay = house.Rent1 * diceout;
                        }
                        else
                        {
                            cashToPay = house.Rent0 * diceout;
                        }
                        break;
                    default:
                        break;
                }
            }
            payingPlayer.Cash -= cashToPay;
        }

        private static void SetlevelChange(GamePlayer gPlayer, int ID)
        {
            var house = (from h in gPlayer.MyState.LocalCardData
                         where h.ID == ID
                         select h).FirstOrDefault();
            int toCheck = house.Group;
            byte locallevel = 0;
            foreach (var item in gPlayer.PlayerProperty)
            {
                var Lhouse = (from h in gPlayer.MyState.LocalCardData
                              where h.ID == item.ID
                              select h).FirstOrDefault();
                if (Lhouse.Group == toCheck)
                {
                    locallevel++;
                }
            }
            bool updateLevel = false;
            byte leveltoset = 0;
            switch (toCheck)
            {
                case 1:
                case 8:
                case 10:
                    if (locallevel == 2)
                    {
                        updateLevel = true;
                        leveltoset = 1;
                    }
                    break;
                case 9:
                    if (locallevel == 2)
                    {
                        updateLevel = true;
                        leveltoset = 1;
                    }
                    if (locallevel == 3)
                    {
                        updateLevel = true;
                        leveltoset = 2;
                    }
                    if (locallevel == 4)
                    {
                        updateLevel = true;
                        leveltoset = 3;
                    }
                    break;
                default:
                    if (locallevel == 3)
                    {
                        updateLevel = true;
                        leveltoset = 1;
                    }
                    break;
            }
            if (updateLevel)
            {
                for (int i = 0; i < gPlayer.PlayerProperty.Count; i++)
                {
                    OwnedProperty temp = gPlayer.PlayerProperty[i];
                    var Thouse = (from h in gPlayer.MyState.LocalCardData
                                  where h.ID == temp.ID
                                  select h).FirstOrDefault();
                    if (Thouse.Group == leveltoset)
                    {
                        temp.SetLevel = leveltoset;
                    }

                }
            }


        }
        private static void PropertyOwnershipChange(int propertyID, GameState State, GamePlayer NewOwner, GamePlayer Oldowner = null)
        {
            if (Oldowner == null)
            {
                State.IsBought[propertyID] = true;
                State.Ownership[propertyID] = NewOwner.OrderNumber;
                NewOwner.PlayerProperty.Add(new OwnedProperty(propertyID));
            }
            else
            {
                OwnedProperty toRemove = null;
                State.Ownership[propertyID] = NewOwner.OrderNumber;
                NewOwner.PlayerProperty.Add(new OwnedProperty(propertyID));
                for (int i = 0; i < Oldowner.PlayerProperty.Count; i++)
                {
                    if (Oldowner.PlayerProperty[i].ID == propertyID)
                    {
                        toRemove = Oldowner.PlayerProperty[i];
                    }
                }
                if (toRemove != null)
                {
                    Oldowner.PlayerProperty.Remove(toRemove);
                }
            }
            SetlevelChange(NewOwner, propertyID);
        }
        #endregion
        #region StandardActions
        public static void ChangeActivePlayer(GameState State)
        {
            bool validPlayer = false;
            byte newOrder = 0;
            GamePlayer playerToChange;
            while (!validPlayer)
            {
                newOrder = 0;
                if (State.ActiveGamePlayer < State.PlayerList.Count - 1)
                {
                    newOrder = (byte)(State.ActiveGamePlayer + 1);
                }
                State.ActiveGamePlayer = newOrder;
                GamePlayer temp = State.ReturnPlayerByOrder(newOrder);
                if (temp.IsActive)
                {
                    validPlayer = true;
                }
            }
            playerToChange = State.ReturnPlayerByOrder(newOrder);
            State.ActiveGamePlayer = playerToChange.OrderNumber;
            State.ActivePlayer = playerToChange.MyPlayer;
            State.CurrentPhase = TurnState.BeginPhase;
            State.TurnNumber++;
            State.DieCast = false;
            State.EnableBuy = false;
            State.PropertyTradeRequested = 0;
            State.PlayerTradeRequested = null;
            if (playerToChange.PrisonTime >= 3)
            {
                PrisonRelease(playerToChange);
            }
        }
        public static void TradeRequested(GameState State, Player P, byte propID, bool propdir)
        {
            if (State.PlayerTradeRequested == null)
            {
                State.CurrentPhase = TurnState.TradePhase;
                State.PlayerTradeRequested = P;
                State.PropertyTradeRequested = propID;
                if (propdir)
                {
                    State.PropertyTradeDirection = Direction.indir;
                }
                else
                {
                    State.PropertyTradeDirection = Direction.outdir;
                }
            }
        }

        public static void startingOutfit(GameState State)
        {
            for (int i = 0; i < State.PlayerList.Count; i++)
            {
                State.PlayerList[i].Cash = 30000;
                State.PlayerList[i].IsPlaying = true;
                State.PlayerList[i].IsActive = true;
            }
        }
        public static void ToggleMorguage(Player P, GameState State, int ID)
        {
            GamePlayer local = State.ReturnPlayerByBasePlayer(P);
            OwnedProperty mProp = (from h in local.PlayerProperty
                                   where h.ID == ID
                                   select h).FirstOrDefault();
            if (mProp.Morguage)
            {
                Morguage(local, ID, true);
            }
            else
            {
                Morguage(local, ID);
            }
        }

        public static void StandardCash(GamePlayer gplayer)
        {
            gplayer.Cash += 4000;
        }
        public static void toJail(GamePlayer gplayer)
        {
            updateLocation(gplayer, 10, true);
            gplayer.IsPrison = true;
            gplayer.PrisonTime = 1;
        }
        public static void PrisonRelease(GamePlayer gplayer, bool payed = true)
        {
            gplayer.IsPrison = false;
            gplayer.PrisonTime = 0;
            if (payed)
            {
                gplayer.Cash -= 2000;
            }
        }
        public static void OnPotentialBankruptcy(GamePlayer gPlayer)
        {
        }
        public static bool IsActivePlayer(Player P, GameState State)
        {
            bool output = false;
            if (State.ReturnPlayerByBasePlayer(P).OrderNumber == State.ActiveGamePlayer)
            {
                output = true;
            }
            return output;
        }
        #endregion
        #region Transactions
        public static void BuyHouse(GamePlayer gPlayer, byte PropID)
        {
            OwnedProperty prop = (from h in gPlayer.PlayerProperty
                                  where h.ID == PropID
                                  select h).FirstOrDefault();
            var house = (from h in gPlayer.MyState.LocalCardData
                         where h.ID == PropID
                         select h).FirstOrDefault();
            if (prop.HouseLevel <= 5 && prop.Morguage == false && prop.SetLevel == 1)
            {
                gPlayer.Cash -= house.HouseCost;
                prop.HouseLevel++;
            }

        }
        public static void TradeAccepted(GameState State)
        {
            GamePlayer requester = State.ReturnPlayerByOrder(State.ActiveGamePlayer);
            GamePlayer accepter = State.ReturnPlayerByBasePlayer(State.PlayerTradeRequested);
            if (State.PropertyTradeDirection == Direction.indir)
            {
                PropertyOwnershipChange(State.PropertyTradeRequested, State, requester, accepter);
            }
            else
            {
                PropertyOwnershipChange(State.PropertyTradeRequested, State, accepter, requester);
            }
            State.PlayerTradeRequested = null;
            State.PropertyTradeRequested = 0;
            State.PropertyTradeDirection = Direction.nulled;
            State.CurrentPhase = TurnState.WaitPhase;
        }
        public static void TradeRejected(GameState State)
        {
            State.PlayerTradeRequested = null;
            State.PropertyTradeRequested = 0;
            State.PropertyTradeDirection = Direction.nulled;
            State.CurrentPhase = TurnState.WaitPhase;
        }
        public static void BuyPropertyFromBank(GameState State)
        {
            if (State.EnableBuy)
            {
                GamePlayer currentActive = State.ReturnPlayerByOrder(State.ActiveGamePlayer);
                var house = (from h in State.LocalCardData
                             where h.Position == currentActive.Location
                             select h).FirstOrDefault();
                PropertyOwnershipChange(house.ID, State, currentActive);
                currentActive.Cash -= house.BuyCost;
                State.CurrentPhase = TurnState.WaitPhase;
            }
        }
        private static void Morguage(GamePlayer Player, int propID, bool undo = false)
        {
            HouseCardData thishouse = (from h in Player.MyState.LocalCardData
                                       where h.ID == propID
                                       select h).FirstOrDefault();

            OwnedProperty local = (from h in Player.PlayerProperty
                                   where h.ID == propID
                                   select h).FirstOrDefault();
            if (!undo)
            {
                if (!local.Morguage)
                {
                    int updatedcash = 0;
                    local.Morguage = true;
                    updatedcash += local.HouseLevel * thishouse.HouseCost / 2;
                    local.HouseLevel = 0;
                    updatedcash += thishouse.BuyCost / 2;
                    Player.Cash += updatedcash;
                }
            }
            else
            {
                Player.Cash -= thishouse.BuyCost / 2;
                Player.Cash -= thishouse.BuyCost / 20;
                local.Morguage = false;
            }
        }

        #endregion
        #region LocationFunctions
        #region publicFunctions


        public static void updateLocation(GamePlayer P, int value, bool absoluteLocation = false)
        {
            if (!absoluteLocation)
            {
                P.Location = (byte)(P.Location + value);
            }
            else
            {
                P.Location = (byte)value;
            }
            BoardEffect(P, P.MyState);
        }

        public static void castPlayerDie(GamePlayer P, SingleGame S)
        {

            if (P.MyPlayer.PlayerId == S.publicState.ActivePlayer.PlayerId && S.publicState.DieCast == false)
            {
                S.publicState.lastDieRoll = DiceRoll();
                S.publicState.DieCast = true;
                int locVal = 0;
                if (P.IsPrison)
                {
                    if (S.publicState.lastDieRoll == null)
                    {
                        toJail(P);
                    }
                    else if (S.publicState.lastDieRoll[0] == S.publicState.lastDieRoll[1])
                    {
                        foreach (int value in S.publicState.lastDieRoll)
                        {
                            locVal = locVal + value;
                        }

                        updateLocation(P, locVal);
                    }
                    else
                    {
                        P.PrisonTime++;
                    }

                }
                else
                {
                    if (S.publicState.lastDieRoll != null)
                    {
                        foreach (int value in S.publicState.lastDieRoll)
                        {
                            locVal = locVal + value;
                        }

                        updateLocation(P, locVal);
                    }
                    else
                    {
                        toJail(P);
                    }
                }

            }
        }
        public static void randomStarterDie(GameState state)
        {
            if (!state.SetupComplete)
            {
                state.lastDieRoll = DiceRoll();
                if (state.lastDieRoll == null)
                {
                    state.lastDieRoll.Add(3);
                    state.lastDieRoll.Add(4);
                }
            }
        }
        #endregion
        #region DiceRoll()
        private static List<int> DiceRoll()
        {
            List<int> resultlist = new List<int>();
            try
            {
                bool prison = false;
                List<dice> diceList = new List<dice>();
                singleRoll(diceList, 2, 6);
                if (diceList[0].Value == diceList[1].Value)
                {
                    singleRoll(diceList, 2, 6);
                    if (diceList[2].Value == diceList[3].Value)
                    {
                        singleRoll(diceList, 2, 6);
                        if (diceList[4].Value == diceList[5].Value)
                        {
                            prison = true;
                        }
                    }
                }
                if (!prison)
                {
                    for (int i = 0; i < diceList.Count; i++)
                    {
                        resultlist.Add(diceList[i].Value);
                    }
                }
                else
                {
                    resultlist = null;
                }
                return resultlist;
            }
            catch (Exception)
            {
                resultlist.Add(3);
                resultlist.Add(4);
                return resultlist;
            }

        }
        private static void singleRoll(List<dice> Dlist, byte rollnumber, byte diceEyes)
        {
            for (int i = 0; i < rollnumber; i++)
            {
                Dlist.Add(new dice(diceEyes));
            }
        }
        #endregion
        #endregion       

    }
    class dice
    {
        static Random r = new Random();
        private byte _value;
        public byte Value
        {
            get
            {
                return _value;
            }
        }
        public dice(int dnumber)
        {

            _value = (byte)r.Next(1, dnumber + 1);
        }
    }
}