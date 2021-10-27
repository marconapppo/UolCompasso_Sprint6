using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SisClientes
{
    public class CidadeCepValidator : AbstractValidator<CidadeCep>
    {
        public CidadeCepValidator()
        {
            RuleFor(x => x.Cep).NotEmpty().WithMessage("O campo cep � obrigat�rio");
            RuleFor(x => x.Cep).Matches(@"^[0-9]{5}-?[0-9]{3}$").WithMessage("O campo Cep � inv�lido");
            RuleFor(x => x.Logradouro).NotEmpty()
                .WithMessage("O campo logradouro � obrigat�rio");
            RuleFor(x => x.Bairro).NotEmpty()
                .WithMessage("O campo bairro � obrigat�rio");
        }
    }
}
