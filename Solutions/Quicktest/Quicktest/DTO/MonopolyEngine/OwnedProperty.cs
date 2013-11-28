using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicktest.DTO.MonopolyEngine
{
    public class OwnedProperty
    {
        public int ID { get; set; }
        public bool Morguage { get; set; }
        public byte SetLevel { get; set; }
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
