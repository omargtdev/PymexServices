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
    public interface IInventarioService
    {
        [OperationContract]
        ResponseDataContract RegistrarEntrada(EntradaDC entrada);

        [OperationContract]
        ResponseDataContract RegistrarSalida(SalidaDC salida);
    }

    [DataContract]
    public class EntradaDC
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Codigo { get; set; }

        [DataMember(Order = 3)]
        public DateTime FechaRegistro { get; set; }

        [DataMember(Order = 4)]
        public ProveedorDC Proveedor { get; set; }

        [DataMember(Order = 5)]
        public HistorialSeguimientoDC HistorialSeguimiento { get; set; }

        [DataMember(Order = 6)]
        public IEnumerable<EntradaDetalleDC> DetalleProductos { get; set; }

        [DataMember(Order = 7)]
        public string UsuarioAccion { get; set; }

        public bool ExistenCamposInvalidos()
        {
            return FechaRegistro == null ||
                   Proveedor == null ||
                   DetalleProductos == null ||
                   string.IsNullOrEmpty(UsuarioAccion);
        }
    }

    [DataContract]
    public class EntradaDetalleDC
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public ProductoDC Producto { get; set; }

        [DataMember(Order = 3)]
        public decimal PrecioCompraUnidad { get; set; }

        [DataMember(Order = 4)]
        public decimal PrecioVentaUnidad { get; set; }

        [DataMember(Order = 5)]
        public int Cantidad { get; set; }
    }

    [DataContract]
    public class SalidaDC
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Codigo { get; set; }

        [DataMember(Order = 3)]
        public DateTime FechaRegistro { get; set; }

        [DataMember(Order = 4)]
        public ClienteDC Cliente { get; set; }

        [DataMember(Order = 5)]
        public HistorialSeguimientoDC HistorialSeguimiento { get; set; }

        [DataMember(Order = 6)]
        public IEnumerable<SalidaDetalleDC> DetalleProductos { get; set; }

        [DataMember(Order = 7)]
        public string UsuarioAccion { get; set; }

        public bool ExistenCamposInvalidos()
        {
            return FechaRegistro == null ||
                   Cliente == null ||
                   DetalleProductos == null ||
                   string.IsNullOrEmpty(UsuarioAccion);
        }
    }

    [DataContract]
    public class SalidaDetalleDC
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public ProductoDC Producto { get; set; }

        [DataMember(Order = 3)]
        public decimal PrecioVentaUnidad { get; set; }

        [DataMember(Order = 4)]
        public int Cantidad { get; set; }
    }
}
