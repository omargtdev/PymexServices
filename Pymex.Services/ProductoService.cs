using Pymex.Services.Contracts;
using Pymex.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Pymex.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProductoService" in both code and config file together.
    public class ProductoService : IProductoService
    {
        public ResponseDataContract Actualizar(ProductoDC entity)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Producto producto = (from productoEntity in db.Producto
                                         where productoEntity.ProductoID == entity.Id
                                         select productoEntity).FirstOrDefault();

                    if (producto == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "El producto a actualizar no existe.";
                        return response;
                    }

                    var productoPorCodigo = db.Producto.Where(p => p.Codigo == entity.Codigo && p.ProductoID != entity.Id).FirstOrDefault();
                    if (productoPorCodigo != null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "Ya existe un producto con ese código";
                        return response;

                    }

                    producto.Codigo = entity.Codigo;
                    producto.Descripcion = entity.Descripcion;
                    producto.CategoriaID = entity.CategoriaId;
                    producto.AlmacenID = entity.AlmacenId;
                    producto.UltimoUsuarioModifico = entity.UsuarioAccion;
                    producto.FechaModificacion = DateTime.Now;

                    db.SaveChanges();

                    response.Mensaje = "Se actualizó el producto correctamente";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al actualizar el producto.";
                response.EsCorrecto = false;
                // Log Exception ...
            }

            return response;
        }

        public ResponseDataContract Crear(ProductoDC entity)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var productoPorCodigo = db.Producto.Where(p => p.Codigo == entity.Codigo).FirstOrDefault();
                    if (productoPorCodigo != null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "Ya existe un producto con ese código";
                        return response;

                    }

                    Producto producto = new Producto();
                    producto.ProductoID = 0;
                    producto.Codigo = entity.Codigo;
                    producto.Descripcion = entity.Descripcion;
                    producto.CategoriaID = entity.CategoriaId;
                    producto.AlmacenID = entity.AlmacenId;
                    producto.UsuarioRegistro = entity.UsuarioAccion;
                    producto.FechaRegistro = DateTime.Now;

                    db.Producto.Add(producto);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al insertar el producto.";
                response.EsCorrecto = false;
                // Log Exception ...
            }

            return response;
        }

        public ResponseDataContract Eliminar(int id)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Producto producto = (from productoEntity in db.Producto
                                         where productoEntity.ProductoID == id
                                         select productoEntity).FirstOrDefault();

                    if (producto == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "El producto a eliminar no existe.";
                        return response;
                    }

                    db.Producto.Remove(producto);
                    db.SaveChanges();

                    response.Mensaje = "Se eliminó correctamente el producto!";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al eliminar el producto.";
                response.EsCorrecto = false;
                // Log Exception ...
            }

            return response;
        }


        public ResponseWithDataDataContract<IEnumerable<ProductoDC>> Listar()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<ProductoDC>>();
            response.EsCorrecto = true;
            response.Mensaje = "Datos encontrados.";

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    response.Data = (from producto in db.Producto
                                     select new ProductoDC
                                     {
                                         Id = producto.ProductoID,
                                         Codigo = producto.Codigo,
                                         Descripcion = producto.Descripcion,
                                         UltimoPrecioCompra = producto.UltimoPrecioCompra.HasValue ? (decimal)producto.UltimoPrecioCompra : 0,
                                         UltimoPrecioVenta = producto.UltimoPrecioVenta.HasValue ? (decimal)producto.UltimoPrecioVenta : 0,
                                         Stock = producto.Stock.HasValue ? (int)producto.Stock : 0,
                                         FechaRegistro = producto.FechaRegistro,
                                         CategoriaId = producto.CategoriaID,
                                         AlmacenId = producto.AlmacenID
                                     }).ToList();
                }
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = "Ups!. Ocurrio un error al obtener los registros.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseWithDataDataContract<ProductoDC> ObtenerPorCodigo(string codigo)
        {
            var response = new ResponseWithDataDataContract<ProductoDC>();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    //Producto producto = db.Producto.Find(id); // Find: Busca primera en la memoria cache de EF, antes de hacer el query
                    Producto producto = db.Producto.Where(p => p.Codigo == codigo).FirstOrDefault();
                    if (producto != null)
                    {
                        response.Mensaje = "Dato encontrado.";
                        // Obteniendo datos del producto
                        response.Data = new ProductoDC
                        {
                            Id = producto.ProductoID,
                            Codigo = producto.Codigo,
                            Descripcion = producto.Descripcion,
                            CategoriaId = producto.CategoriaID,
                            AlmacenId = producto.AlmacenID,
                            UltimoPrecioCompra = producto.UltimoPrecioCompra.HasValue ? (decimal)producto.UltimoPrecioCompra : 0,
                            UltimoPrecioVenta = producto.UltimoPrecioVenta.HasValue ? (decimal)producto.UltimoPrecioVenta : 0,
                            Stock = producto.Stock.HasValue ? (int)producto.Stock : 0,
                            FechaRegistro = producto.FechaRegistro
                        };
                    }
                    else
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "No existe el registro.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = "Ups!. Ocurrio un error al obtener el registro.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseWithDataDataContract<ProductoDC> ObtenerPorId(int id)
        {
            var response = new ResponseWithDataDataContract<ProductoDC>();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    //Producto producto = db.Producto.Find(id); // Find: Busca primera en la memoria cache de EF, antes de hacer el query
                    Producto producto = db.Producto.Where(p => p.ProductoID == id).FirstOrDefault();
                    if (producto != null)
                    {
                        response.Mensaje = "Dato encontrado.";
                        // Obteniendo datos del producto
                        response.Data = new ProductoDC
                        {
                            Id = producto.ProductoID,
                            Codigo = producto.Codigo,
                            Descripcion = producto.Descripcion,
                            CategoriaId = producto.CategoriaID,
                            AlmacenId = producto.AlmacenID,
                            UltimoPrecioCompra = producto.UltimoPrecioCompra.HasValue ? (decimal)producto.UltimoPrecioCompra : 0,
                            UltimoPrecioVenta = producto.UltimoPrecioVenta.HasValue ? (decimal)producto.UltimoPrecioVenta : 0,
                            Stock = producto.Stock.HasValue ? (int)producto.Stock : 0,
                            FechaRegistro = producto.FechaRegistro
                        };
                    }
                    else
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "No existe el registro.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = "Ups!. Ocurrio un error al obtener el registro.";
                // Log Exception ...
            }

            return response;
        }
    }
}
