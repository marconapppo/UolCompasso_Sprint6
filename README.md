# UolCompasso_Sprint6

1) Conecção com o MongoDb:
  banco = Auditoria;
  coleção = UsuarioProduto;
2) Banco SisProdutos = Sprint06;
3) Banco SisClientes = SisClientes;

4) Algumas funções utilizam Authorize, para isso crie um usuario e se logue com ele, ao se logar ela retornará um token que poderá usar para adentrar a estas
 funções (duração de 2 horas).
 
5) Para criar cliente, utilize um usuario que não tenha criado um cliente, caso exista, ele retornará um http conflict.

Lembrar de iniciar as Tres aplicações ao msm tempo, o swagger está ativado mas o postman está com todos os comandos.

PS: Caso for fazer Add-Migration, lembrar de remover .Annotation("SqlServer:Identity", "1, 1"), na Id da tabela Cliente do banco SisClientes.
