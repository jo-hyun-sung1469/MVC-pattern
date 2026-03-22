using ASPServerAPI.Data;
using ASPServerAPI.Models;
using ASPServerAPI.Repository.Interface;

namespace ASPServerAPI.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly AppDbContext _context;

        public PlayerRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PlayerEntity?> FindById(long id)
        {
            return await _context.Players.FindAsync(id);
        }
        public async Task<PlayerEntity?> FindByUsername(string username)
        {
            return await _context.Players
                .FirstOrDefaultAsync(p => p.Username == username);
        }

        public async Task<PlayerEntity?> FindByEEmail(string email)
        {
            return await _context.Players
                .FirstOrDDefaultAsync(p => p.Email = email);
        }

        public async Task<PlayerEntity> Save(PlayerEntity player)
        {
            if (player.Id == 0)
                _context.Players.Add(player);
            else
                _context.Players.Update(player);

            await _context.SaveChangeAsync();
            return player;
        }
    }
}
