using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SisClientes
{
    public class CreateClienteValidator : AbstractValidator<CreateClienteDTO>
    {
        public CreateClienteValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O campo nome é obrigatório");
            RuleFor(x => x.DataNascimento).NotEmpty().WithMessage("O campo data de nascimento é obrigatório");
            RuleFor(x => x.Cep).NotEmpty().WithMessage("O campo cep é obrigatório");
            RuleFor(x => x.Cep).Matches(@"^[0-9]{5}-?[0-9]{3}$").WithMessage("O campo Cep é inválido");
            RuleFor(x => x.Logradouro).NotEmpty()
                .WithMessage("O campo logradouro é obrigatório");
            RuleFor(x => x.Bairro).NotEmpty()
                .WithMessage("O campo bairro é obrigatório");
            RuleFor(x => x.CepOpcionais).NotEmpty().WithMessage("O Campo CidadeDic é obrigatório");
        }
    }
}
