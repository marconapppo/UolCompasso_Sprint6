using AutoMapper;
using System;
using System.Collections.Generic;

namespace SisClientes
{
    public class CidadeProfile : Profile
    {
        public CidadeProfile(){
            CreateMap<CreateCidadeDTO,Cidade>();
            CreateMap<Cidade, ReadCidadeDTO>();
            CreateMap<UpdateCidadeDTO, Cidade>();
        }
        
    }
}
