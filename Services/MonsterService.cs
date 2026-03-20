using ASPServerAPI.DTOs.Monster;
using ASPServerAPI.Models;
using ASPServerAPI.Services.Interface;
using ASPServerAPI.Repository.Interface;

namespace ASPServerAPI.Services
{
    public class MonsterService : IMonsterService
    {
        private readonly IMonsterRepository _monsterRepository;
        public MonsterService(IMonsterRepository monsterRepository)
        {
            _monsterRepository = monsterRepository;
        }

        public async Task<MonsterStateResponse?> GetById(long id)
        {
            var entity = await _monsterRepository.FindById(id)
                ?? throw new KeyNotFoundException($"Monster{id} not found");
            return ConvertToDto(entity);
        }

        public async Task<MonsterStateResponse> GetOrCreateMonster(long playerId)
        {
            var existing = await _monsterRepository.FindByPlayerId(playerId);
            if(existing != null)
            {
                return ConvertToDto(existing);
            }
            var monster = new MonsterEntity
            {
                PlayerId = playerId,
                Name = "Monster",
                Hp = 500,
                MaxHp = 500,
                Defence = 45,
                Agility = 90,
                AttackDamage = 50,
                Level = 1,
                DebuffCount = 0
            };
        }

        public async Task UpdateMonster(long id, MonsterStateResponse req)
        {
            var entity = await _monsterRepository.FindById(id)
                ?? throw new KeyNotFoundException($"Monster {id} not found");
            entity.Hp = req.Hp;
            entity.MaxHp = req.MaxHp;
            entity.AttackDamage = req.AttackDamage;
            entity.Defence = req.Defence;
            entity.Agility = req.Agility;
            entity.DebuffCount = req.DebuffCount;

            await _monsterRepository.Save(entity);
        }

        public async Task<MonsterStateResponse?> GetMonsterState(long id)
        {
            var entity = await _monsterRepository.FindById(id);
            return entity == null ? null : ConvertToDto(entity);
        }

        private MonsterStateResponse ConvertToDto(MonsterEntity entity)
        {
            return new MonsterStateResponse
            {
                Id = entity.Id,
                PlayerId = entity.PlayerId,
                Name = entity.Name,
                Hp = entity.Hp,
                MaxHp = entity.MaxHp,
                AttackDamage = entity.AttackDamage,
                Agility = entity.Agility,
                Defence = entity.Defence,
                Level = entity.Level,
                DebuffCount = entity.DebuffCount,
            };
        }
    }
}