using sm_coding_challenge.Models;
using System.Threading.Tasks;

namespace sm_coding_challenge.Persistence
{
    public interface IPlayerRepository
    {
        Task<PlayerModel> GetPlayer(string id);
    }
}