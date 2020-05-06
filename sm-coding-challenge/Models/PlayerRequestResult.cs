using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace sm_coding_challenge.Models
{

    /// <summary>
    /// Provides a mechanism for notifying the client of the status of the request allowing the frontend to respond 
    /// gracefully to a failed request scenario
    /// </summary>
    [DataContract]
    public class PlayerRequestResult
    {
        /// <summary>
        /// The player object, is null for failed requests
        /// </summary>
        [DataMember(Name = "player")]
        public PlayerModel Player { get; set; }
        /// <summary>
        /// True if request succeeded else false
        /// </summary>
        [DataMember(Name = "successful")]
        public bool Successful { get; set; }
        /// <summary>
        /// Any helpful error message for the consumer
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
