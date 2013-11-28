using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicktest.DTO
{
    public class PlayerLobby
    {

        public PlayerLobby(DTO.Player host)
        {
            HostPlayer = host;
            Player = new List<Player>();
            Player.Add(HostPlayer);
        }

        //[DataMember]
        public DTO.Lobby LobbyId { get; set; }

        //[DataMember]
        public List<DTO.Player> Player { get; set; }

        //[DataMember]
        public DTO.Player HostPlayer { get; set; }

        //[DataMember]
        public bool IsAwaitingForPlayers { get; set; }

        //[DataMember]
        public bool StartGame { get; set; }
    }
}
