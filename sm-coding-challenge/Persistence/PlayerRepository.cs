using Microsoft.EntityFrameworkCore;
using sm_coding_challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sm_coding_challenge.Persistence
{
    /// <summary>
    /// Save/Retrieve to an alternate data store for longer storage
    /// </summary>
    public class PlayerRepository : IPlayerRepository
    {
        SMContext db;
        public PlayerRepository(SMContext _db)
        {
            db = _db;
        }
        public async Task<PlayerModel> GetPlayer(string id)
        {
            var player = await db.Players.FirstOrDefaultAsync(x=> x.Id == id);
            if(player != null)
            {
                if(player.Passes.Count() == 0 && db.Passes.Any(x=>x.PlayerId == id))
                {
                    player.Passes = db.Passes.Where(x => x.PlayerId == id).ToHashSet();
                }

                if (player.Rushes.Count() == 0 && db.Rushes.Any(x => x.PlayerId == id))
                {
                    player.Rushes = db.Rushes.Where(x => x.PlayerId == id).ToHashSet();
                }

                if (player.Receives.Count() == 0 && db.Receives.Any(x => x.PlayerId == id))
                {
                    player.Receives = db.Receives.Where(x => x.PlayerId == id).ToHashSet();
                }

                if (player.Kicks.Count() == 0 && db.Kicks.Any(x => x.PlayerId == id))
                {
                    player.Kicks = db.Kicks.Where(x => x.PlayerId == id).ToHashSet();
                }
            }
            return player;
        }

        public async Task<bool> AddPlayer(PlayerModel player)
        {
            if (db != null)
            {
                await db.Players.AddAsync(player);
                await db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
