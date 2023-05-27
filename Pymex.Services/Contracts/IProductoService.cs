using Pymex.Services.Models;
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

    }

    [DataContract]
    public class ProductoDC
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string Codigo { get; set; }
        [DataMember] public string Descripcion { get; set; }
        [DataMember] public short CategoriaId { get; set; }
        [DataMember] public short AlmacenId { get; set; }
        [DataMember] public decimal UltimoPrecioCompra { get; set; }
        [DataMember] public decimal UltimoPrecioVenta { get; set; }
        [DataMember] public int Stock { get; set; }
        [DataMember] public DateTime FechaRegistro { get; set; }
        [DataMember] public string UsuarioAccion { get; set; }
    }

}
