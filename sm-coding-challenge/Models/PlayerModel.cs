using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace sm_coding_challenge.Models
{
    [DataContract]
    public class PlayerModel
    {
        public PlayerModel()
        {
            Kicks = new HashSet<Kick>();
            Rushes = new HashSet<Rush>();
            Passes = new HashSet<Pass>();
            Receives = new HashSet<Receive>();
        }

        [DataMember(Name = "player_id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"PlayerId: {Id} PlayerName: {Name}";
        }

        [DataMember(Name="rushing")]
        public virtual ICollection<Rush> Rushes { get; set; }
        
        [DataMember(Name="kicking")]
        public virtual ICollection<Kick> Kicks { get; set; }
        
        [DataMember(Name="passing")]
        public virtual ICollection<Pass> Passes { get; set; }
        
        [DataMember(Name="receiving")]
        public virtual ICollection<Receive> Receives { get; set; }

        public bool Equals(PlayerModel other)
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
            int hashPlayerName = Name == null ? 0 : Name.GetHashCode();

            //Get hash code for the Id field.
            int hashPlayerId = Id.GetHashCode();

            //Calculate the hash code for the Player.
            return hashPlayerName ^ hashPlayerId;
        }
    }
}

