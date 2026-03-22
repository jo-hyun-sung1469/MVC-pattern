using ASPServerAPI.Data;
using ASPServerAPI.Models;
using Microsoft.EntityFrameworkCore;
using ASPServerAPI.Repository.Interface;

namespace ASPServerAPI.Repository
{
    public class MonsterRepository : IMonsterRepository
    {
        private readonly AppDbContext _context;
        public MonsterRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<MonsterEntity?> FindById(long id)
        {
            return await _context.Monsters.FindAsync(id);
        }
        public async Task<MonsterEntity?> FindByPlayerId(long playerId)
        {
            return await _context.Monsters.FirstOrDefaultAsync(m => m.PlayerId == platerId);
        }
        public async Task<MonsterEntity>Save(MonsterEntity monster)
        {
            if (monster.Id == 0)
                _context.Monsters.Add(monster);
            else
                _context.Monsters.Update(monster);
            await _context.SaveChangeAsync();
            return monster;
        }
    }
}
