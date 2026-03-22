using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Micorsoft.IdentityModel.Tokens;
using ASPServerAPI.Data;
using ASPServerAPI.Services;
using ASPServerAPI.Repository;
using ASPServerAPI.Repository.Interface;
using ASPServerAPI.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AddDbContext>(options => options.UseMySql(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnetion"))));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        VaildAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(IMonsterRepository, MonsterRepository);//의존성 주입
builder.Services.AddScoped(IMonsterService, MonsterService);//의존성 주입
builder.Services.AddScoped(IPlayerService, PlayerService);
builder.Services.AddScoped(IPlayerRepository, PlayerRepository)
//요청 생성시
var app = builder.Build();

if(app.Enviroment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();