using Pymex.Services.Contracts;
using Pymex.Services.Models;
using Pymex.Services.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;

namespace Pymex.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "InventarioService" in both code and config file together.
    public class InventarioService : IInventarioService
    {
        public ResponseDataContract RegistrarEntrada(EntradaDC entrada)
        {
            throw new NotImplementedException();
        }

        public ResponseDataContract RegistrarSalida(SalidaDC salida)
        {
            throw new NotImplementedException();
        }
    }
}
