using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using Pymex.Services.Contracts;
using Pymex.Services.Mappers;
using Pymex.Services.Mappers.Contracts;
using Pymex.Services.Models;
using Pymex.Services.Utils;

namespace Pymex.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AlmacenService" in both code and config file together.
    public class AlmacenService : IAlmacenService
    {

        private readonly IGenericMapper<Almacen, AlmacenDC> _mapper = new AlmacenMapper();

        public ResponseDataContract Actualizar(AlmacenDC dataContract)
        {
            var response = new ResponseDataContract();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Almacen almacen = (from almacenEntity in db.Almacen
                                           where almacenEntity.AlmacenID == dataContract.Id
                                           select almacenEntity).FirstOrDefault();

                    if (almacen == null)
                    {
                        response.Mensaje = "El almacen a actualizar no existe.";
                        return response;
                    }

                    _mapper.ToEditEntity(almacen, dataContract);
                    db.SaveChanges();
                }

                response.Mensaje = "Se actualizó correctamente el almacen!";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al actualizar el almacen.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseDataContract Crear(AlmacenDC dataContract)
        {
            var response = new ResponseDataContract();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var almacen = _mapper.ToCreateEntity(dataContract);
                    db.Almacen.Add(almacen);
                    db.SaveChanges();
                }

                response.Mensaje = "Se creó correctamente el almacen!";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al insertar el almacen.";
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
                    Almacen almacen = (from almacenEntity in db.Almacen
                                           where almacenEntity.AlmacenID == id
                                           select almacenEntity).FirstOrDefault();

                    if (almacen == null)
                    {
                        response.Mensaje = "El almacen a eliminar no existe.";
                        return response;
                    }

                    db.Almacen.Remove(almacen);
                    db.SaveChanges();
                }

                response.EsCorrecto = true;
                response.Mensaje = "Se eliminó correctamente el almacen!";
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.InnerException.InnerException;
                if (typeof(SqlException) == sqlException.GetType())
                {
                    if ((sqlException as SqlException).Number == SqlExceptionNumbers.ForeignKey)
                    {
                        response.Mensaje = "No se puede eliminar un almacen en el cual tiene productos registrados.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al eliminar el almacen.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<AlmacenDC>> Listar()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<AlmacenDC>>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    response.Data = (from almacen in db.Almacen
                                     select almacen).ToList()
                                     .Select(a => _mapper.ToDataContract(a));
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

        public ResponseWithDataDataContract<AlmacenDC> ObtenerPorId(int id)
        {
            var response = new ResponseWithDataDataContract<AlmacenDC>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    var almacen = db.Almacen.Where(a => a.AlmacenID == id).FirstOrDefault();
                    if (almacen == null)
                    {
                        response.Mensaje = "No existe el almacen con ese id.";
                        return response;
                    }

                    response.Data = _mapper.ToDataContract(almacen);
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
