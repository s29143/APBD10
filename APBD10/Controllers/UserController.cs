using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APBD10.Context;
using APBD10.DTOs;
using APBD10.Helpers;
using APBD10.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace APBD10.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ApbdContext _context;
    private readonly IConfiguration _configuration;

    public UserController(ApbdContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("/api/[controller]/register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var passAndSalt = SecurityHelpers.GetHashAndSalt(dto.Password);
        var user = new User()
        {
            Login = dto.Login,
            Password = passAndSalt.Item1,
            Salt = passAndSalt.Item2,
            IsActive = true,
            CreatedAt = DateTime.Now,
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return Created();
    }

    [AllowAnonymous]
    [HttpPost("/api/[controller]/login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Login == dto.Login);
        if (user is null)
        {
            return Unauthorized();
        }
        string pass = SecurityHelpers.GetHashWithSalt(user.Password, user.Salt);
        if (user.Password != pass)
        {
            return Unauthorized();
        }

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "http://localhost",
            audience: "http://localhost",
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
            claims: new []
            {
                new Claim(ClaimTypes.Role, "user")
            },
            expires: DateTime.Now.AddMinutes(10)
        );

        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExpr = DateTime.Now.AddDays(1);
        await _context.SaveChangesAsync();  

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = user.RefreshToken
        });
    }
    
    [HttpPost("/api/[controller]/refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshTokenDTO refreshTokenDto, [FromHeader(Name = "Authorization")] string token)
    {
        var user = _context.Users.FirstOrDefault(u => u.RefreshToken == refreshTokenDto.RefreshToken);
        if (user is null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        if (user.RefreshTokenExpr < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }
        
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

        JwtSecurityToken jwt = new JwtSecurityToken(
            issuer: "http://localhost",
            audience: "http://localhost",
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
            claims: new []
            {
                new Claim(ClaimTypes.Role, "user")
            },
            expires: DateTime.Now.AddMinutes(10)
        );
        
        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExpr = DateTime.Now.AddDays(1);
        await _context.SaveChangesAsync();  

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
            refreshToken = user.RefreshToken
        });
        
    }
}