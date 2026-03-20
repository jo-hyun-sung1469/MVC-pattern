using ASPServerAPI.Models;

namespace ASPServerAPI.Repository.Interface
{
    public interface IMonsterRepository
    {
        Task<MonsterEntity> FindById(long id);
        Task<MonsterEntity> FindByPlayerId(long PlayerId);
        Task<MonsterEntity> Save(MonsterEntity monster);
    }
}
