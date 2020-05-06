using sm_coding_challenge.Models;
using System.Threading.Tasks;

namespace sm_coding_challenge.Services.DataProvider
{
    public interface IDataProvider
    {
        /// <summary>
        /// I changed the return type of this model to return an object that reveals the status of the request along with the actual player data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlayerRequestResult> GetPlayerById(string id);
    }
}
