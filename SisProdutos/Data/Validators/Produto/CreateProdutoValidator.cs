using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SisProdutos
{
    public class CreateProdutoValidator : AbstractValidator<CreateProdutoDto>
    {
        public CreateProdutoValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O campo nome é obrgatório");
            RuleFor(x => x.Descricao).NotEmpty().WithMessage("O campo descrição é obrgatório");
            RuleFor(x => x.Preco).NotEmpty().WithMessage("O campo preço é obrgatório");
            RuleFor(x => x.PalavraChave).NotEmpty().WithMessage("O campo palavra chave é obrgatório");
            RuleFor(x => x.Categoria).NotEmpty().WithMessage("O campo categoria é obrgatório");
            RuleForEach(x => x.CepOpcionais).SetValidator(new CepValidator());
        }
    }
}
