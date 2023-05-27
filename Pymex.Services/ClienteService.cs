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
    public class ClienteService : IClienteService
    {
        public ResponseDataContract Actualizar(ClienteDC entity)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Cliente cliente = (from clienteEntity in db.Cliente
                                         where clienteEntity.ClienteID == entity.Id
                                         select clienteEntity).FirstOrDefault();

                    if (cliente == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "El cliente a actualizar no existe.";
                        return response;
                    }

                    var clientePorNumeroDocumento = db.Cliente.Where(c => c.NumeroDocumento == entity.NumeroDocumento && c.ClienteID != entity.Id).FirstOrDefault();
                    if (clientePorNumeroDocumento != null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "Ya existe un cliente con ese número de documento";
                        return response;

                    }

                    cliente.TipoDocumento = (byte)entity.TipoDocumento;
                    cliente.NumeroDocumento = entity.NumeroDocumento;
                    cliente.NombreCompleto = entity.NombreCompleto;
                    cliente.UltimoUsuarioModifico = entity.UsuarioAccion;
                    cliente.FechaModificacion = DateTime.Now;

                    db.SaveChanges();

                    response.Mensaje = "Se actualizó el cliente correctamente";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al actualizar el cliente.";
                response.EsCorrecto = false;
                // Log Exception ...
            }

            return response;
        }

        public ResponseDataContract Crear(ClienteDC entity)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {

                    var clientePorNumeroDocumento = db.Cliente.Where(c => c.NumeroDocumento == entity.NumeroDocumento).FirstOrDefault();
                    if (clientePorNumeroDocumento != null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "Ya existe un cliente con ese número de documento";
                        return response;

                    }

                    Cliente cliente = new Cliente();
                    cliente.TipoDocumento = (byte)entity.TipoDocumento;
                    cliente.NumeroDocumento = entity.NumeroDocumento;
                    cliente.NombreCompleto = entity.NombreCompleto;
                    cliente.UsuarioRegistro = entity.UsuarioAccion;
                    cliente.FechaRegistro = DateTime.Now;

                    db.Cliente.Add(cliente);
                    db.SaveChanges();

                    response.Mensaje = "Se agregó el cliente correctamente";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al insertar el cliente.";
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
                    Cliente cliente = (from clienteEntity in db.Cliente
                                         where clienteEntity.ClienteID == id
                                         select clienteEntity).FirstOrDefault();

                    if (cliente == null)
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "El cliente a eliminar no existe.";
                        return response;
                    }

                    db.Cliente.Remove(cliente);
                    db.SaveChanges();

                    response.Mensaje = "Se eliminó correctamente el cliente!";
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

        public ResponseWithDataDataContract<IEnumerable<ClienteDC>> Listar()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<ClienteDC>>();
            response.EsCorrecto = true;
            response.Mensaje = "Datos encontrados.";

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    response.Data = (from cliente in db.Cliente
                                     select new ClienteDC
                                     {
                                         Id = cliente.ClienteID,
                                         TipoDocumento = (TipoDocumento)cliente.TipoDocumento,
                                         NumeroDocumento = cliente.NumeroDocumento,
                                         NombreCompleto = cliente.NombreCompleto,
                                         FechaRegistro = cliente.FechaRegistro
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

        public ResponseWithDataDataContract<ClienteDC> ObtenerPorId(int id)
        {
            var response = new ResponseWithDataDataContract<ClienteDC>();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Cliente cliente = db.Cliente.Where(p => p.ClienteID == id).FirstOrDefault();
                    if (cliente != null)
                    {
                        response.Mensaje = "Dato encontrado.";
                        // Obteniendo datos del cliente
                        response.Data = new ClienteDC
                        {
                            Id = cliente.ClienteID,
                            TipoDocumento = (TipoDocumento)cliente.TipoDocumento,
                            NumeroDocumento = cliente.NumeroDocumento,
                            NombreCompleto = cliente.NombreCompleto,
                            FechaRegistro = cliente.FechaRegistro
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
