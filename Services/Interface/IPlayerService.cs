using ASPServerAPI.DTOs.Player;
using ASPServerAPI.DTOs.Battle;
using ASPServerAPI.Models;

namespace ASPServerAPI.Services.Interface
{
    public interface IPlayerService
    {
        Task<PlayerEntity> GetById(long id);
        Task<PlayerStateResponse> UseSkill(long playerId, int skillId, long? monsterId);
        Task<PlayerStateResponse> UseStatPoint(long playerId, int choiceStat);
        Task UpdateEntity(PlayerStateResponse gamePlayer, PlayerEntity entity);
        Task<Player> GetGamePlayer(long id);
    }
}
