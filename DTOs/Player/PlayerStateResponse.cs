namespace ASPServerAPI.DTOs.Player
{
    public class PlayerStateResponse
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int StatPoint { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int AttackDamage { get; set; }
        public int BasicAttack { get; set; }
        public int Defence { get; set; }
        public int Agility { get; set; }
        public int Mana { get; set; }
        public int MaxMana { get; set; }
        public int Barrier { get; set; }
        public int[] SkillCooldown { get; set; } = Array.Empty<int>();
        public int[] BuffCount { get; set; } = Array.Empty<int>();
    }
}