using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace sm_coding_challenge.Models
{
    [DataContract]
    public class LatestPlayers
    {
        [DataMember(Name ="receiving")]
        public Receive Receiving { get; set; }

        [DataMember(Name = "rushing")]
        public Rush Rushing { get; set; }
    }
}
