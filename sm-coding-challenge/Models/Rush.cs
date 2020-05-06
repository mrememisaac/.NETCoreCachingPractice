using System.Runtime.Serialization;

namespace sm_coding_challenge.Models
{
    public class Rush : Skill
    {
        [DataMember(Name = "att")]
        public int Att { get; set; }

        [DataMember(Name = "tds")]
        public int Tds { get; set; }
        
        [DataMember(Name = "fum")]
        public int Fum { get; set; }

    }
}