using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SisClientes
{
    public class CreateCidadeValidator : AbstractValidator<CreateCidadeDTO>
    {
        public CreateCidadeValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O campo nome � obrigat�rio");
            RuleFor(x => x.Estado).NotEmpty().WithMessage("O campo estado � obrigat�rio");
        }
    }
}
