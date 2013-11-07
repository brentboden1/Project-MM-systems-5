using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LobbyService.Web.DTO
{
    [DataContract]
    public class PlayerLobby
    {
        [DataMember]
        public DTO.Lobby LobbyId { get; set; }

        [DataMember]
        public List<DTO.Player> Player { get; set; }

        [DataMember]
        public DTO.Player HostPlayer { get; set; }

        [DataMember]
        public bool IsAwaitingForPlayers { get; set; }
    }
}