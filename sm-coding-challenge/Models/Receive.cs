using System.Runtime.Serialization;

namespace sm_coding_challenge.Models
{
    public class Receive : Skill
    {
        [DataMember(Name = "tds")]
        public int Tds { get; set; }

        [DataMember(Name = "rec")]
        public int Rec { get; set; }
    }
}