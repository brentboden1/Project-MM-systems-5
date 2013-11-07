using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LobbyService.Web.DTO
{
    [DataContract]
    public class Player
    {
        [DataMember]
        public string PlayerName { get; set; }

        [DataMember]
        public int PlayerId { get; set; }

        [DataMember]
        public bool AlreadExist { get; set; }

        [DataMember]
        public int DiceOne { get; set; }

        [DataMember]
        public int DiceTwo { get; set; }

        [DataMember]
        public bool DiceOnceDouble { get; set; }

        [DataMember]
        public bool DiceTwiceDouble { get; set; }

        [DataMember]
        public bool Jail { get; set; }

        [DataMember]
        public int DiceEyes { get; set; }

        [DataMember]
        public bool IsDiceRolling { get; set; }
    }
}