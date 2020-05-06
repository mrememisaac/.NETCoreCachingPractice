using sm_coding_challenge.Models;
using sm_coding_challenge.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sm_coding_challenge.Services.DataProvider
{
    public abstract class SkillLoader 
    {
        public PlayerModel CreatePlayer(Skill skill, PlayerModel player, string id)
        {
            if (player != null) return player;
            if (skill != null && skill.PlayerId == id)
            {
                player = new PlayerModel() { Id = id, Name = skill.Name };
            }
            return player;
        }
        public abstract PlayerModel Load(DataResponseModel dataResponse, PlayerModel playerModel, string id);

    }
}
