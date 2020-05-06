using sm_coding_challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sm_coding_challenge.Services.DataProvider
{
    public class KicksDataLoader<Kick> : SkillLoader, ISkillLoader<Kick>
    {
        public override PlayerModel Load(DataResponseModel dataResponse, PlayerModel player, string id)
        {
            //create a player object if a kick skill exists with the expected player id
            //and if player is null, this approach ensures that even if there is no matching player id 
            // in the any of the collections above for eg they could be empty, our code wil still run
            player = CreatePlayer(dataResponse.Kicking.FirstOrDefault(x => x.PlayerId == id), player, id);

            //if player object exists, add the receive skill entries
            if (player != null)
            {
                /*
                 * The data returned for a player id contains data details of other players, this seems unusual 
                 * I assume that the request is asking for 
                 * actions(rushes, kickings, passings, receivings) of the player with the supplied id.
                 * For this reason, before persisting this information in the redis cache, i clean out entries that do not have the player id of interest
                 * thats the reason for the where clause
                 */
                dataResponse.Kicking.Where(x => x.PlayerId == player.Id).Distinct().ToList().ForEach(kick => {
                    player.Kicks.Add(kick);
                });
            }
            return player;
        }
    }
}
