using System.Text.Json;
using ASPServerAPI.Services.Interface;
using ASPServerAPI.Repository.Interface;
using ASPServerAPI.Models;
using ASPServerAPI.DTOs.Player;
using ASPServerAPI.DTOs.Battle;
using ASPServerAPI.DTOs.Monster;

namespace ASPServerAPI.Services
{
    public class PlayerService
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly IMonsterService _monsterService;
        
        public PlayerService(IPlayerRepository playerRepo, IMonsterService monster)
        {
            _playerRepo = playerRepo;
            _monsterService = monster;
        }

        public async Task<PlayerEntity> GetById(long id)
        {
            return await _playerRepo.FindById(id)
                ?? throw new KeyNotFoundException($"Player {id} not found");
        }

        public async Task<Player> GetGamePlayer(long id)
        {
            var entity = await GetById(id);
            return ConvertToGame(entity);
        }

        public Player ConvertToGame(PlayerEntity e)
        {
            return new Player(
                e.UserName,
                e.Hp,
                e.MaxHp,
                e.AttackDamage,
                e.BasicAttack,
                e.Defence,
                e.Mana,
                e.MaxMana,
                e.Agility,
                LoadCooldown(e),
                e.Statpoint,
                e.Level,
                e.Barrier,
                LoadBuffCount(e)
            );
        }

        public async Task UpdateEntity(Player player, PlayerEntity e)
        {
            e.Hp = player.Hp;
            e.MaxHp = player.MaxHp;
            e.AttackDamage = player.AttackDamage;
            e.BasicAttack = player.BasicAttack;
            e.Level = player.Level;
            e.Defence = player.Defence;
            e.Mana = player.Mana;
            e.MaxMana = player.MaxMana;
            e.Agility = player.Agility;
            e.StatPoint = player.StatPoint;
            e.SkillCooldownJson = JsonSerializer.Serialize(player.SkillCooldown);
            e.BuffCountJson = JsonSerializer.Serialize(player.BuffCount);

            await _playerRepo.Save(e);
        }

        private int[] LoadCooldown(PlayerEntity e)
        {
            if (string.IsNullOrEmpty(e.SkillCooldownJson)) return new int[12];
            return JsonSerializer.Deserialize<int[]>(e.SkillCooldownJson) ?? new int[12];
        }

        private int[] LoadBuffCount(PlayerEntity e)
        {
            if (string.IsNullOrEmpty(e.BuffCountJson)) return new int[2];
            return JsonSerializer.Deserialize<int[]>(e.BuffCountJson) ?? new int[2];//JsonSerializer가 뭐야?
        }

        public async Task<PlayerStateResponse> UseSkill(long playerId, int skillId, long? monsterId)
        {
            var playerEntity = await GetById(playerId);
            var player = ConvertToGame(playerEntity);
            Monster? target = null;
            MonsterEntity? monsterEntity = null;

            if (monsterId.HasValue)//HasValue가 뭐야? = monsterId는 null값이 올수 있는 값이여서
                // 
            {
                monsterEntity = await _monsterService.GetEntityById(monsterId.Value)//GetEntityById 함수는 없는데?
                    ?? throw new KeyNotFoundException("Monster not found");
                target = _monsterService.ConvertToMonster(monsterEntity);//ConvertToMonster도 없는 함수인데?
            }

            int damageFromMonster = target?.GiveDamage(player) ?? 0;
            int barrierToPlayer = 0;
            int damageFromPlayer = 0;
            int healFromPlayer = 0;

            //스킬 실행
            ExecuteSkill(skillId, player, target, ref damageFromPlayer, ref healFromPlayer, ref barrierToPlayer, ref damageFromMonster);

            bool attackAvoidToPlayer = player.AttackEvasion(player);
            bool attackAvoidToMonster = target?.AttackEvasion(target) ?? false;

            //턴 종료 처리
            ProcessTurnEnd(player, target, damageFromPlayer, damageFromMonster,
                healFromPlayer, barrierToPlayer, attackAvoidToPlayer, attackAvoidToMonster, out bool monsterDead, out bool playerDead);

            //DB에 저장할 값들을 Entity에 일단 저장
            await UpdateEntity(player, playerEntity);
            if (target != null && monsterEntity != null)
                await _monsterService.UpdateMonsterFromGame(target, monsterEntity);//이 함수 또한 없는데?

            //응답반환? 
            return BuildAttackResponse(player, target, damageFromPlayer, damageFromMonster,
                healFromPlayer, barrierToPlayer, attackAvoidToPlayer, attackAvoidToMonster,
                monsterDead, playerDead);
        }

        public async Task<PlayerStateResponse> UseStatPoint(long playerId, int choiceStat)
        {
            var playerEntity = await GetById(playerId);
            var player = ConvertToGame(playerEntity);

            if (playerEntity.StatPoint > 0)
                player.StatsUp(choiceStat);

            await UpdateEntity(player, playerEntity);

            return new PlayerStatsResponse//우리가 쓸려고 하는 DTO인 StatUpResponse는 어쩌고? 안썼던 이유는? 그리고 안쓸거라면 그 DTO는 지워야돼?
            {
                Name = player.Name,
                Level = player.Level,
                StatPoint = player.StatPoint,
                Hp = player.Hp,
                MaxHp = player.MaxHp,
                AttackDamage = player.AttackDamage,
                BasicAttack = player.BasicAttack,
                Defence = player.Defence,
                Agility = player.Agility,
                Mana = player.Mana,
                Barrier = player.Barrier,
                SkillCooldown = player.SkillCooldown,
                BuffCount = player.BuffCount
            };
        }

