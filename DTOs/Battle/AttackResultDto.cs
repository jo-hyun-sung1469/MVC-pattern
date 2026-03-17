namespace ASPServerAPI.DTOs.Battle
{
    public class AttackResultDto
    {
        public int DamageToMonster { get; set; }
        public int DamageToPlayer { get; set; }
        public int HealToPlayer { get; set; }
        public int BarrierToPlayer { get; set; }
        public bool AttackAvoidToPlayer { get; set; }
        public bool AttackAvoidToMonster { get; set; }
    }
}