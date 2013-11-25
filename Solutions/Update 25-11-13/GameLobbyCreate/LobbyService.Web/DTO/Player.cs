using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LobbyService.Web.DTO
{
    [DataContract]
    public class Player
    {
        [Required(ErrorMessage="Playername is required")]
        [DataMember]
        public string PlayerName { get; set; }

        [DataMember]
        public int PlayerId { get; set; }

        [DataMember]
        public bool AlreadExist { get; set; }
    }
}