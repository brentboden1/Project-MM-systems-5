﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LobbyService.Web.DTO.MonopolyEngine
{
    public class GameFunctions
    {

        public enum TurnState
        {
            BeginPhase,
            BuyPhase,
            TradePhase,
            WaitPhase,
            EndPhase
        }        
        
        public static void BoardEffect(GamePlayer gplayer, GameState State)
        {
            switch (gplayer.Location)
            {
                case 0:
                    StandardCash(gplayer);
                    State.ActiveTileName = "Start";
                    break;
                case 2:
                case 17:
                case 33:
                    State.NewCommunal(gplayer);
                    State.ActiveTileName = "Algemeen Fonds";
                    break;                
                case 7:
                case 22:
                case 36:
                    State.NewChance(gplayer);
                    State.ActiveTileName = "Kans";
                    break;                
                case 10:
                    State.ActiveTileName = "Op Bezoek";
                    break;
                case 20:
                    State.ActiveTileName = "Gratis Parkeren";
                    break;
                case 30:
                    State.ActiveTileName = "Naar Gevangenis";
                    toJail(gplayer);
                    break;
                case 4:
                    State.ActiveTileName = "Inkomsten Belasting";
                    gplayer.Cash -= 4000;
                    break;
                case 38:
                    State.ActiveTileName = "Extra Belastingen";
                    gplayer.Cash -= 2000;
                    break;
                default:
                    int temp = gplayer.Location;
                    var house = (from h in State.LocalCardData
                                 where h.Position == temp
                                 select h).FirstOrDefault();
                    State.ActiveTileName = house.Name;
                    CardEffect(house.ID,State,gplayer);
                    break;
            }
        }
        #region PropertyEffects
        public static void CardEffect(int localID,GameState State,GamePlayer gPlayer)
        {
            var house = (from h in State.LocalCardData
                         where h.ID == localID
                         select h).FirstOrDefault();
            if (State.IsBought[localID] == false)
            {
                State.EnableBuy = true;
            }
            else
            {
                //payrent
                
            }
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
        public static void StandardCash(GamePlayer gplayer)
        {
            gplayer.Cash += 4000;
        }
        public static void toJail(GamePlayer gplayer)
        {
            updateLocation(gplayer, 10, true);
            gplayer.IsPrison = true;
        }
        public static void OnPotentialBankruptcy(GamePlayer gPlayer)
        {
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
        }

        public static void castPlayerDie(GamePlayer P, SingleGame S)
        {
            if (P.MyPlayer.PlayerId == S.publicState.ActivePlayer.PlayerId && S.publicState.DieCast == false)
            {
                S.publicState.lastDieRoll = DiceRoll();
                S.publicState.DieCast = true;
                int locVal = 0;
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