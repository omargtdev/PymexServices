using Pymex.Services.Contracts;
using Pymex.Services.Models;
using Pymex.Services.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.PeerResolvers;
using System.Text;
using System.Xml.Linq;

namespace Pymex.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "InventarioService" in both code and config file together.
    public class InventarioService : IInventarioService
    {
        public ResponseWithDataDataContract<EntradaDC> BuscarEntradaPorCodigo(string codigo)
        {
            var response = new ResponseWithDataDataContract<EntradaDC>();
            response.EsCorrecto = true;

            try
            {
                using(PymexEntities db = new PymexEntities())
                {
                    var entrada = db.usp_BuscarEntradaPorCodigo(codigo).FirstOrDefault();
                    if(entrada == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "No existe una entrada con ese codigo";
                        return response;
                    }

                    var entradaProductos = (from entradaDetalle in db.EntradaProducto
                                            where entradaDetalle.EntradaID == entrada.EntradaID
                                            join producto in db.Producto on entradaDetalle.Producto equals producto
                                            select new EntradaDetalleDC
                                            {
                                                Id = entradaDetalle.EntradaProductoID,
                                                Producto = new ProductoDC
                                                {
                                                    Id = producto.ProductoID,
                                                    Codigo = producto.Codigo,
                                                    Descripcion = producto.Descripcion,
                                                    Activo = producto.Activo,
                                                    UltimoPrecioCompra = producto.UltimoPrecioCompra.HasValue ? (decimal)producto.UltimoPrecioCompra : 0,
                                                    UltimoPrecioVenta = producto.UltimoPrecioVenta.HasValue ? (decimal)producto.UltimoPrecioVenta : 0,
                                                    Stock = producto.Stock.HasValue ? (int)producto.Stock : 0,
                                                    HistorialSeguimiento = new HistorialSeguimientoDC
                                                    {
                                                        FechaRegistro = producto.FechaRegistro,
                                                        UsuarioRegistro = producto.UsuarioRegistro,
                                                        FechaModificacion = producto.FechaModificacion,
                                                        UltimoUsuarioModificacion = producto.UltimoUsuarioModifico
                                                    },
                                                    CategoriaId = producto.CategoriaID,
                                                    AlmacenId = producto.AlmacenID
                                                },
                                                PrecioCompraUnidad = entradaDetalle.PrecioCompraUnidad,
                                                PrecioVentaUnidad = entradaDetalle.PrecioVentaUnidad,
                                                Cantidad = entradaDetalle.Cantidad,
                                                
                                            }).ToList();

                    response.Data = new EntradaDC
                    {
                        Id = (int)entrada.EntradaID,
                        Codigo = entrada.Codigo,
                        FechaRegistro = entrada.FechaRegistro.Value,
                        DetalleProductos = entradaProductos,
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
                    response.Mensaje = "Se encontró la entrada";
                }
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = "Ups! Ocurrio un error al intentar obtener la entrada";
            }

            return response;
        }

        public ResponseWithDataDataContract<SalidaDC> BuscarSalidaPorCodigo(string codigo)
        {
            var response = new ResponseWithDataDataContract<SalidaDC>();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var salida = db.usp_BuscarSalidaPorCodigo(codigo).FirstOrDefault();
                    if (salida == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "No existe una salida con ese codigo";
                        return response;
                    }

                    var salidaProductos = (from salidaDetalle in db.SalidaProducto
                                            where salidaDetalle.SalidaID == salida.SalidaID
                                            join producto in db.Producto on salidaDetalle.Producto equals producto
                                            select new SalidaDetalleDC
                                            {
                                                Id = salidaDetalle.SalidaProductoID,
                                                Producto = new ProductoDC
                                                {
                                                    Id = producto.ProductoID,
                                                    Codigo = producto.Codigo,
                                                    Descripcion = producto.Descripcion,
                                                    Activo = producto.Activo,
                                                    UltimoPrecioCompra = producto.UltimoPrecioCompra.HasValue ? (decimal)producto.UltimoPrecioCompra : 0,
                                                    UltimoPrecioVenta = producto.UltimoPrecioVenta.HasValue ? (decimal)producto.UltimoPrecioVenta : 0,
                                                    Stock = producto.Stock.HasValue ? (int)producto.Stock : 0,
                                                    HistorialSeguimiento = new HistorialSeguimientoDC
                                                    {
                                                        FechaRegistro = producto.FechaRegistro,
                                                        UsuarioRegistro = producto.UsuarioRegistro,
                                                        FechaModificacion = producto.FechaModificacion,
                                                        UltimoUsuarioModificacion = producto.UltimoUsuarioModifico
                                                    },
                                                    CategoriaId = producto.CategoriaID,
                                                    AlmacenId = producto.AlmacenID
                                                },
                                                PrecioVentaUnidad = salidaDetalle.PrecioVentaUnidad,
                                                Cantidad = salidaDetalle.Cantidad,

                                            }).ToList();

                    response.Data = new SalidaDC
                    {
                        Id = (int)salida.SalidaID,
                        Codigo = salida.Codigo,
                        FechaRegistro = salida.FechaRegistro.Value,
                        DetalleProductos = salidaProductos,
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
                    response.Mensaje = "Se encontró la salida";
                }
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = "Ups! Ocurrio un error al intentar obtener la salida";
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<EntradaDC>> ListarEntradas()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<EntradaDC>>();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var entradas = db.usp_ListarEntradas().AsQueryable().Select(entrada => new EntradaDC
                    {
                        Id = entrada.EntradaID,
                        Codigo = entrada.Codigo,
                        FechaRegistro = entrada.FechaRegistro,
                        Proveedor = new ProveedorDC
                        {
                            Id = entrada.EntradaID,
                            TipoDocumento = (TipoDocumento)entrada.TipoDocumento,
                            NumeroDocumento = entrada.NumeroDocumento,
                            NombreCompleto = entrada.NombreCompleto,
                            HistorialSeguimiento = new HistorialSeguimientoDC
                            {
                                FechaRegistro = entrada.ProveedorFechaRegistro.Value,
                                UsuarioRegistro = entrada.ProveedorUsuarioRegistro,
                                FechaModificacion = entrada.ProveedorFechaModificacion,
                                UltimoUsuarioModificacion = entrada.ProveedorUltimoUsuarioModifico
                            },
                        },
                        HistorialSeguimiento = new HistorialSeguimientoDC
                        {
                            FechaRegistro = entrada.EntradaFechaRegistro,
                            UsuarioRegistro = entrada.EntradaUsuarioRegistro,
                            FechaModificacion = null,
                            UltimoUsuarioModificacion = null
                        }
                    }).ToList();


                    response.Data = entradas;
                    response.Mensaje = "Se obtuvo los registros.";

                }
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = "Ups! Ocurrio un error al intentar obtener las entradas";
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<SalidaDC>> ListarSalidas()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<SalidaDC>>();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var entradas = db.usp_ListarSalidas().AsQueryable().Select(entrada => new SalidaDC
                    {
                        Id = entrada.SalidaID,
                        Codigo = entrada.Codigo,
                        FechaRegistro = entrada.FechaRegistro,
                        Cliente = new ClienteDC
                        {
                            Id = entrada.ClienteID,
                            TipoDocumento = (TipoDocumento)entrada.TipoDocumento,
                            NumeroDocumento = entrada.NumeroDocumento,
                            NombreCompleto = entrada.NombreCompleto,
                            HistorialSeguimiento = new HistorialSeguimientoDC
                            {
                                FechaRegistro = entrada.ClienteFechaRegistro.Value,
                                UsuarioRegistro = entrada.ClienteUsuarioRegistro,
                                FechaModificacion = entrada.ClienteFechaModificacion,
                                UltimoUsuarioModificacion = entrada.ClienteUltimoUsuarioModifico
                            },
                        },
                        HistorialSeguimiento = new HistorialSeguimientoDC
                        {
                            FechaRegistro = entrada.SalidaFechaRegistro,
                            UsuarioRegistro = entrada.SalidaUsuarioRegistro,
                            FechaModificacion = null,
                            UltimoUsuarioModificacion = null
                        }
                    }).ToList();


                    response.Data = entradas;
                    response.Mensaje = "Se obtuvo los registros.";

                }
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = "Ups! Ocurrio un error al intentar obtener las salidas";
            }

            return response;
        }

        public ResponseDataContract RegistrarEntrada(EntradaDC entrada)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = false;

            if (entrada.ExistenCamposInvalidos())
            {
                response.Mensaje = "Se tiene que completar los campos requeridos";
                return response;
            }

            try
            {
                // Convirtiendo el detalle en un XML
                XDocument xml = new XDocument(new XElement("Productos")); // Root
                foreach (var detalle in entrada.DetalleProductos)
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
                    
                using (PymexEntities db = new PymexEntities())
                {
                    int rowsAffected = db.usp_RegistrarEntrada(entrada.FechaRegistro, entrada.UsuarioAccion, entrada.Proveedor.Id, xml.ToString());
                    if (rowsAffected <= 0)
                        throw new Exception("No se registró ningún registro");

                    response.EsCorrecto = true;
                    response.Mensaje = "Se registró la entrada correctamente!";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups!. Ocurrio un error al tratar de registrar.";
            }

            return response;
        }

        public ResponseDataContract RegistrarSalida(SalidaDC salida)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = false;

            if (salida.ExistenCamposInvalidos())
            {
                response.Mensaje = "Se tiene que completar los campos requeridos";
                return response;
            }

            try
            {
                // Convirtiendo el detalle en un XML
                XDocument xml = new XDocument(new XElement("Productos")); // Root
                foreach (var detalle in salida.DetalleProductos)
                {
                    var productoNode = new XElement("Producto");
                    productoNode.Add(
                        new XElement("ProductoID", detalle.Producto.Id),
                        new XElement("PrecioVentaUnidad", detalle.PrecioVentaUnidad),
                        new XElement("Cantidad", detalle.Cantidad)
                    );
                    xml.Root.Add(productoNode);
                }

                using (PymexEntities db = new PymexEntities())
                {
                    int rowsAffected = db.usp_RegistrarSalida(salida.FechaRegistro, salida.UsuarioAccion, salida.Cliente.Id, xml.ToString());
                    if (rowsAffected <= 0)
                        throw new Exception("No se registró ningún registro");

                    response.EsCorrecto = true;
                    response.Mensaje = "Se registró la salida correctamente!";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups!. Ocurrio un error al tratar de registrar.";
            }

            return response;
        }
    }
}
