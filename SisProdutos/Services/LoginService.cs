using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SisProdutos;
using System;
using System.Linq;

public class LoginService
    {
        private SignInManager<IdentityUser<int>> _signInManager;
        private TokenService _tokenService;
        private HttpContext _httpContext;



        public LoginService(SignInManager<IdentityUser<int>> signInManager,
            TokenService tokenService, HttpContext httpContext)
        {
            _signInManager = signInManager;
            _tokenService = tokenService;
            _httpContext = httpContext;
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
    }
