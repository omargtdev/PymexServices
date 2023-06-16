using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Pymex.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReportesService" in both code and config file together.
    [ServiceContract]
    public interface IReportesService
    {
        [OperationContract]
        ResponseWithDataDataContract<ProductoDC> ObtenerProductoMasVendido();

        [OperationContract]
        ResponseWithDataDataContract<IEnumerable<EntradaDC>> ObtenerEntradasPorProveedor(string numeroDocumento, DateTime fechaInicio, DateTime fechaFin);

        [OperationContract]
        ResponseWithDataDataContract<IEnumerable<SalidaDC>> ObtenerSalidasPorCliente(string numeroDocumento, DateTime fechaInicio, DateTime fechaFin);

        [OperationContract]
        ResponseWithDataDataContract<IEnumerable<ClienteDC>> ObtenerClientesSinNingunaCompra();
    }
}
