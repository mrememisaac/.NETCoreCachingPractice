using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using sm_coding_challenge.Models;
using sm_coding_challenge.Services.CacheProvider;

namespace sm_coding_challenge.Services.DataProvider
{
    public class DataProviderImpl : IDataProvider
    {
        public static TimeSpan Timeout = TimeSpan.FromSeconds(30);

        private ICacheProvider _cache;
        private ILogger<DataProviderImpl> _logger;

        public DataProviderImpl(ICacheProvider cache, ILogger<DataProviderImpl> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Creates a Player Request Result object, using supplied parameters, defaults to a positive message and success status
        /// </summary>
        /// <param name="player"></param>
        /// <param name="message"></param>
        /// <param name="successful"></param>
        /// <returns>
        /// PlayerRequestResult object populated with supplied parameters
        /// </returns>
        private PlayerRequestResult CreatePlayerRequestResult(PlayerModel player)
        {
            return new PlayerRequestResult() { 
                Player = player, 
                Message = player == null ? "Player not found" : "Request successful", 
                Successful = player == null ? false : true 
            };
        }

        public async Task<PlayerRequestResult> GetPlayerById(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                return CreatePlayerRequestResult(null);
            }
            PlayerModel player = await FetchPlayerFromCache(id);
            if(player == null)
            {
                player = await FetchPlayerFromSource(id);
            }
            return CreatePlayerRequestResult(player);
        }

        private async Task<PlayerModel> FetchPlayerFromCache(string id)
        {
            PlayerModel playerModel = null;
            playerModel = await _cache.GetPlayerAsync(id);
            return playerModel;
        }

        private async Task<PlayerModel> FetchPlayerFromSource(string id)
        {
            
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            PlayerModel player = null;

            PlayerModel CreatePlayer(Skill skill)
            {
                if(player != null) return player;
                if(skill != null && skill.PlayerId == id){
                    player = new PlayerModel() { Id = id, Name = skill.Name };
                }
                return player;
            }

            //Local function that fills out the kicks list on the PlayerModel
            PlayerModel LoadKicks(DataResponseModel dataResponse)
            {
                //create a player object if a kick skill exists with the expected player id
                //and if player is null, this approach ensures that even if there is no matching player id 
                // in the any of the collections above for eg they could be empty, our code wil still run
                player = CreatePlayer(dataResponse.Kicking.FirstOrDefault(x => x.PlayerId == id));

                //if player object exists, add the receive skill entries
                if (player != null)
                {
                    /*
                     * The data returned for a player id contains data details of other players, this seems unusual as I assume that the request is asking for 
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

            //Local function that fills out the Receives list on the PlayerModel
            PlayerModel LoadReceives(DataResponseModel dataResponse)
            {
                //create a player object if a pass skill exists with the expected player id
                //and if player is null, this approach ensures that even if there is no matching player id 
                // in the any of the collections above for eg they could be empty, our code wil still run
                player = CreatePlayer(dataResponse.Receiving.FirstOrDefault(x => x.PlayerId == id));

                //if player object exists, add the receive skill entries
                if (player != null)
                {
                    /*
                     * The data returned for a player id contains data details of other players, this seems unusual as I assume that the request is asking for 
                     * actions(rushes, kickings, passings, receivings) of the player with the supplied id.
                     * For this reason, before persisting this information in the redis cache, i clean out entries that do not have the player id of interest
                     * thats the reason for the where clause
                     */
                    dataResponse.Receiving.Where(x => x.PlayerId == player.Id).Distinct().ToList().ForEach(receive =>
                    {
                        player.Receives.Add(receive);
                    });
                }
                return player;
            }

            //Local function that fills out the Rueshes collection on the PlayerModel
            PlayerModel LoadRushes(DataResponseModel dataResponse)
            {
                //create a player object if a rush skill exists with the expected player id 
                //and if player is null
                player = CreatePlayer(dataResponse.Rushing.FirstOrDefault(x => x.PlayerId == id));

                //if player object exists, add the rush skill entries
                //and if player is null
                if (player != null)
                {
                    /*
                     * The data returned for a player id contains data details of other players, this seems unusual as I assume that the request is asking for 
                     * actions(rushes, kickings, passings, receivings) of the player with the supplied id.
                     * For this reason, before persisting this information in the redis cache, i clean out entries that do not have the player id of interest
                     * thats the reason for the where clause
                     */
                    dataResponse.Rushing.Where(x=>x.PlayerId == player.Id).Distinct().ToList()
                    .ForEach(rush =>
                    {
                        player.Rushes.Add(rush);
                    });
                }
                return player;
            }

            //Local function that fills out the Passes collection on the PlayerModel
            PlayerModel LoadPasses(DataResponseModel dataResponse)
            {
                //create a player object if a pass skill exists with the expected player id
                //and if player is null, this approach ensures that even if there is no matching player id 
                // in the any of the collections above for eg they could be empty, our code wil still run
                player = CreatePlayer(dataResponse.Passing.FirstOrDefault(x => x.PlayerId == id));

                //if player object exists, add the pass skill entries
                //and if player is null, this approach ensures that even if there is no matching player id 
                // in the any of the collections above for eg they could be empty, our code wil still run
                if (player != null)
                {
                    /*
                     * The data returned for a player id contains data details of other players, this seems unusual as I assume that the request is asking for 
                     * actions(rushes, kickings, passings, receivings) of the player with the supplied id.
                     * For this reason, before persisting this information in the redis cache, i clean out entries that do not have the player id of interest
                     * thats the reason for the where clause
                     */
                    dataResponse.Passing.Where(x => x.PlayerId == player.Id).Distinct().ToList().ForEach(pass =>
                    {
                        player.Passes.Add(pass);
                    });
                }
                return player;
            }

            //start of call to third party api
            using (var client = new HttpClient(handler))
            {
                try
                {
                    //optimize the code below
                    client.Timeout = Timeout;
                    var response = client.GetAsync("https://gist.githubusercontent.com/RichardD012/a81e0d1730555bc0d8856d1be980c803/raw/3fe73fafadf7e5b699f056e55396282ff45a124b/basic.json").Result;
                    var stringData = response.Content.ReadAsStringAsync().Result;
                    
                    var dataResponse = JsonConvert.DeserializeObject<DataResponseModel>(stringData, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                    //Use of local functions to tidy up this code section
                    LoadRushes(dataResponse);
                    LoadPasses(dataResponse);
                    LoadReceives(dataResponse);
                    LoadKicks(dataResponse);

                    //cache response for future
                    if (player != null)
                    {
                        await _cache.SetPlayerAsync(player);
                    }
                }
                catch (Exception e)
                {
                    //Log error
                    _logger.LogError(e.HResult, e.Message, id);
                    //Return useful feedback to the caller
                }
            }
            
            return player;
        }

    }
}
