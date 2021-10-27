using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SisProdutos
{
    public class CepValidator : AbstractValidator<string>
    {
        public CepValidator()
        {
            RuleFor(x => x).NotEmpty().WithMessage("O campo cep � obrigat�rio");
            RuleFor(x => x).Matches(@"^[0-9]{5}-?[0-9]{3}$").WithMessage("O campo Cep � inv�lido");
        }
    }
}
