using ASPServerAPI.DTOs.Monster;
using ASPServerAPI.DTOs.Player;

namespace ASPServerAPI.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string UserName { get; set; } = stirng.Empty;
        public long PlayerId { get; set; }
        public PlayerStateResponse Stats { get; set; } = new();
        public MonsterStateResponse MonsterStats { get; set; } = new();
    }
}
