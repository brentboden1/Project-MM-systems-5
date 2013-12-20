using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LobbyService.Web.DTO.MonopolyEngine
{
    [DataContract]
    public class GamePlayerToClient
    {


        [DataMember]
        public Player MyPlayer { get; set; }

        [DataMember]
        public string MyName { get; set; }

        [DataMember]
        public byte Location { get; set; }

        [DataMember]
        public byte PrisonTime { get; set; }

        [DataMember]
        public bool IsPrison { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsPlaying { get; set; }

        [DataMember]
        public bool HasEscapePrisonCh { get; set; }

        [DataMember]
        public bool HasEscapePrisonCo { get; set; }

        [DataMember]
        public int Cash { get; set; }
    }
}