using Pymex.Services.Contracts;
using Pymex.Services.Mappers;
using Pymex.Services.Mappers.Contracts;
using Pymex.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pymex.Services
{
    public class ClienteService : IClienteService
    {

        private readonly IGenericMapper<Cliente, ClienteDC> _mapper = new ClienteMapper();

        public ResponseDataContract Actualizar(ClienteDC dataContract)
        {
            var response = new ResponseDataContract();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Cliente cliente = (from clienteEntity in db.Cliente
                                         where clienteEntity.ClienteID == dataContract.Id
                                         select clienteEntity).FirstOrDefault();

                    if (cliente == null)
                    {
                        response.Mensaje = "El cliente a actualizar no existe.";
                        return response;
                    }

                    var clientePorNumeroDocumento = db.Cliente.Where(c => c.NumeroDocumento == dataContract.NumeroDocumento && c.ClienteID != dataContract.Id).FirstOrDefault();
                    if (clientePorNumeroDocumento != null)
                    {
                        response.Mensaje = "Ya existe un cliente con ese número de documento.";
                        return response;

                    }

                    _mapper.ToEditEntity(cliente, dataContract);
                    db.SaveChanges();

                }

                response.Mensaje = "Se actualizó el cliente correctamente.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al actualizar el cliente.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseDataContract Crear(ClienteDC dataContract)
        {
            var response = new ResponseDataContract();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {

                    var clientePorNumeroDocumento = db.Cliente.Where(c => c.NumeroDocumento == dataContract.NumeroDocumento).FirstOrDefault();
                    if (clientePorNumeroDocumento != null)
                    {
                        response.Mensaje = "Ya existe un cliente con ese número de documento.";
                        return response;

                    }

                    var cliente = _mapper.ToCreateEntity(dataContract);
                    db.Cliente.Add(cliente);
                    db.SaveChanges();

                }

                response.Mensaje = "Se agregó el cliente correctamente.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al insertar el cliente.";
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
                }

                response.Mensaje = "Se eliminó correctamente el cliente!";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al eliminar el cliente.";
                // Log Exception ...
            }

            return response;
        }
            
        public ResponseWithDataDataContract<IEnumerable<ClienteDC>> Listar()
        {
            var response = new ResponseWithDataDataContract<IEnumerable<ClienteDC>>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    response.Data = (from cliente in db.Cliente
                                     select cliente).ToList()
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

        public ResponseWithDataDataContract<ClienteDC> ObtenerPorId(int id)
        {
            var response = new ResponseWithDataDataContract<ClienteDC>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Cliente cliente = db.Cliente.Where(p => p.ClienteID == id).FirstOrDefault();
                    if (cliente == null)
                    {
                        response.Mensaje = "No existe el registro.";
                        return response;
                    }
                    
                    // Obteniendo datos del cliente
                    response.Data = _mapper.ToDataContract(cliente);
                }

                response.Mensaje = "Dato encontrado.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrio un error al obtener el registro.";
                // Log Exception ...
            }

            return response;
        }
    }
}
