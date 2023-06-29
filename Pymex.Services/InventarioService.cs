using Pymex.Services.Contracts;
using Pymex.Services.Mappers;
using Pymex.Services.Mappers.Contracts;
using Pymex.Services.Models;
using Pymex.Services.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Pymex.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "InventarioService" in both code and config file together.
    public class InventarioService : IInventarioService
    {

        private readonly IInventarioMapper _mapper;

        public InventarioService()
        {
            _mapper = new InventarioMapper(
                new ProductoMapper(
                    new AlmacenMapper(),
                    new CategoriaMapper()
                )
            );
        }


        public ResponseWithDataDataContract<EntradaDC> BuscarEntradaPorCodigo(string codigo)
        {
            var response = new ResponseWithDataDataContract<EntradaDC>();

            try
            {
                using(PymexEntities db = new PymexEntities())
                {
                    var entrada = db.usp_BuscarEntradaPorCodigo(codigo).FirstOrDefault();
                    if(entrada == null)
                    {
                        response.Mensaje = "No existe una entrada con ese código.";
                        return response;
                    }

                    var entradaProductos = (from entradaDetalle in db.EntradaProducto
                                            where entradaDetalle.EntradaID == entrada.EntradaID
                                            select entradaDetalle)
                                            .Include(ed => ed.Producto) // Incluyendo Producto en el detalle
                                            .Include(ed => ed.Producto.Almacen)
                                            .Include (ed => ed.Producto.Categoria)
                                            .ToList()
                                            .Select(ed => _mapper.ToDataContract(ed));

                    response.Data = _mapper.ToEntradaWithProductsDataContract(entrada, entradaProductos);
                }

                response.Mensaje = "Se encontró la entrada.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al intentar obtener la entrada.";
            }

            return response;
        }

        public ResponseWithDataDataContract<SalidaDC> BuscarSalidaPorCodigo(string codigo)
        {
            var response = new ResponseWithDataDataContract<SalidaDC>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var salida = db.usp_BuscarSalidaPorCodigo(codigo).FirstOrDefault();
                    if (salida == null)
                    {
                        response.Mensaje = "No existe una salida con ese código.";
                        return response;
                    }

                    var salidaProductos = (from salidaDetalle in db.SalidaProducto
                                            where salidaDetalle.SalidaID == salida.SalidaID
                                            select salidaDetalle)
                                            .Include(sd => sd.Producto) // Incluyendo Producto en el detalle
                                            .Include(sd => sd.Producto.Almacen)
                                            .Include (sd => sd.Producto.Categoria)
                                            .ToList()
                                            .Select(sd => _mapper.ToDataContract(sd));

                    response.Data = _mapper.ToSalidaWithProductsDataContract(salida, salidaProductos);
                }

                response.Mensaje = "Se encontró la salida.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al intentar obtener la salida.";
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<EntradaDC>> ListarEntradas()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<EntradaDC>>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var entradas = db.usp_ListarEntradas()
                        .AsQueryable()
                        .ToList()
                        .Select(e => _mapper.ToDataContract(e));

                    response.Data = entradas;
                }

                response.Mensaje = "Se obtuvo los registros.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al intentar obtener las entradas.";
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<SalidaDC>> ListarSalidas()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<SalidaDC>>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var salidas = db.usp_ListarSalidas()
                        .AsQueryable()
                        .ToList()
                        .Select(s => _mapper.ToDataContract(s));

                    response.Data = salidas;
                }

                response.Mensaje = "Se obtuvo los registros.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al intentar obtener las salidas.";
            }

            return response;
        }

        public ResponseDataContract RegistrarEntrada(EntradaDC entrada)
        {
            var response = new ResponseDataContract();

            if (entrada.ExistenCamposInvalidos())
            {
                response.Mensaje = "Se tiene que completar los campos requeridos.";
                return response;
            }

            try
            {
                // Convirtiendo el detalle en un XML
                var productosXML = _mapper.ToEntradaProductsXML(entrada.DetalleProductos);
                    
                using (PymexEntities db = new PymexEntities())
                {
                    int rowsAffected = db.usp_RegistrarEntrada(entrada.FechaRegistro, entrada.UsuarioAccion, entrada.Proveedor.Id, productosXML);
                    if (rowsAffected <= 0)
                        throw new Exception("No se registró ningún registro");

                }

                response.Mensaje = "Se registró la entrada correctamente!";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al tratar de registrar.";
            }

            return response;
        }

        public ResponseDataContract RegistrarSalida(SalidaDC salida)
        {
            var response = new ResponseDataContract();

            if (salida.ExistenCamposInvalidos())
            {
                response.Mensaje = "Se tiene que completar los campos requeridos";
                return response;
            }

            try
            {
                // Convirtiendo el detalle en un XML
                var productosXML = _mapper.ToSalidaProductsXML(salida.DetalleProductos);

                using (PymexEntities db = new PymexEntities())
                {
                    int rowsAffected = db.usp_RegistrarSalida(salida.FechaRegistro, salida.UsuarioAccion, salida.Cliente.Id, productosXML);
                    if (rowsAffected <= 0)
                        throw new Exception("No se registró ningún registro");
                }

                response.Mensaje = "Se registró la salida correctamente!";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups!. Ocurrio un error al tratar de registrar.";
            }

            return response;
        }
    }
}
