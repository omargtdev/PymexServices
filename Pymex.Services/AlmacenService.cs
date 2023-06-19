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

namespace Pymex.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AlmacenService" in both code and config file together.
    public class AlmacenService : IAlmacenService
    {
        public ResponseDataContract Actualizar(AlmacenDC entity)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Almacen almacen = (from almacenEntity in db.Almacen
                                           where almacenEntity.AlmacenID == entity.Id
                                           select almacenEntity).FirstOrDefault();

                    if (almacen == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "El almacen a actualizar no existe.";
                        return response;
                    }

                    almacen.Descripcion = entity.Descripcion;
                    almacen.Direccion = entity.Direccion;
                    almacen.Telefono = entity.Telefono;
                    almacen.Aforo = entity.Aforo;

                    db.SaveChanges();
                    response.Mensaje = "Se actualizó correctamente el almacen!";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al actualizar el almacen.";
                response.EsCorrecto = false;
                // Log Exception ...
            }

            return response;
        }

        public ResponseDataContract Crear(AlmacenDC entity)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Almacen almacen = new Almacen()
                    {
                        Descripcion = entity.Descripcion,
                        Direccion = entity.Direccion,
                        Telefono = entity.Telefono,
                        Aforo = entity.Aforo
                    };

                    db.Almacen.Add(almacen);
                    db.SaveChanges();
                    response.Mensaje = "Se creó correctamente el almacen!";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al insertar el almacen.";
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
                    Almacen almacen = (from almacenEntity in db.Almacen
                                           where almacenEntity.AlmacenID == id
                                           select almacenEntity).FirstOrDefault();

                    if (almacen == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "El almacen a eliminar no existe.";
                        return response;
                    }

                    db.Almacen.Remove(almacen);
                    db.SaveChanges();

                    response.Mensaje = "Se eliminó correctamente el almacen!";
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.InnerException.InnerException;
                if (typeof(SqlException) == sqlException.GetType())
                {
                    if ((sqlException as SqlException).Number == SqlExceptionNumbers.ForeignKey)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "No se puede eliminar un almacen en los cuales tiene productos registrados.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al eliminar el almacen.";
                response.EsCorrecto = false;
                // Log Exception ...
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<AlmacenDC>> Listar()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<AlmacenDC>>();
            response.EsCorrecto = true;
            response.Mensaje = "Datos encontrados.";

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    response.Data = (from almacen in db.Almacen
                                     select new AlmacenDC
                                     {
                                         Id = almacen.AlmacenID,
                                         Descripcion = almacen.Descripcion,
                                         Direccion = almacen.Direccion,
                                         Telefono = almacen.Telefono,
                                         Aforo = almacen.Aforo.HasValue ? almacen.Aforo.Value : 0
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

        public ResponseWithDataDataContract<AlmacenDC> ObtenerPorId(int id)
        {
            var response = new ResponseWithDataDataContract<AlmacenDC>();
            response.EsCorrecto = true;
            response.Mensaje = "Dato encontrado.";

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var almacen = db.Almacen.Where(a => a.AlmacenID == id).FirstOrDefault();
                    if (almacen == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "No existe el almacen con ese id";
                        return response;
                    }

                    response.Data = new AlmacenDC
                    {
                        Id = almacen.AlmacenID,
                        Descripcion = almacen.Descripcion,
                        Direccion = almacen.Direccion,
                        Telefono = almacen.Telefono,
                        Aforo = almacen.Aforo.HasValue ? almacen.Aforo.Value : 0
                    };

                    response.Mensaje = "Se encontró el almacen";
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
