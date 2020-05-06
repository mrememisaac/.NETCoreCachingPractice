using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace sm_coding_challenge.Models
{
    [DataContract]
    public class Skill
    {
        [DataMember(Name ="entry_id")]
        public string Id { get; set; }

        [DataMember(Name = "player_id")]
        public string PlayerId { get; set; }

        [DataMember(Name ="name")]
        public string Name { get; set; }

        [DataMember(Name ="week")]
        public int Week { get; set; }

        [DataMember(Name ="position")]
        public string Position { get; set; }

        [DataMember(Name ="yds")]
        public int Yds { get; set; }

        public bool Equals(Skill other)
        {

            //Check whether the compared object is null.
            if (other is null) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the Players' properties are equal.
            return Id.Equals(other.Id) && Name.Equals(other.Name);
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public override int GetHashCode()
        {

            //Get hash code for the Name field if it is not null.
            int hashSkillName = Name == null ? 0 : Name.GetHashCode();

            //Get hash code for the Id field.
            int hashSkillId = Id.GetHashCode();

            //Calculate the hash code for the Player.
            return hashSkillName ^ hashSkillId;
        }

    }
}
