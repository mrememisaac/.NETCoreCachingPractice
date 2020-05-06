using System.Runtime.Serialization;

namespace sm_coding_challenge.Models
{
    public class Kick : Skill
    {
        [DataMember(Name ="field_goals_made")]
        public int FieldGoalsMade { get; set; }
        
        [DataMember(Name ="field_goals_att")]
        public int FieldGoalsAtt { get; set; }
        
        [DataMember(Name ="extra_pt_made")]
        public int ExtraPointsMade { get; set; }
        
        [DataMember(Name ="extra_pt_att")]
        public int ExtraPointsAtt { get; set; }
    }
}