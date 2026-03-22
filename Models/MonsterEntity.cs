using System.ComponentModel.DataAnnotations;

namespace ASPServerAPI.Models
{
    public class MonsterEntity
    {
        public long Id { get; set; }
        public long PlayerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Defence { get; set; }
        public int Agility { get; set; }
        public int AttackDamage { get; set; }
        public int Level { get; set; }
        public int DebuffCount { get; set; }
    }
}