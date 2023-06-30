using Pymex.Services.Contracts;
using Pymex.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Pymex.Services.Mappers;
using Pymex.Services.Mappers.Contracts;
using System.Data.Entity;

namespace Pymex.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProductoService" in both code and config file together.
    public class ProductoService : IProductoService
    {

        private readonly IProductoMapper _mapper = new ProductoMapper(new AlmacenMapper(), new CategoriaMapper());

        public ResponseDataContract Actualizar(ProductoDC dataContract)
        {
            var response = new ResponseDataContract();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Producto producto = (from productoEntity in db.Producto
                                         where productoEntity.ProductoID == dataContract.Id
                                         select productoEntity).FirstOrDefault();

                    if (producto == null)
                    {
                        response.Mensaje = "El producto a actualizar no existe.";
                        return response;
                    }

                    var productoPorCodigo = db.Producto.Where(p => p.Codigo == dataContract.Codigo && p.ProductoID != dataContract.Id).FirstOrDefault();
                    if (productoPorCodigo != null)
                    {
                        response.Mensaje = "Ya existe un producto con ese código";
                        return response;

                    }

                    _mapper.ToEditEntity(producto, dataContract);

                    db.SaveChanges();
                }

                response.Mensaje = "Se actualizó el producto correctamente.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al actualizar el producto.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseDataContract Crear(ProductoDC dataContract)
        {
            var response = new ResponseDataContract();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var productoPorCodigo = db.Producto.Where(p => p.Codigo == dataContract.Codigo).FirstOrDefault();
                    if (productoPorCodigo != null)
                    {
                        response.Mensaje = "Ya existe un producto con ese código.";
                        return response;

                    }

                    var producto = _mapper.ToCreateEntity(dataContract);
                    db.Producto.Add(producto);
                    db.SaveChanges();
                }

                response.Mensaje = "Se creó el producto correctamente!";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al insertar el producto.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseDataContract ActivarPorCodigo(ProductoDC dataContract)
        {
            var response = new ResponseDataContract();

            try
            {
                using(PymexEntities db = new PymexEntities())
                {
                    var productoPorCodigo = db.Producto.Where(p => p.Codigo == dataContract.Codigo).FirstOrDefault();
                    if (productoPorCodigo == null)
                    {
                        response.Mensaje = "No existe un producto con ese código.";
                        return response;

                    }

                    _mapper.ToActivateProduct(productoPorCodigo, dataContract);
                    db.SaveChanges();
                }
                
                response.Mensaje = $"Se  {(dataContract.Activo ? "activó" : "desactivó")} el producto correctamente.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = $"Ups! Ocurrió un error al {(dataContract.Activo ? "activar" : "desactivar")} el producto.";
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<ProductoDC>> Listar()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<ProductoDC>>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    response.Data = (from producto in db.Producto
                                     select producto)
                                     .Include(p => p.Almacen)
                                     .Include(p => p.Categoria)
                                     .ToList()
                                      .Select(p => _mapper.ToDataContract(p));
                }

                response.Mensaje = "Datos encontrados.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al obtener los registros.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseWithDataDataContract<ProductoDC> ObtenerPorCodigo(string codigo)
        {
            var response = new ResponseWithDataDataContract<ProductoDC>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Producto producto = db.Producto.Where(p => p.Codigo == codigo)
                                        .Include(p => p.Almacen)
                                        .Include (p => p.Categoria)
                                        .FirstOrDefault();
                    if (producto == null)
                    {
                        response.Mensaje = "No existe el registro.";
                        return response;
                    }

                    // Obteniendo datos del producto
                    response.Data = _mapper.ToDataContract(producto);
                }

                response.Mensaje = "Dato encontrado.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al obtener el registro.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseWithDataDataContract<ProductoDC> ObtenerPorId(int id)
        {
            var response = new ResponseWithDataDataContract<ProductoDC>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Producto producto = db.Producto.Where(p => p.ProductoID == id)
                                        .Include(p => p.Almacen)
                                        .Include (p => p.Categoria)
                                        .FirstOrDefault();
                    if (producto == null)
                    {
                        response.Mensaje = "No existe el registro.";
                        return response;
                    }

                    // Obteniendo datos del producto
                    response.Data = _mapper.ToDataContract(producto);
                }

                response.Mensaje = "Dato encontrado.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups!. Ocurrio un error al obtener el registro.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<ProductoDC>> ListarSoloActivos()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<ProductoDC>>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    response.Data = (from producto in db.Producto
                                     where producto.Activo == true
                                     select producto)
                                    .Include(p => p.Almacen)
                                    .Include (p => p.Categoria)
                                    .ToList()
                                    .Select(p => _mapper.ToDataContract(p));
                }

                response.Mensaje = "Datos encontrados.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrio un error al obtener los registros.";
                // Log Exception ...
            }

            return response;
        }
    }
}
