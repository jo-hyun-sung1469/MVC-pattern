using System.ComponentModel.DataAnnotations;

namespace ASPServerAPI.Models
{
    public class PlayerEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }//User.cs파일의 FK이다
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Defence { get; set; }
        public int Agility { get; set; }
        public int AttackDamage { get; set; }
        public int Statpoint { get; set; }
        public int Mana { get; set; }   
        public int MaxMana { get; set; }
        public int BasicAttack { get; set; }
        public string SkillCooldownJson { get; set; } = string.Empty;//배열을 JSON 문자열로 저장
        public string BuffCountJson { get; set; } = string.Empty;
        public int Barrier { get; set; }
    }
}