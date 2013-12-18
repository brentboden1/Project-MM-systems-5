using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace LobbyService.Web.DTO.MonopolyEngine
{
    [DataContract]
    public class SingleGame
    {
        private bool _started;
        [DataMember]
        public bool Started { get { return _started; } }
        [DataMember]
        public List<Player> MyPlayers { get; set; }
        private GameState _privateState;
        [DataMember]
        public GameState publicState { get { return _privateState; } }


        public SingleGame()
        {
            _started = false;
            MyPlayers = new List<Player>();
            //MyPlayers = activeLobby.Player;
        }
        //quick function to allow players to join after object creation
        public void UpdatePlayers(PlayerLobby activeLobby)
        {
            if (!Started)
            {
                MyPlayers = activeLobby.Player;
            }

        }
        //Needs to be called for proper initialisation of engine, hardlocks players
        public void StartGame()
        {
            _privateState = new GameState(MyPlayers);
            _started = true;
        }
        //Call handles dice throw and immediate unavoidable effects (paying rent, taking cards, jail) Manual Purchase required
        public void Dice()
        {
            GameFunctions.castPlayerDie(publicState.ReturnPlayerByOrder(publicState.ActiveGamePlayer), this);
        }
        //Small check to test if the calling player is active
        public bool ActiveTest(Player testPlayer)
        {
            return GameFunctions.IsActivePlayer(testPlayer, this.publicState);
        }
        //Attempts to buy the tile the active player is occupying, call results in no action if GameState.Enablebuy == false
        public void BuyActiveTile()
        {
            GameFunctions.BuyPropertyFromBank(this.publicState);
        }
        //1st call enables morguage, second call pays it back bind to same button or different
        public void MorguageToggle(Player P, int ID)
        {
            GameFunctions.ToggleMorguage(P, this.publicState, ID);
        }
        //to be called when player ends turn
        public void Turnchange()
        {
            GameFunctions.ChangeActivePlayer(this.publicState);
        }
        //Tradepartner = other player :: PropertyId is 0 - 27 byte value in housecarddatabase :: Dir = direction of trade True= In False=Out :: Amount is how much is requested
        public void RequestTrade(Player TradePartner, byte PropertyID, bool Dir, int Amount)
        {
            GameFunctions.TradeRequested(this.publicState, TradePartner, PropertyID, Dir, Amount);
        }
        //Requested Player Accepts Trade (needs clientside check for Publicstate.PlayerTradeRequested)
        public void AcceptTrade()
        {
            GameFunctions.TradeAccepted(this.publicState);
        }
        //Requested Player Rejects trade (opposite of AcceptTrade())
        public void RejectTrade()
        {
            GameFunctions.TradeRejected(this.publicState);
        }
        //need ID of the property the player wants to build a house on, call results in no action if property is morguage, player doesn't have a full set or house level is already at 5
        public void BuyHouse(Player P, byte PropertyID)
        {
            GamePlayer gplayer = publicState.ReturnPlayerByBasePlayer(P);
            GameFunctions.BuyHouse(gplayer, PropertyID);
        }
        //Player escapes prison manually, needs button prompt. Bool equals false if player has escape from jail card
        public void EscapePrison(Player P, bool payed = true)
        {
            GameFunctions.PrisonRelease(publicState.ReturnPlayerByBasePlayer(P), payed);
        }
        //Player draws a communal card with the choice between payment and picking a chance card: True equals chance chosen only if GameState.ChanceChoice is true
        public void ChanceCardChoice(bool choice, Player P)
        {
            GameFunctions.CardChoiceChance(choice, publicState.ReturnPlayerByBasePlayer(P));
        }
        //Player leaves the game
        public void Quit(Player P)
        {
            GameFunctions.LeaveGame(publicState.ReturnPlayerByBasePlayer(P));
        }
        public string GetPlayerName(int i)
        {
            return publicState.ReturnPlayerByOrder((byte)i).MyPlayer.PlayerName;
        }
        
    }
}