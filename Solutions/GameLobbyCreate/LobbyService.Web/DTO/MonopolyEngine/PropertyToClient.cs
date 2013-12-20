using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LobbyService.Web.DTO.MonopolyEngine
{
    [DataContract]
    public class PropertyToClient
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public bool Morguage { get; set; }
        [DataMember]
        public byte SetLevel { get; set; }
        [DataMember]
        public byte HouseLevel { get; set; }
    }
}