using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LobbyService.Web.DTO.MonopolyEngine
{
     [DataContract]
    public class StateToClient
    {

        // [DataMember]
         //public Player ActivePlayer { get; set; }

         [DataMember]
         public string ActiveTileName { get; set; }

         [DataMember]
         public GameFunctions.TurnState CurrentPhase { get; set; }

         [DataMember]
         public bool DieCast { get; set; }

         [DataMember]
         public bool EnableBuy { get; set; }

         [DataMember]
         public List<int> lastDieRoll { get; set; }

         [DataMember]
         public Player PlayerTradeRequested { get; set; }

         [DataMember]
         public byte PropertyTradeRequested { get; set; }

         [DataMember]
         public GameFunctions.Direction PropertyTradeDirection { get; set; }

         [DataMember]
         public int TurnNumber { get; set; }

        /* [DataMember]
         public List<DTO.MonopolyEngine.GamePlayer> PlayerList { get; set; }*/

           [DataMember]
         public GamePlayerToClient ActivePlayer { get; set; }

         [DataMember]
           public List<string> Log { get; set; }
    }
}