using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Pymex.Services.Contracts;
using Pymex.Services.Mappers;
using Pymex.Services.Mappers.Contracts;
using Pymex.Services.Models;
using Pymex.Services.Utils;
using Pymex.Services.ValueObjects;

namespace Pymex.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CategoriaService" in both code and config file together.
    public class CategoriaService : ICategoriaService
    {

        private readonly IGenericMapper<Categoria, CategoriaDC> _mapper = new CategoriaMapper();

        public ResponseDataContract Actualizar(CategoriaDC dataContract)
        {
            var response = new ResponseDataContract();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Categoria categoria = (from categoriaEntity in db.Categoria
                                           where categoriaEntity.CategoriaID == dataContract.Id
                                           select categoriaEntity).FirstOrDefault();

                    if (categoria == null)
                    {
                        response.Mensaje = "La categoría a actualizar no existe.";
                        return response;
                    }

                    _mapper.ToEditEntity(categoria, dataContract);
                    db.SaveChanges();
                }

                response.Mensaje = "Se actualizó correctamente la categoría.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al actualizar la categoría.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseDataContract Crear(CategoriaDC dataContract)
        {
            var response = new ResponseDataContract();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var categoria = _mapper.ToCreateEntity(dataContract);
                    db.Categoria.Add(categoria);
                    db.SaveChanges();
                }

                response.Mensaje = "Se creó correctamente la categoria!";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al insertar la categoría.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseDataContract Eliminar(int id)
        {
            var response = new ResponseDataContract();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Categoria categoria = (from categoriaEntity in db.Categoria
                                       where categoriaEntity.CategoriaID == id
                                       select categoriaEntity).FirstOrDefault();

                    if (categoria == null)
                    {
                        response.Mensaje = "La categoría a eliminar no existe.";
                        return response;
                    }

                    db.Categoria.Remove(categoria);
                    db.SaveChanges();
                }

                response.Mensaje = "Se eliminó correctamente la categoría!";
                response.EsCorrecto = true;
            }
            catch(DbUpdateException ex)
            {
                var sqlException = ex.InnerException.InnerException;
                // Validar FK de categoria en producto
                if(typeof(SqlException) == sqlException.GetType())
                {
                    if ((sqlException as SqlException).Number == SqlExceptionNumbers.ForeignKey)
                    {
                        response.Mensaje = "No se puede eliminar una categoría en el cual tiene productos registrados.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al eliminar la categoría.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<CategoriaDC>> Listar()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<CategoriaDC>>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    response.Data = (from categoria in db.Categoria
                                     select categoria).ToList()
                                     .Select(c => _mapper.ToDataContract(c));
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

        public ResponseWithDataDataContract<CategoriaDC> ObtenerPorId(int id)
        {
            var response = new ResponseWithDataDataContract<CategoriaDC>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var categoria = db.Categoria.Where(c => c.CategoriaID == id).FirstOrDefault();
                    if(categoria == null)
                    {
                        response.Mensaje = "No existe la categoría con ese id";
                        return response;
                    }

                    response.Data = _mapper.ToDataContract(categoria);
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
    }
}
