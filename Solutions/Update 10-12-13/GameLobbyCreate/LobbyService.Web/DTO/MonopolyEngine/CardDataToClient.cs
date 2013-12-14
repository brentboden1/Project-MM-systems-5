using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LobbyService.Web.DTO.MonopolyEngine
{
    [DataContract]
    public class CardDataToClient
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int BuyCost { get; set; }

        [DataMember]
        public int Rent0 { get; set; }

        [DataMember]
        public int Rent1 { get; set; }

        [DataMember]
        public int Rent2 { get; set; }

        [DataMember]
        public int Rent3 { get; set; }

        [DataMember]
        public int Rent4 { get; set; }

        [DataMember]
        public int Rent5 { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int Group { get; set; }

        [DataMember]
        public int Position { get; set; }

        [DataMember]
        public int HouseCost { get; set; }
    }
}