        private void ExecuteSkill(int skillId, Player player, Monster? target,
            ref int damageFromPlayer, ref int healFromPlayer,
            ref int barrierToPlayer, ref int damageFromMonster)
        {
            switch (skillId)
            {
                case 0 : damageFromPlayer = player.DoubleDamage(); break;
                case 1 : damageFromPlayer = target != null ? player.IgnoreDefense(target) : 0; break;
                case 2 : damageFromPlayer = player.IncreaseDefense(damageFromMonster, player);  break;
                case 3 : if (target != null) player.DebuffCreate(target); break;
                case 4 : player.BuffCreate_HpRegen(player); break;
                case 5 : player.BuffCreate_ManaRegen(player); break;
                case 6 : barrierToPlayer = player.BarrierCreate(); break;
                case 7 : healFromPlayer = player.HpRecorveryEnhanced(player); break;
                case 8 : player.AttackPowerUp(player);
                    if (target != null) damageFromPlayer = player.GiveDamage(target, player); break;
                case 9 : damageFromPlayer = target != null ? player.TradeStrike(player, target) : 0; break;
                case 10 : damageFromPlayer = player.Overdrive(player, target); break;
                case 11 : barrierToPlayer = player.DefensiveReadiness(player); break;
                case 12 : damageFromPlayer = target != null ? player.GiveDamage(target, player) : 0; break;
                case 13 : healFromPlayer = player.HpRecovery(player); break;
                default: throw new ArgumentException($"Invalid skillId: {skillId}");
            }
        }
        private void ProcessTurnEnd(Player player, Monster? target,
            int damageFromPlayer, int damageFromMonster,
            int healFromPlayer, int barrierToPlayer,
            bool attackAvoidToPlayer, bool attackAvoidToMonster,
            out bool monsterDead, out bool playerDead)
        {
            monsterDead = false;
            playerDead = false;

            if (target != null && !attackAvoidToMonster)
            {
                target.TakeDamage(damageFromPlayer, target);
            }

            player.Barrier += barrierToPlayer;
            player.Hp = Math.Min(player.hp + healFromPlayer, player.MaxHp);

            if(target != null && target.Hp > 0)
            {
                if (!attackAvoidToPlayer)
                    player.TakeDamage(damageFromMonster, player);
                player.TurnEnd();
                target.TurnEnd();
            }

            monsterDead = target?.Hp <= 0;
            if (monsterDead)
            {
                if (target != null) target.Hp = 0;
                player.LevelUp();
                target?.LevelUp();//벨런스 조절
            }
            else
            {
                playerDead = player.Hp <= 0;
            }
        }

        private AttackResponse BuildAttackResponse(Player player, Monster? target,
            int damageFromPlayer, int damageFromMonster,
            int healFromPlayer, int barrierToPlayer,
            bool attackAvoidToPlayer, bool attackAvoidToMonster,
            bool monsterDead, bool playerDead)
        {
            return new AttackResponse
            {
                AttackResult = new AttackResultDto
                {
                    DamageToMonster = damageFromPlayer,
                    DamageToPlayer = damageFromMonster,
                    HealToPlayer = healFromPlayer,
                    BarrierToPlayer = barrierToPlayer,
                    AttackAvoidToPlayer = attackAvoidToPlayer,
                    AttackAvoidToMonster = attackAvoidToMonster
                },
                player = new PlayerStatsResponse//근데 PlayerStatsResponse의 필드 값이 이것보다 많은데 그것들은 어떡해?
                //값을 안넣은 이유가 있어? 그리고 안 넣는다면 값이 다 날라가는거 아니야?
                {
                    Name = player.Name,
                    Level = player.Level,
                    StatPoint = player.StatPoint,
                    Hp = player.Hp,
                    MaxHp = player.MaxHp,
                    AttackDamage = player.AttackDamage,
                    BasicAttack = player.BasicAttack,
                    Defence = player.Defence,
                    Agility = player.Agility,
                    Mana = player.Mana,
                    Barrier = player.Barrier,
                    SkillCooldown = player.SkillCooldown,
                    BuffCount = player.BuffCount
                },
                Monster = target != null ? new MonsterStateResponse//근데 MonsterStatsResponse의 필드 값이 이것보다 많은데 그것들은 어떡해?
                //값을 안넣은 이유가 있어? 그리고 안 넣는다면 값이 다 날라가는거 아니야?
                {
                    Hp = target.Hp,
                    MaxHp = target.MaxHp,
                    Level = target.Level
                } : new NonsterStatsResponse(),
                IsMonsterDead = monsterDead,
                IsPlayerDead = playerDead
            };
            //        public int CurrentTurn { get; set; }//현재턴?
           // public string BattleMessage { get; set; } = string.Empty; 이것들은 왜 안받아?
        }
    }
}