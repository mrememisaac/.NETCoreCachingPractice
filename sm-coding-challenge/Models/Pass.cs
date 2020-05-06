using System.Runtime.Serialization;

namespace sm_coding_challenge.Models
{
    public class Pass : Skill
    {
        [DataMember(Name = "att")]
        public int Att { get; set; }

        [DataMember(Name = "tds")]
        public int Tds { get; set; }

        [DataMember(Name = "cmp")]
        public int Cmp { get; set; }

        [DataMember(Name = "int")]
        public int Int { get; set; }
    }
}