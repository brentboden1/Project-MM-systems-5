using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicktest.DTO
{
    public class Player
    {
        //[Required(ErrorMessage = "Playername is required")]
        //[DataMember]
        public string PlayerName { get; set; }

        //[DataMember]
        public int PlayerId { get; set; }

        //[DataMember]
        public bool AlreadExist { get; set; }
    }
}
