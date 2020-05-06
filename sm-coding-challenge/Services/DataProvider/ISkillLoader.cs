using sm_coding_challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sm_coding_challenge.Services.DataProvider
{
    public interface ISkillLoader<T>
    {
        PlayerModel CreatePlayer(Skill skill, PlayerModel player, string id);
        abstract PlayerModel Load(DataResponseModel dataResponse, PlayerModel playerModel, string id);
    }
}
