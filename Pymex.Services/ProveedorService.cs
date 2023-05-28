using Pymex.Services.Contracts;
using Pymex.Services.Models;
using Pymex.Services.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Pymex.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProveedorService" in both code and config file together.
    public class ProveedorService : IProveedorService
    {
        public ResponseDataContract Actualizar(ProveedorDC entity)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Proveedor proveedor = (from proveedorEntity in db.Proveedor
                                       where proveedorEntity.ProveedorID == entity.Id
                                       select proveedorEntity).FirstOrDefault();

                    if (proveedor == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "El proveedor a actualizar no existe.";
                        return response;
                    }

                    var proveedorPorNumeroDocumento = db.Proveedor.Where(p => p.NumeroDocumento == entity.NumeroDocumento && p.ProveedorID != entity.Id).FirstOrDefault();
                    if (proveedorPorNumeroDocumento != null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "Ya existe un proveedor con ese número de documento";
                        return response;
                    }

                    proveedor.TipoDocumento = (byte)entity.TipoDocumento;
                    proveedor.NumeroDocumento = entity.NumeroDocumento;
                    proveedor.NombreCompleto = entity.NombreCompleto;
                    proveedor.UltimoUsuarioModifico = entity.UsuarioAccion;
                    proveedor.FechaModificacion = DateTime.Now;

                    db.SaveChanges();

                    response.Mensaje = "Se actualizó el proveedor correctamente";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al actualizar el proveedor.";
                response.EsCorrecto = false;
                // Log Exception ...
            }

            return response;
        }

        public ResponseDataContract Crear(ProveedorDC entity)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {

                    var proveedorPorNumeroDocumento = db.Proveedor.Where(p => p.NumeroDocumento == entity.NumeroDocumento).FirstOrDefault();
                    if (proveedorPorNumeroDocumento != null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "Ya existe un proveedor con ese número de documento";
                        return response;

                    }

                    Proveedor proveedor = new Proveedor();
                    proveedor.TipoDocumento = (byte)entity.TipoDocumento;
                    proveedor.NumeroDocumento = entity.NumeroDocumento;
                    proveedor.NombreCompleto = entity.NombreCompleto;
                    proveedor.UsuarioRegistro = entity.UsuarioAccion;
                    proveedor.FechaRegistro = DateTime.Now;

                    db.Proveedor.Add(proveedor);
                    db.SaveChanges();

                    response.Mensaje = "Se agregó el proveedor correctamente";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al insertar el proveedor.";
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
                    Proveedor proveedor  = (from proveedorEntity in db.Proveedor
                                       where proveedorEntity.ProveedorID == id
                                       select proveedorEntity).FirstOrDefault();

                    if (proveedor == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "El proveedor a eliminar no existe.";
                        return response;
                    }

                    db.Proveedor.Remove(proveedor);
                    db.SaveChanges();

                    response.Mensaje = "Se eliminó correctamente el proveedor!";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al insertar el proveedor.";
                response.EsCorrecto = false;
                // Log Exception ...
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<ProveedorDC>> Listar()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<ProveedorDC>>();
            response.EsCorrecto = true;
            response.Mensaje = "Datos encontrados.";

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    response.Data = (from proveedor in db.Proveedor
                                     select new ProveedorDC
                                     {
                                         Id = proveedor.ProveedorID,
                                         TipoDocumento = (TipoDocumento)proveedor.TipoDocumento,
                                         NumeroDocumento = proveedor.NumeroDocumento,
                                         NombreCompleto = proveedor.NombreCompleto,
                                         FechaRegistro = proveedor.FechaRegistro
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

        public ResponseWithDataDataContract<ProveedorDC> ObtenerPorId(int id)
        {
            var response = new ResponseWithDataDataContract<ProveedorDC>();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Proveedor proveedor = db.Proveedor.Where(p => p.ProveedorID == id).FirstOrDefault();
                    if (proveedor != null)
                    {
                        response.Mensaje = "Dato encontrado.";
                        // Obteniendo datos del proveedor
                        response.Data = new ProveedorDC
                        {
                            Id = proveedor.ProveedorID,
                            TipoDocumento = (TipoDocumento)proveedor.TipoDocumento,
                            NumeroDocumento = proveedor.NumeroDocumento,
                            NombreCompleto = proveedor.NombreCompleto,
                            FechaRegistro = proveedor.FechaRegistro
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
