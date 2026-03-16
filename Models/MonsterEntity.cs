namespace ASPServerAPI.Models
{
    public class MonsterEntity
    {
        private long id { get; set; }
        private long playerid { get; set; }
        private string name { get; set; }
        private int hp { get; set; }
        private int maxHp { get; set; }
        protected int Defence { get; set; }
        protected int agility { get; set; }
        protected int attackDamage { get; set; }
        protected int level { get; set; }
        private int debuffCount { get; set; }
    }
}
