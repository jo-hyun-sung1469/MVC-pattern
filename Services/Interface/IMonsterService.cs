using ASPServerAPI.DTOs.Monster;
using ASPServerAPI.DTOs.Battle;

namespace ASPServerAPI.Services.Interface
{
    public interface IMonsterService
    {
        Task<MonsterStateResponse?> GetById(long id);
        Task<MonsterStateResponse> GetOrCreateMonster(long playerId);
        Task UpdateMonster(long id, MonsterStateResponse req);
        Task<MonsterStateResponse?> GetMonsterState(long id);
    }
}