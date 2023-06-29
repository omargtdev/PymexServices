using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Pymex.Services.Contracts;
using Pymex.Services.Mappers;
using Pymex.Services.Mappers.Contracts;
using Pymex.Services.Models;
using Pymex.Services.ValueObjects;

namespace Pymex.Services
{
    public class ReportesService : IReportesService
    {

        private readonly IDataContractMapper<Producto, ProductoDC> _productoMapper;
        private readonly IDataContractMapper<Cliente, ClienteDC> _clienteMapper;
        private readonly IInventarioMapper _inventarioMapper;

        public ReportesService()
        {
            _productoMapper = new ProductoMapper(new AlmacenMapper(), new CategoriaMapper());
            _clienteMapper = new ClienteMapper();
            _inventarioMapper = new InventarioMapper(_productoMapper, new ProveedorMapper(), _clienteMapper);
        }

        public ResponseWithDataDataContract<IEnumerable<ClienteDC>> ObtenerClientesSinNingunaCompra()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<ClienteDC>>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    // Los clientes que no esten presentes en los registros de entrada (ClienteID)
                    var clientes = (from cliente in db.Cliente
                                    where !db.Salida.Any(s => cliente.ClienteID == s.ClienteID)
                                    select cliente)
                                    .ToList()
                                    .Select(c => _clienteMapper.ToDataContract(c));

                    response.Data = clientes;
                }

                response.Mensaje = "Se obuvieron los registros.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al intentar obtener los registros.";
            }

            return response;
        }

        public ResponseWithDataDataContract<ProductoDC> ObtenerProductoMasVendido()
        {
            var response = new ResponseWithDataDataContract<ProductoDC>();

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
                                   select producto)
                                    .Include(p => p.Almacen)
                                    .Include (p => p.Categoria)
                                   .FirstOrDefault();

                    response.Data = _productoMapper.ToDataContract(productoMasVendido);

                }

                response.Mensaje = "Se obtuvo el producto.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrio un error al intentar obtener el producto";
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<SalidaDC>> ObtenerSalidasPorCliente(string numeroDocumento, DateTime fechaInicio, DateTime fechaFin)
        {
            var response = new ResponseWithDataDataContract<IEnumerable<SalidaDC>>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Cliente cliente = db.Cliente.Where(c => c.NumeroDocumento == numeroDocumento).FirstOrDefault();
                    if (cliente == null)
                    {
                        response.Mensaje = "El cliente con el número de documento dado no existe.";
                        return response;
                    }

                    // Buscar las entradas del proveedor
                    var salidas = (from salida in db.Salida
                                   where
                                    salida.ClienteID == cliente.ClienteID &&
                                    (salida.FechaHoraRegistro >= fechaInicio && salida.FechaHoraRegistro <= fechaFin)
                                   select salida)
                                   .ToList()
                                   .Select(s => _inventarioMapper.ToSalidaWithClienteDataContract(s, cliente));

                    response.Data = salidas;

                }

                response.Mensaje = "Se obtuvo los registros.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrio un error al intentar obtener las salidas";
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<EntradaDC>> ObtenerEntradasPorProveedor(string numeroDocumento, DateTime fechaInicio, DateTime fechaFin)
        {
            var response = new ResponseWithDataDataContract<IEnumerable<EntradaDC>>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Proveedor proveedor = db.Proveedor.Where(p => p.NumeroDocumento == numeroDocumento).FirstOrDefault();
                    if (proveedor == null)
                    {
                        response.Mensaje = "El proveedor con el número de documento dado no existe.";
                        return response;
                    }

                    // Buscar las entradas del proveedor
                    var entradas = (from entrada in db.Entrada
                                    where
                                     entrada.ProveedorID == proveedor.ProveedorID &&
                                     (entrada.FechaHoraRegistro >= fechaInicio && entrada.FechaHoraRegistro <= fechaFin)
                                    select entrada)
                                    .ToList()
                                    .Select(e => _inventarioMapper.ToEntradaWithProveedorDataContract(e, proveedor));

                    response.Data = entradas;
                }

                response.Mensaje = "Se obtuvo los registros.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrio un error al intentar obtener las entradas.";
            }

            return response;
        }
    }
}
