using sm_coding_challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sm_coding_challenge.Services.CacheProvider
{
    public interface ICacheProvider
    {
        /// <summary>
        /// Gets player from cache asynchronously
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlayerModel> GetPlayerAsync(string id);

        /// <summary>
        /// Saves player asynchronously to cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetPlayerAsync(PlayerModel player);

    }
}
