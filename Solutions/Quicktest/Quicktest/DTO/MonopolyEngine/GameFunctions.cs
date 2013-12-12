using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicktest.DTO.MonopolyEngine
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
                    State.EventNotification = gplayer.MyPlayer.PlayerName + " is op " + State.ActiveTileName + " geland";
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                case 2:
                case 17:
                case 33:
                    State.NewCommunal(gplayer);
                    State.ActiveTileName = "Algemeen Fonds";
                    State.EventNotification = gplayer.MyPlayer.PlayerName + " is op " + State.ActiveTileName + " geland";
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                case 7:
                case 22:
                case 36:
                    State.NewChance(gplayer);
                    State.ActiveTileName = "Kans";
                    State.EventNotification = gplayer.MyPlayer.PlayerName + " is op " + State.ActiveTileName + " geland";
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                case 10:
                    State.ActiveTileName = "Op Bezoek";
                    State.EventNotification = gplayer.MyPlayer.PlayerName + " is op " + State.ActiveTileName + " geland";
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                case 20:
                    State.ActiveTileName = "Gratis Parkeren";
                    State.EventNotification = gplayer.MyPlayer.PlayerName + " is op " + State.ActiveTileName + " geland";
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                case 30:
                    State.ActiveTileName = "Naar Gevangenis";
                    State.EventNotification = gplayer.MyPlayer.PlayerName + " is op " + State.ActiveTileName + " geland";
                    toJail(gplayer);
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                case 4:
                    State.ActiveTileName = "Inkomsten Belasting";
                    State.EventNotification = gplayer.MyPlayer.PlayerName + " is op " + State.ActiveTileName + " geland";
                    PayBank(gplayer, 4000);                    
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                case 38:
                    State.ActiveTileName = "Extra Belastingen";
                    State.EventNotification = gplayer.MyPlayer.PlayerName + " is op " + State.ActiveTileName + " geland";
                    PayBank(gplayer, 2000);                    
                    State.CurrentPhase = TurnState.WaitPhase;
                    break;
                default:
                    int temp = gplayer.Location;
                    var house = (from h in State.LocalCardData
                                 where h.Position == temp
                                 select h).FirstOrDefault();
                    State.ActiveTileName = house.Name;
                    State.EventNotification = gplayer.MyPlayer.PlayerName + " is op " + State.ActiveTileName + " geland";
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
            payingPlayer.MyState.EventNotification = payingPlayer.MyPlayer.PlayerName + " betaalt " + cashToPay.ToString() +
                " huur aan " + landLord.MyPlayer.PlayerName;
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
                //State.EventNotification = NewOwner.MyPlayer.PlayerName + " koopt " + State.LocalCardData[propertyID].Name 
                   // + " van de bank";
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
        

        public static void updateLogCash(int initialVal, int newVal, GamePlayer gPlayer)
        {
            if (initialVal < newVal)
            {
                int temp = newVal - initialVal;
                gPlayer.MyState.EventNotification = gPlayer.MyPlayer.PlayerName + " verdient " + temp.ToString();
            }
            else if (initialVal > newVal)
            {
                int temp = initialVal - newVal;
                gPlayer.MyState.EventNotification = gPlayer.MyPlayer.PlayerName + " verliest " + temp.ToString();
            }
        }

        public static void ChangeActivePlayer(GameState State)
        {
            if (State.CurrentPhase != TurnState.EndPhase)
            {
                bool isGameFinished = false;
                GamePlayer GameOverTest = State.ReturnPlayerByOrder(State.ActiveGamePlayer);
                if (GameOverTest.Cash < 0)
                {
                    RemovePlayerFromGame(GameOverTest);
                }
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
                    //State.ActiveGamePlayer = newOrder;
                    GamePlayer temp = State.ReturnPlayerByOrder(newOrder);
                    if (temp.IsPlaying)
                    {
                        validPlayer = true;
                    }
                }
                playerToChange = State.ReturnPlayerByOrder(newOrder);
                if (playerToChange.OrderNumber == State.ActiveGamePlayer)
                {
                    isGameFinished = true;
                }
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
                State.EventNotification = playerToChange.MyPlayer.PlayerName + " is aan de beurt";
                if (isGameFinished)
                {
                    GameFinished(State);
                }
            }
        }
        public static void TradeRequested(GameState State, Player P, byte propID, bool propdir, int tvalue)
        {
            if (State.PlayerTradeRequested == null && State.CurrentPhase != TurnState.TradePhase)
            {
                State.CurrentPhase = TurnState.TradePhase;
                State.PlayerTradeRequested = P;
                State.PropertyTradeRequested = propID;
                State.PropertyTradeValue = tvalue;
                if (propdir)
                {
                    State.PropertyTradeDirection = Direction.indir;                    
                }
                else
                {
                    State.PropertyTradeDirection = Direction.outdir;
                }
                State.EventNotification = State.ReturnPlayerByOrder(State.ActiveGamePlayer).MyPlayer.PlayerName + " wilt handelen met " + P.PlayerName + " over " + State.LocalCardData[propID].Name +" voor "+ tvalue.ToString(); 
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
                State.EventNotification = P.PlayerName + " zet " + State.LocalCardData[ID].Name + " in hypoteek";
            }
            else
            {
                Morguage(local, ID);
                State.EventNotification = P.PlayerName + " betaalt de hypoteek op " + State.LocalCardData[ID].Name;
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
            gplayer.MyState.EventNotification = gplayer.MyPlayer.PlayerName + " gaat naar de gevangenis";
        }
        public static void PrisonRelease(GamePlayer gplayer, bool payed = true)
        {
            gplayer.IsPrison = false;
            gplayer.PrisonTime = 0;
            if (payed)
            {
                gplayer.Cash -= 2000;
            }
            else 
            {
                if (gplayer.HasEscapePrisonCh)
                {
                    ReturnEscapeJail(gplayer, 1);
                }
                else if (gplayer.HasEscapePrisonCo)
                {
                    ReturnEscapeJail(gplayer, 2);
                }
            }
            gplayer.MyState.EventNotification = gplayer.MyPlayer.PlayerName + " verlaat de gevangenis";
        }
        public static void OnPotentialBankruptcy(GamePlayer gPlayer)
        {            
            //currently unused, useful for future expansion            
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
        private static void PayBank(GamePlayer gPlayer, int amount)
        {
            gPlayer.Cash -= amount;
            gPlayer.MyState.EventNotification = gPlayer.MyPlayer.PlayerName + " betaalt " + amount.ToString() + " aan de bank";
        }

        private static void ReturnPropertyToBank(GamePlayer gPlayer, int PropId)
        {
            gPlayer.MyState.IsBought[PropId] = false;
            gPlayer.MyState.Ownership[PropId] = null;
            OwnedProperty toRemove = null;
            for (int i = 0; i < gPlayer.PlayerProperty.Count; i++)
            {
                if (gPlayer.PlayerProperty[i].ID == PropId)
                {
                    toRemove = gPlayer.PlayerProperty[i];
                }
            }
            if (toRemove != null)
            {
                gPlayer.PlayerProperty.Remove(toRemove);
            }

        }
        public static void BuyHouse(GamePlayer gPlayer, byte PropID)
        {            
            OwnedProperty prop = (from h in gPlayer.PlayerProperty
                                  where h.ID == PropID
                                  select h).FirstOrDefault();
            var house = (from h in gPlayer.MyState.LocalCardData
                                 where h.ID == PropID
                                 select h).FirstOrDefault();            
            if (prop.HouseLevel <= 5 && prop.Morguage == false && prop.SetLevel == 1 && house.Type == "Standard")
            {
                gPlayer.Cash -= house.HouseCost;
                prop.HouseLevel++;
            }
            gPlayer.MyState.EventNotification = gPlayer.MyPlayer.PlayerName + " koopt een huis op " + gPlayer.MyState.LocalCardData[PropID].Name;
        }
        public static void TradeAccepted(GameState State)
        {
            GamePlayer requester = State.ReturnPlayerByOrder(State.ActiveGamePlayer);
            GamePlayer accepter = State.ReturnPlayerByBasePlayer(State.PlayerTradeRequested);
            if (State.PropertyTradeDirection == Direction.indir)
            {
                PropertyOwnershipChange(State.PropertyTradeRequested, State, requester, accepter);
                requester.Cash -= State.PropertyTradeValue;
                accepter.Cash += State.PropertyTradeValue;
            }
            else
            {
                PropertyOwnershipChange(State.PropertyTradeRequested, State, accepter, requester);
                accepter.Cash -= State.PropertyTradeValue;
                requester.Cash += State.PropertyTradeValue;
            }
            State.EventNotification = requester.MyPlayer.PlayerName + " verhandelt " + State.LocalCardData[State.PropertyTradeRequested].Name + " met " + accepter.MyPlayer.PlayerName;
            State.PlayerTradeRequested = null;
            State.PropertyTradeRequested = 0;
            State.PropertyTradeDirection = Direction.nulled;
            State.CurrentPhase = TurnState.WaitPhase;
            State.PropertyTradeValue = 0;
            
        }
        public static void TradeRejected(GameState State)
        {
            State.PlayerTradeRequested = null;
            State.PropertyTradeRequested = 0;
            State.PropertyTradeDirection = Direction.nulled;
            State.CurrentPhase = TurnState.WaitPhase;
            State.PropertyTradeValue = 0;
            State.EventNotification = State.ReturnPlayerByOrder(State.ActiveGamePlayer).MyPlayer.PlayerName + " weigert te handelen met " + State.ReturnPlayerByBasePlayer(State.PlayerTradeRequested).MyPlayer.PlayerName;
        }
        public static void BuyPropertyFromBank(GameState State)
        {
            if (State.EnableBuy)
            {
                GamePlayer currentActive = State.ReturnPlayerByOrder(State.ActiveGamePlayer);
                var house = (from h in State.LocalCardData
                             where h.Position == currentActive.Location
                             select h).FirstOrDefault();
                State.EventNotification = currentActive.MyPlayer.PlayerName + " koopt " + house.Name
                    + " van de bank voor " + house.BuyCost.ToString();
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
        #region EventCardFunctions
        private static void ReturnEscapeJail(GamePlayer gPlayer, byte type)
        {
            switch (type)
            {
                case 0:
                    if (gPlayer.HasEscapePrisonCh)
                        gPlayer.MyState.ModifyPrisonCard(true, true);
                    if (gPlayer.HasEscapePrisonCo)
                        gPlayer.MyState.ModifyPrisonCard(false, true);
                    gPlayer.HasEscapePrisonCh = false;
                    gPlayer.HasEscapePrisonCo = false;    
                    
                    break;
                case 1:
                    if (gPlayer.HasEscapePrisonCh)
                        gPlayer.MyState.ModifyPrisonCard(true, true);
                    gPlayer.HasEscapePrisonCh = false;
                    
                    break;
                case 2:
                    if (gPlayer.HasEscapePrisonCo)
                        gPlayer.MyState.ModifyPrisonCard(false, true);
                    gPlayer.HasEscapePrisonCo = false;
                    
                    break;
                default:
                    break;
            }
        }
        public static void HandleCardEvent(GamePlayer gPlayer, EventCardData Card)
        {            
            if (Card.IsPrison)
            {
                toJail(gPlayer);
            }
            else if (Card.IsEscapePrison)
            {
                if (Card.Type == "kans")
                {
                    gPlayer.HasEscapePrisonCh = true;
                    gPlayer.MyState.ModifyPrisonCard(true, false);
                }
                else
                {
                    gPlayer.HasEscapePrisonCo = true;
                    gPlayer.MyState.ModifyPrisonCard(false, false);
                }
            }
            else if (Card.IsLocationChangeBW)
            {
                if (Card.SoftLocation > 0)
                {
                    gPlayer.Location -= (byte)Card.SoftLocation;
                    BoardEffect(gPlayer, gPlayer.MyState);
                }
                else
                {
                    updateLocation(gPlayer, (int)Card.HardLocation, true);

                }
            }
            else if (Card.IsLocationChangeFW)
            {
                if (Card.SoftLocation > 0)
                {
                    updateLocation(gPlayer, (int)Card.SoftLocation);
                }
                else
                {
                    if (gPlayer.Location > (int)Card.HardLocation)
                    {
                        int temp = (39 - gPlayer.Location) + (int)Card.HardLocation;
                        updateLocation(gPlayer, temp);
                    }
                    else
                    {
                        updateLocation(gPlayer, (int)Card.HardLocation, true);
                    }
                }
            }
            else if (Card.IsChanceChoice)
            {
                gPlayer.MyState.ChanceChoice = true;
            }
            else if (Card.CashChange != 0)
            {
                if (Card.IsGlobal)
                {
                    foreach (var item in gPlayer.MyState.PlayerList)
                    {
                        item.Cash -= Card.CashChange;
                        gPlayer.Cash += Card.CashChange;
                    }
                }
                else if (Card.IsPayHouse)
                {
                    int temp = 0;
                    foreach (var item in gPlayer.PlayerProperty)
                    {
                        temp += item.HouseLevel;
                    }
                    gPlayer.Cash -= temp * Card.CashChange;
                }
                else
                {
                    gPlayer.Cash += Card.CashChange;
                }
            }
            
        }
        public static void CardChoiceChance(bool choiceTaken, GamePlayer gPlayer)
        {
            if (gPlayer.MyState.ChanceChoice)
            {
                if (choiceTaken)
                {
                gPlayer.MyState.NewChance(gPlayer);
                }
                else
                {
                gPlayer.Cash += gPlayer.MyState.ActiveEventCard.CashChange;
                }
            gPlayer.MyState.ChanceChoice = false;
            }
        }
        #endregion
        #region GameOverFunctions
        private static void RemovePlayerFromGame(GamePlayer gPlayer)
        {
            gPlayer.MyState.EventNotification = gPlayer.MyPlayer.PlayerName + " is bankroet";
            gPlayer.IsPlaying = false;
            List<int> temp = new List<int>();
            foreach (var item in gPlayer.PlayerProperty)
            {
                temp.Add(item.ID);
                
            }
            foreach (var item in temp)
	        {
                ReturnPropertyToBank(gPlayer, item);		 
	        }
            ReturnEscapeJail(gPlayer, 0);
            gPlayer.IsPrison = false;
            gPlayer.PrisonTime = 0;
            gPlayer.Location = 10;
            gPlayer.Cash = 0;

            
        }
        public static void GameFinished(GameState State)
        {
            for (int i = 0; i < State.PlayerList.Count; i++)
            {
                State.PlayerList[i].IsPlaying = false;                
            }
            State.CurrentPhase = TurnState.EndPhase;
            State.EventNotification = "Het spel is beeindigt " + State.ActivePlayer.PlayerName + " heeft gewonnen";
        }

        public static void LeaveGame(GamePlayer gPlayer)
        {
            gPlayer.IsActive = false;
            RemovePlayerFromGame(gPlayer);
            gPlayer.MyState.EventNotification = gPlayer.MyPlayer.PlayerName + " geeft op en verlaat het spel";
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
            if (S.publicState.CurrentPhase != TurnState.EndPhase)
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
    #region HelperClasses
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
    static class MyExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
    #endregion 
}
