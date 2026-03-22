using ASPServerAPI.Models;
namespace ASPServerAPI.Repository.Interface
{
    public interface IPlayerRepository
    {
        Task<PlayerEntity?> FindById(long id);
        Task<PlayerEntity?> FindByUsername(string username);
        Task<PlayerEntity?> FindByEmail(string email);
        Task<PlayerEntity> Save(PlayerEntity player);
    }
}
