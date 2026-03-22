using Microsoft.AspNetCore.Mvc;
using ASPServerAPI.Services;
using ASPServerAPI.DTOs.Auth;

namespace ASPServerAPI.Controller
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : ControllerBase    
    {
        private readonly AuthService _authService;
        public AuthController(AuthController authController)
        {
            _authService = authController;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            var result = await _authService.Register(register);
            if (result == null)
            {
                return BadRequest("이미 존재하는 이메일 입니다");
            }
            return OK(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var result = await _authService.Login(login);
            if (result == null)
                return Unauthorized("Email이나 Password가 올바르지 않습니다");
            return OK(result);
        }
    }
}