
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;

namespace SisProdutos
{
    public class LogoutService
    {
        private SignInManager<IdentityUser<int>> _signInManager;

        public LogoutService(SignInManager<IdentityUser<int>> signInManager)
        {
            _signInManager = signInManager;
        }

        public Result DeslogaUsuario()
        {
            Task resultadoIdentity = _signInManager.SignOutAsync();

            if (resultadoIdentity.IsCompletedSuccessfully)
            {
                return Result.Ok();
            }            
            return Result.Fail("Logout falhou");

        }
    }
}
