using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pymex.Services.Contracts.Operations
{
    [ServiceContract]
    public interface IBuscarPorDocumentoOperation<T> where T : class
    {
        [OperationContract]
        ResponseWithDataDataContract<T> ObtenerPorNumeroDocumento(string numeroDocumento);
    }
}
