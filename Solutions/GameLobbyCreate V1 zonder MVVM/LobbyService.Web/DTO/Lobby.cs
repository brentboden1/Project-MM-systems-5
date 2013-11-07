using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LobbyService.Web.DTO
{
    [DataContract]
    public class Lobby
    {
        [DataMember]
        public string LobbyName { get; set; }

        [DataMember]
        public int LobbyId { get; set; }
    }
}