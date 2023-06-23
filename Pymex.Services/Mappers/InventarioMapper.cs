using Pymex.Services.Contracts;
using Pymex.Services.Mappers.Contracts;
using Pymex.Services.Models;
using Pymex.Services.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pymex.Services.Mappers
{
    public class InventarioMapper : IInventarioMapper
    {

        // Inyectando dependencias
        private readonly IDataContractMapper<Producto, ProductoDC> _productoMapper;

        public InventarioMapper(IDataContractMapper<Producto, ProductoDC> productoMapper) 
        {
            _productoMapper = productoMapper; 
        }

        public EntradaDC ToDataContract(usp_ListarEntradas_Result entity)
        {
            return new EntradaDC
            {
                Id = entity.EntradaID,
                Codigo = entity.Codigo,
                FechaRegistro = entity.FechaRegistro,
                Proveedor = new ProveedorDC
                {
                    Id = entity.EntradaID,
                    TipoDocumento = (TipoDocumento)entity.TipoDocumento,
                    NumeroDocumento = entity.NumeroDocumento,
                    NombreCompleto = entity.NombreCompleto,
                    HistorialSeguimiento = new HistorialSeguimientoDC
                    {
                        FechaRegistro = entity.ProveedorFechaRegistro.Value,
                        UsuarioRegistro = entity.ProveedorUsuarioRegistro,
                        FechaModificacion = entity.ProveedorFechaModificacion,
                        UltimoUsuarioModificacion = entity.ProveedorUltimoUsuarioModifico
                    },
                },
                HistorialSeguimiento = new HistorialSeguimientoDC
                {
                    FechaRegistro = entity.EntradaFechaRegistro,
                    UsuarioRegistro = entity.EntradaUsuarioRegistro,
                    FechaModificacion = null,
                    UltimoUsuarioModificacion = null
                }
            };
        }

        public SalidaDC ToDataContract(usp_ListarSalidas_Result entity)
        {
            return new SalidaDC
            {
                Id = entity.SalidaID,
                Codigo = entity.Codigo,
                FechaRegistro = entity.FechaRegistro,
                Cliente = new ClienteDC
                {
                    Id = entity.ClienteID,
                    TipoDocumento = (TipoDocumento)entity.TipoDocumento,
                    NumeroDocumento = entity.NumeroDocumento,
                    NombreCompleto = entity.NombreCompleto,
                    HistorialSeguimiento = new HistorialSeguimientoDC
                    {
                        FechaRegistro = entity.ClienteFechaRegistro.Value,
                        UsuarioRegistro = entity.ClienteUsuarioRegistro,
                        FechaModificacion = entity.ClienteFechaModificacion,
                        UltimoUsuarioModificacion = entity.ClienteUltimoUsuarioModifico
                    },
                },
                HistorialSeguimiento = new HistorialSeguimientoDC
                {
                    FechaRegistro = entity.SalidaFechaRegistro,
                    UsuarioRegistro = entity.SalidaUsuarioRegistro,
                    FechaModificacion = null,
                    UltimoUsuarioModificacion = null
                }
            };
        }

        public EntradaDetalleDC ToDataContract(EntradaProducto entity)
        {
            return new EntradaDetalleDC
            {
                Id = entity.EntradaProductoID,
                Producto = _productoMapper.ToDataContract(entity.Producto),
                PrecioCompraUnidad = entity.PrecioCompraUnidad,
                PrecioVentaUnidad = entity.PrecioVentaUnidad,
                Cantidad = entity.Cantidad,
            };
        }

        public SalidaDetalleDC ToDataContract(SalidaProducto entity)
        {
            return new SalidaDetalleDC
            {
                Id = entity.SalidaProductoID,
                Producto = _productoMapper.ToDataContract(entity.Producto),
                PrecioVentaUnidad = entity.PrecioVentaUnidad,
                Cantidad = entity.Cantidad
            };
        }

        public string ToEntradaProductsXML(IEnumerable<EntradaDetalleDC> detalleProductos)
        {
            XDocument xml = new XDocument(new XElement("Productos")); // Root
            foreach (var detalle in detalleProductos)
            {
                var productoNode = new XElement("Producto");
                productoNode.Add(
                    new XElement("ProductoID", detalle.Producto.Id),
                    new XElement("PrecioCompraUnidad", detalle.PrecioCompraUnidad),
                    new XElement("PrecioVentaUnidad", detalle.PrecioVentaUnidad),
                    new XElement("Cantidad", detalle.Cantidad)
                );
                xml.Root.Add(productoNode);
            }
            
            return xml.ToString();
        }

        public string ToSalidaProductsXML(IEnumerable<SalidaDetalleDC> detalleProductos)
        {
            XDocument xml = new XDocument(new XElement("Productos")); // Root
            foreach (var detalle in detalleProductos)
            {
                var productoNode = new XElement("Producto");
                productoNode.Add(
                    new XElement("ProductoID", detalle.Producto.Id),
                    new XElement("PrecioVentaUnidad", detalle.PrecioVentaUnidad),
                    new XElement("Cantidad", detalle.Cantidad)
                );
                xml.Root.Add(productoNode);
            }

            return xml.ToString();
        }

        public EntradaDC ToEntradaWithProductsDataContract(usp_BuscarEntradaPorCodigo_Result entrada, IEnumerable<EntradaDetalleDC> detalleProductos)
        {
            return new EntradaDC
            {
                Id = (int)entrada.EntradaID,
                Codigo = entrada.Codigo,
                FechaRegistro = entrada.FechaRegistro.Value,
                DetalleProductos = detalleProductos,
                Proveedor = new ProveedorDC
                {
                    Id = (int)entrada.ProveedorID,
                    TipoDocumento = (TipoDocumento)entrada.TipoDocumento,
                    NumeroDocumento = entrada.NumeroDocumento,
                    NombreCompleto = entrada.NombreCompleto,
                    HistorialSeguimiento = new HistorialSeguimientoDC
                    {
                        FechaRegistro = entrada.ProveedorFechaRegistro.Value,
                        UsuarioRegistro = entrada.ProveedorUsuarioRegistro,
                        FechaModificacion = entrada.ProveedorFechaModificacion,
                        UltimoUsuarioModificacion = entrada.ProveedorUltimoUsuarioModifico
                    }
                },
                HistorialSeguimiento = new HistorialSeguimientoDC
                {
                    FechaRegistro = entrada.EntradaFechaRegistro.Value,
                    UsuarioRegistro = entrada.EntradaUsuarioRegistro,
                    FechaModificacion = null,
                    UltimoUsuarioModificacion = null
                }
            };

        }

        public SalidaDC ToSalidaWithProductsDataContract(usp_BuscarSalidaPorCodigo_Result salida, IEnumerable<SalidaDetalleDC> detalleProductos)
        {
            return new SalidaDC
            {
                Id = (int)salida.SalidaID,
                Codigo = salida.Codigo,
                FechaRegistro = salida.FechaRegistro.Value,
                DetalleProductos = detalleProductos,
                Cliente = new ClienteDC
                {
                    Id = (int)salida.SalidaID,
                    TipoDocumento = (TipoDocumento)salida.TipoDocumento,
                    NumeroDocumento = salida.NumeroDocumento,
                    NombreCompleto = salida.NombreCompleto,
                    HistorialSeguimiento = new HistorialSeguimientoDC
                    {
                        FechaRegistro = salida.ClienteFechaRegistro.Value,
                        UsuarioRegistro = salida.ClienteUsuarioRegistro,
                        FechaModificacion = salida.ClienteFechaModificacion,
                        UltimoUsuarioModificacion = salida.ClienteUltimoUsuarioModifico
                    }
                },
                HistorialSeguimiento = new HistorialSeguimientoDC
                {
                    FechaRegistro = salida.SalidaFechaRegistro.Value,
                    UsuarioRegistro = salida.SalidaUsuarioRegistro,
                    FechaModificacion = null,
                    UltimoUsuarioModificacion = null
                }
            };
        }
    }
}
