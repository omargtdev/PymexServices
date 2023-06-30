using Pymex.Services.Contracts;
using Pymex.Services.Mappers;
using Pymex.Services.Mappers.Contracts;
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

        private readonly IGenericMapper<Proveedor, ProveedorDC> _mapper = new ProveedorMapper();

        public ResponseDataContract Actualizar(ProveedorDC dataContract)
        {
            var response = new ResponseDataContract();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Proveedor proveedor = (from proveedorEntity in db.Proveedor
                                       where proveedorEntity.ProveedorID == dataContract.Id
                                       select proveedorEntity).FirstOrDefault();

                    if (proveedor == null)
                    {
                        response.Mensaje = "El proveedor a actualizar no existe.";
                        return response;
                    }

                    var proveedorPorNumeroDocumento = db.Proveedor.Where(p => p.NumeroDocumento == dataContract.NumeroDocumento && p.ProveedorID != dataContract.Id).FirstOrDefault();
                    if (proveedorPorNumeroDocumento != null)
                    {
                        response.Mensaje = "Ya existe un proveedor con ese número de documento.";
                        return response;
                    }

                    _mapper.ToEditEntity(proveedor, dataContract);
                    db.SaveChanges();
                }

                response.Mensaje = "Se actualizó el proveedor correctamente.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al actualizar el proveedor.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseDataContract Crear(ProveedorDC dataContract)
        {
            var response = new ResponseDataContract();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {

                    var proveedorPorNumeroDocumento = db.Proveedor.Where(p => p.NumeroDocumento == dataContract.NumeroDocumento).FirstOrDefault();
                    if (proveedorPorNumeroDocumento != null)
                    {
                        response.Mensaje = "Ya existe un proveedor con ese número de documento.";
                        return response;

                    }

                    var proveedor = _mapper.ToCreateEntity(dataContract);
                    db.Proveedor.Add(proveedor);
                    db.SaveChanges();
                }

                response.Mensaje = "Se agregó el proveedor correctamente.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al insertar el proveedor.";
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
                    Proveedor proveedor  = (from proveedorEntity in db.Proveedor
                                       where proveedorEntity.ProveedorID == id
                                       select proveedorEntity).FirstOrDefault();

                    if (proveedor == null)
                    {
                        response.Mensaje = "El proveedor a eliminar no existe.";
                        return response;
                    }

                    db.Proveedor.Remove(proveedor);
                    db.SaveChanges();
                }

                response.Mensaje = "Se eliminó correctamente el proveedor!";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al insertar el proveedor.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<ProveedorDC>> Listar()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<ProveedorDC>>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    response.Data = (from proveedor in db.Proveedor
                                     select proveedor).ToList()
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

        public ResponseWithDataDataContract<ProveedorDC> ObtenerPorId(int id)
        {
            var response = new ResponseWithDataDataContract<ProveedorDC>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Proveedor proveedor = db.Proveedor.Where(p => p.ProveedorID == id).FirstOrDefault();
                    if (proveedor == null)
                    {
                        response.Mensaje = "No existe el proveedor.";
                        return response;
                    }

                    // Obteniendo datos del proveedor
                    response.Data = _mapper.ToDataContract(proveedor);
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
