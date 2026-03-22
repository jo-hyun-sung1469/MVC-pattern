using ASPServerAPI.DTOs.Monster;
using ASPServerAPI.DTOs.Player;

namespace ASPServerAPI.DTOs.Battle
{
    public class AttackResponse
    {
        public AttackResultDto AttackResult { get; set; } = new();
        public PlayerStatsResponse player { get; set; } = new();
        public MonsterStateResponse Monster { get; set; } = new();
        public bool IsMonsterDead { get; set; } 
        public bool IsPlayerDead { get; set; }
        public int CurrentTurn { get; set; }//현재턴?
        public string BattleMessage { get; set; } = string.Empty;
        //이 메시지를 받아서 화면에 띄우기
    }
}