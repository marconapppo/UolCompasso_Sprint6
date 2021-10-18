using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SisProdutos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace UsuariosApi.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<CreateUsuarioDto, Usuario>();
            CreateMap<Usuario, IdentityUser<int>>();
        }
    }
}
