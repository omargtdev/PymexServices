using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Pymex.Services.Contracts;
using Pymex.Services.Models;
using Pymex.Services.Utils;
using Pymex.Services.ValueObjects;

namespace Pymex.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CategoriaService" in both code and config file together.
    public class CategoriaService : ICategoriaService
    {
        public ResponseDataContract Actualizar(CategoriaDC entity)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Categoria categoria = (from categoriaEntity in db.Categoria
                                           where categoriaEntity.CategoriaID == entity.Id
                                           select categoriaEntity).FirstOrDefault();

                    if (categoria == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "La categoria a actualizar no existe.";
                        return response;
                    }

                    categoria.Descripcion = entity.Descripcion;
                    db.SaveChanges();
                    response.Mensaje = "Se actualizó correctamente la categoria!";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al actualizar la categoria.";
                response.EsCorrecto = false;
                // Log Exception ...
            }

            return response;
        }

        public ResponseDataContract Crear(CategoriaDC entity)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Categoria categoria = new Categoria()
                    {
                        Descripcion = entity.Descripcion
                    };

                    db.Categoria.Add(categoria);
                    db.SaveChanges();
                    response.Mensaje = "Se creó correctamente la categoria!";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al insertar la categoria.";
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
                    Categoria categoria = (from categoriaEntity in db.Categoria
                                       where categoriaEntity.CategoriaID == id
                                       select categoriaEntity).FirstOrDefault();

                    if (categoria == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "La categoria a eliminar no existe.";
                        return response;
                    }

                    db.Categoria.Remove(categoria);
                    db.SaveChanges();

                    response.Mensaje = "Se eliminó correctamente la categoria!";
                }
            }
            catch(DbUpdateException ex)
            {
                var sqlException = ex.InnerException.InnerException;
                // Validar FK de categoria en producto
                if(typeof(SqlException) == sqlException.GetType())
                {
                    if ((sqlException as SqlException).Number == SqlExceptionNumbers.ForeignKey)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "No se puede eliminar una categoria en los cuales tiene productos registrados.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al eliminar la categoria.";
                response.EsCorrecto = false;
                // Log Exception ...
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<CategoriaDC>> Listar()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<CategoriaDC>>();
            response.EsCorrecto = true;
            response.Mensaje = "Datos encontrados.";

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    response.Data = (from categoria in db.Categoria
                                     select new CategoriaDC
                                     {
                                        Id = categoria.CategoriaID,
                                        Descripcion = categoria.Descripcion
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

        public ResponseWithDataDataContract<CategoriaDC> ObtenerPorId(int id)
        {
            var response = new ResponseWithDataDataContract<CategoriaDC>();
            response.EsCorrecto = true;
            response.Mensaje = "Dato encontrado.";

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var categoria = db.Categoria.Where(c => c.CategoriaID == id).FirstOrDefault();
                    if(categoria == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "No existe la categoria con ese id";
                        return response;
                    }

                    response.Data = new CategoriaDC
                    {
                        Id = categoria.CategoriaID,
                        Descripcion = categoria.Descripcion
                    };

                    response.Mensaje = "Se encontró la categoria";
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
