using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SisClientes
{
    public class UpdateCidadeValidator : AbstractValidator<UpdateCidadeDTO>
    {
        public UpdateCidadeValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O campo nome é obrigatório");
            RuleFor(x => x.Estado).NotEmpty().WithMessage("O campo estado é obrigatório");
        }
    }
}
