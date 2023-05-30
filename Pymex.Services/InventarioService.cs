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
        public ResponseWithDataDataContract<IEnumerable<ClienteDC>> ObtenerClientesSinNingunaCompra()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<ClienteDC>>();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    // Los clientes que no esten presentes en los registros de entrada (ClienteID)
                    var clientes = (from cliente in db.Cliente
                                    where !db.Salida.Any(s => cliente.ClienteID == s.ClienteID)
                                    select new ClienteDC
                                    {
                                        Id = cliente.ClienteID,
                                        NombreCompleto = cliente.NombreCompleto,
                                        TipoDocumento = (TipoDocumento)cliente.TipoDocumento,
                                        NumeroDocumento = cliente.NumeroDocumento,
                                        HistorialSeguimiento = new HistorialSeguimientoDC
                                        {
                                            FechaRegistro = cliente.FechaRegistro,
                                            UsuarioRegistro = cliente.UsuarioRegistro,
                                            FechaModificacion = cliente.FechaModificacion,
                                            UltimoUsuarioModificacion = cliente.UltimoUsuarioModifico
                                        }
                                    }).ToList();

                    response.Data = clientes;
                    response.Mensaje = "Se obuvieron los registros.";

                }
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = "Ups! Ocurrio un error al intentar obtener los registros";
            }

            return response;
        }

        /// <summary>
        ///     Obtener el producto más adquirido por los clientes, es decir, de las entradas
        ///     presentadas.
        /// </summary>
        /// <returns>La respuesta con el producto más vendido</returns>
        public ResponseWithDataDataContract<ProductoDC> ObtenerProductoMasVendido()
        {
            var response = new ResponseWithDataDataContract<ProductoDC>();
            response.EsCorrecto = true;

             try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var productoMasVendido =
                                  (from producto in db.Producto
                                   where producto.ProductoID == (
                                        (from salidaProducto in db.SalidaProducto
                                         group salidaProducto by salidaProducto.ProductoID into newGroup // Agrupando por el ID Producto
                                         orderby newGroup.Sum(g => g.Cantidad) descending // Ordenando por la suma de la cantidad ascendentemente
                                         select newGroup)
                                            .Take(1) // TOP 1
                                            .FirstOrDefault()
                                            .Key) // ProductoID
                                    select new ProductoDC 
                                    {
                                        Id = producto.ProductoID,
                                        Codigo = producto.Codigo,
                                        Descripcion = producto.Descripcion,
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
                                    }).FirstOrDefault();
                                

                    response.Data = productoMasVendido;
                    response.Mensaje = "Se obtuvo el producto.";

                }
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = "Ups! Ocurrio un error al intentar obtener el producto";
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<SalidaDC>> ObtenerSalidasPorCliente(string numeroDocumento, DateTime fechaInicio, DateTime fechaFin)
        {
            var response = new ResponseWithDataDataContract<IEnumerable<SalidaDC>>();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Cliente cliente = db.Cliente.Where(c => c.NumeroDocumento == numeroDocumento).FirstOrDefault();
                    if (cliente == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "El cliente con el número de documento dado no existe.";
                        return response;
                    }

                    // Buscar las entradas del proveedor
                    var salidas = (from salida in db.Salida
                                   where 
                                    salida.ClienteID == cliente.ClienteID &&
                                    (salida.FechaHoraRegistro >= fechaInicio && salida.FechaHoraRegistro <= fechaFin)
                                   select new SalidaDC // Cabecera
                                   {
                                       Id = salida.SalidaID,
                                       Codigo = salida.Codigo,
                                       FechaRegistro = salida.FechaRegistro,
                                       Cliente = new ClienteDC
                                       {
                                           Id = cliente.ClienteID,
                                           TipoDocumento = (TipoDocumento)cliente.TipoDocumento,
                                           NumeroDocumento = cliente.NumeroDocumento,
                                           NombreCompleto = cliente.NombreCompleto,
                                           HistorialSeguimiento = new HistorialSeguimientoDC
                                           {
                                               FechaRegistro = cliente.FechaRegistro,
                                               UsuarioRegistro = cliente.UsuarioRegistro,
                                               FechaModificacion = cliente.FechaModificacion,
                                               UltimoUsuarioModificacion = cliente.UltimoUsuarioModifico
                                           }
                                       },
                                       DetalleProductos = ( // Detalle
                                            from salidaProducto in db.SalidaProducto
                                            where salidaProducto.SalidaID == salida.SalidaID
                                            select new SalidaDetalleDC
                                            {
                                                Id = salidaProducto.SalidaProductoID,
                                                PrecioVentaUnidad = salidaProducto.PrecioVentaUnidad,
                                                Producto = (
                                                    (from producto in db.Producto
                                                    where producto.ProductoID == salidaProducto.ProductoID
                                                    select new ProductoDC
                                                    {
                                                        Id = producto.ProductoID,
                                                        Codigo = producto.Codigo,
                                                        Descripcion = producto.Descripcion,
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
                                                    }).FirstOrDefault()
                                                ),
                                                Cantidad = salidaProducto.Cantidad
                                            }
                                        ),
                                       HistorialSeguimiento = new HistorialSeguimientoDC
                                       {
                                           FechaRegistro = salida.FechaHoraRegistro,
                                           UsuarioRegistro = salida.UsuarioRegistro,
                                           FechaModificacion = null,
                                           UltimoUsuarioModificacion = null
                                       }
                                   }
                                  ).ToList();
                        
                    response.Data = salidas;
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

        ResponseWithDataDataContract<IEnumerable<EntradaDC>> IInventarioService.ObtenerEntradasPorProveedor(string numeroDocumento, DateTime fechaInicio, DateTime fechaFin)
        {
            var response = new ResponseWithDataDataContract<IEnumerable<EntradaDC>>();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Proveedor proveedor = db.Proveedor.Where(p => p.NumeroDocumento == numeroDocumento).FirstOrDefault();
                    if (proveedor == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "El proveedor con el número de documento dado no existe.";
                        return response;
                    }

                    // Buscar las entradas del proveedor
                    var entradas = (from entrada in db.Entrada
                                   where
                                    entrada.ProveedorID == proveedor.ProveedorID &&
                                    (entrada.FechaHoraRegistro >= fechaInicio && entrada.FechaHoraRegistro <= fechaFin)
                                   select new EntradaDC
                                   {
                                       Id = entrada.EntradaID,
                                       Codigo = entrada.Codigo,
                                       FechaRegistro = entrada.FechaRegistro,
                                       Proveedor = new ProveedorDC // Cabecera
                                       {
                                           Id = proveedor.ProveedorID,
                                           TipoDocumento = (TipoDocumento)proveedor.TipoDocumento,
                                           NumeroDocumento = proveedor.NumeroDocumento,
                                           NombreCompleto = proveedor.NombreCompleto,
                                           HistorialSeguimiento = new HistorialSeguimientoDC
                                           {
                                               FechaRegistro = proveedor.FechaRegistro,
                                               UsuarioRegistro = proveedor.UsuarioRegistro,
                                               FechaModificacion = proveedor.FechaModificacion,
                                               UltimoUsuarioModificacion = proveedor.UltimoUsuarioModifico
                                           }
                                       },
                                       DetalleProductos = ( // Detalle
                                            from entradaProducto in db.EntradaProducto
                                            where entradaProducto.EntradaID == entrada.EntradaID
                                            select new EntradaDetalleDC
                                            {
                                                Id = entradaProducto.EntradaProductoID,
                                                PrecioCompraUnidad = entradaProducto.PrecioCompraUnidad,
                                                PrecioVentaUnidad = entradaProducto.PrecioVentaUnidad,
                                                Producto = (
                                                    (from producto in db.Producto
                                                     where producto.ProductoID == entradaProducto.ProductoID
                                                     select new ProductoDC
                                                     {
                                                         Id = producto.ProductoID,
                                                         Codigo = producto.Codigo,
                                                         Descripcion = producto.Descripcion,
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
                                                     }).FirstOrDefault()
                                                ),
                                                Cantidad = entradaProducto.Cantidad
                                            }
                                        ),
                                       HistorialSeguimiento = new HistorialSeguimientoDC
                                       {
                                           FechaRegistro = entrada.FechaHoraRegistro,
                                           UsuarioRegistro = entrada.UsuarioRegistro,
                                           FechaModificacion = null,
                                           UltimoUsuarioModificacion = null
                                       }
                                   }
                                  ).ToList();

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
    }
}
