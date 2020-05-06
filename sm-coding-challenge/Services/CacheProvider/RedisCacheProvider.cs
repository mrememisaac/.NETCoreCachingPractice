using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using sm_coding_challenge.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sm_coding_challenge.Services.CacheProvider
{
    public class RedisCacheProvider : ICacheProvider
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ILogger<RedisCacheProvider> _logger;

        public RedisCacheProvider(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisCacheProvider> logger)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _logger = logger;
        }

        public async Task<PlayerModel> GetPlayerAsync(string id)
        {
            PlayerModel player = null;
            if(string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                return player;
            }
            try
            {
                var db = _connectionMultiplexer.GetDatabase();
                var playerString = await db.StringGetAsync(id);
                if (!string.IsNullOrEmpty(playerString) && !string.IsNullOrWhiteSpace(playerString))
                {
                    player = JsonConvert.DeserializeObject<PlayerModel>(playerString, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ex)
            {
                //Log error
                _logger.LogError(ex.HResult, ex.Message, id);
            }
            return player;
        }

        public async Task SetPlayerAsync(PlayerModel player)
        {
            try
            {
                var db = _connectionMultiplexer.GetDatabase();
                //turn into string
                var playerString = JsonConvert.SerializeObject(player);
                //prevent duplicate child fields
                await db.StringSetAsync(player.Id, playerString, TimeSpan.FromDays(7));
            }
            catch (Exception ex)
            {
                //Log error
                _logger.LogError(ex.HResult, ex.Message, player.Id);
            }
        }
    }
}
