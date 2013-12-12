using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace LobbyService.Web.DTO.MonopolyEngine
{
    [DataContract]
    public class OwnedProperty
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public bool Morguage { get; set; }
        [DataMember]
        public byte SetLevel { get; set; }
        [DataMember]
        public byte HouseLevel { get; set; }

        public OwnedProperty(int cardID)
        {
            ID = cardID;
            Morguage = false;
            SetLevel = 0;
            HouseLevel = 0;
        }
    }
}