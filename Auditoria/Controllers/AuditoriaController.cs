using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auditoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriaController : ControllerBase
    {
        MongoDB mongo = new MongoDB();

        [HttpPost]
        public void PostPedido([FromBody] ClienteProduto clienteProduto)
        {

            //conectando no mongo
            mongo.ConectandoBanco();
            //inserindo no mongo
            mongo.InserindoBanco(clienteProduto);

        }

        

        
    }
}
