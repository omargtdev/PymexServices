using Pymex.Services.Models;
using Pymex.Services.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Pymex.Services.Contracts
{
    [ServiceContract]
    public interface IProductoService : IGenericService<ProductoDC>
    {
        [OperationContract]
        ResponseWithDataDataContract<ProductoDC> ObtenerPorCodigo(string codigo);
    }

    [DataContract]
    public class ProductoDC
    {
        [DataMember(Order = 1)] public int Id { get; set; }
        [DataMember(Order = 2)] public string Codigo { get; set; }
        [DataMember(Order = 3)] public string Descripcion { get; set; }
        [DataMember(Order = 4)] public short CategoriaId { get; set; }
        [DataMember(Order = 5)] public short AlmacenId { get; set; }
        [DataMember(Order = 6)] public decimal UltimoPrecioCompra { get; set; }
        [DataMember(Order = 7)] public decimal UltimoPrecioVenta { get; set; }
        [DataMember(Order = 8)] public int Stock { get; set; }
        [DataMember(Order = 9)] public HistorialSeguimientoDC HistorialSeguimiento { get; set; }
        [DataMember(Order = 10)] public string UsuarioAccion { get; set; }
    }

}
