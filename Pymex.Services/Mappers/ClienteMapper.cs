using Pymex.Services.Contracts;
using Pymex.Services.Mappers.Contracts;
using Pymex.Services.Models;
using Pymex.Services.ValueObjects;
using System;

namespace Pymex.Services.Mappers
{
    public class ClienteMapper : IGenericMapper<Cliente, ClienteDC>
    {
        public Cliente ToCreateEntity(ClienteDC dataContract)
        {
            return new Cliente
            {
                ClienteID = 0,
                TipoDocumento = (byte)dataContract.TipoDocumento,
                NumeroDocumento = dataContract.NumeroDocumento,
                NombreCompleto = dataContract.NombreCompleto,
                UsuarioRegistro = dataContract.UsuarioAccion,
                FechaRegistro = DateTime.Now
            };
        }

        public ClienteDC ToDataContract(Cliente entity)
        {
            return new ClienteDC
            {
                 Id = entity.ClienteID,
                 TipoDocumento = (TipoDocumento)entity.TipoDocumento,
                 NumeroDocumento = entity.NumeroDocumento,
                 NombreCompleto = entity.NombreCompleto,
                 HistorialSeguimiento = new HistorialSeguimientoDC
                 {
                     FechaRegistro = entity.FechaRegistro,
                     UsuarioRegistro = entity.UsuarioRegistro,
                     FechaModificacion = entity.FechaModificacion,
                     UltimoUsuarioModificacion = entity.UltimoUsuarioModifico
                 }
            };
        }

        public void ToEditEntity(Cliente entity, ClienteDC dataContract)
        {
            entity.TipoDocumento = (byte)dataContract.TipoDocumento;
            entity.NumeroDocumento = dataContract.NumeroDocumento;
            entity.NombreCompleto = dataContract.NombreCompleto;
            entity.UltimoUsuarioModifico = dataContract.UsuarioAccion;
            entity.FechaModificacion = DateTime.Now;
        }
    }
}
