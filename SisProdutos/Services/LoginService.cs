using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SisProdutos;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

public class LoginService 
{
    private SignInManager<IdentityUser<int>> _signInManager;
    private TokenService _tokenService;

    public LoginService(SignInManager<IdentityUser<int>> signInManager,
        TokenService tokenService)
    {
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public Result LogaUsuario(LoginRequest request)
    {
        var resultadoIdentity = _signInManager
            .PasswordSignInAsync(request.Username, request.Password, false, false);
        if (resultadoIdentity.Result.Succeeded)
        {
            var identityUser = _signInManager
                .UserManager
                .Users
                .FirstOrDefault(usuario => 
                usuario.NormalizedUserName == request.Username.ToUpper());
            Token token = _tokenService.CreateToken(identityUser);
            
            return Result.Ok().WithSuccess(token.Value);
        }
        return Result.Fail("Login falhou");
            
    }
    public string GenerateToken(LoginRequest request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, request.Username),
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}
