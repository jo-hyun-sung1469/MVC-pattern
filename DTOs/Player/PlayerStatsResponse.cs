namespace ASPServerAPI.DTOs
{
    public class PlayerStatsResponse
    {
        public int Level { get; set; }
        public int StatPoint { get; set; }
        public int Hp { get; set; }
        public int MaxHP { get; set; }
        public int AttackDamage { get; set; }
        public int BasicAttack { get; set; }
        public int Defense { get; set; }
        public int Agility { get; set; }
        public int Mana { get; set; }
        public int MaxMana { get; set; }
        public int Barrier { get; set; }
        public int[] SkillCooldown { get; set; } = Array.Empty<int>();
        public int[] BuffCount { get; set; } = Array.Empty<int>();
    }
}