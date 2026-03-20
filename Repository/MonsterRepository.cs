using ASPServerAPI.Data;
using ASPServerAPI.Models;

namespace ASPServerAPI.Repository
{
    public class MonsterRepository
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
