namespace ASPServerAPI.Models
{
    public class PlayerEntity
    {
        private Long id { get; set; }
        private String password { get; set; }
        private String username { get; set; }
        private int level { get; set; }
        private int hp { get; set; }
        protected int maxHp { get; set; }
        protected int Defence { get; set; }
        protected int agility { get; set; }
        protected int attackDamage { get; set; }
        private int statistics { get; set; }
        private int Intelligence { get; set; }
        private int maxIntelligence { get; set; }
        private int basicattack { get; set; }
        private String skillCooldownJson { get; set; } //배열을 JSON 문자열로 저장
        private String buffCountJson { get; set; }
        private int barrier { get; set; }


    }
}
