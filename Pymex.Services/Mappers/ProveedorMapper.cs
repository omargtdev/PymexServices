using Pymex.Services.Contracts;
using Pymex.Services.Mappers.Contracts;
using Pymex.Services.Models;
using Pymex.Services.ValueObjects;
using System;

namespace Pymex.Services.Mappers
{
    public class ProveedorMapper : IGenericMapper<Proveedor, ProveedorDC>
    {
        public Proveedor ToCreateEntity(ProveedorDC dataContract)
        {
            return new Proveedor
            {
                ProveedorID = 0,
                TipoDocumento = (byte)dataContract.TipoDocumento,
                NumeroDocumento = dataContract.NumeroDocumento,
                NombreCompleto = dataContract.NombreCompleto,
                UsuarioRegistro = dataContract.UsuarioAccion,
                FechaRegistro = DateTime.Now
            };
        }

        public ProveedorDC ToDataContract(Proveedor entity)
        {
            return new ProveedorDC
            {
                Id = entity.ProveedorID,
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

        public void ToEditEntity(Proveedor entity, ProveedorDC dataContract)
        {
            entity.TipoDocumento = (byte)dataContract.TipoDocumento;
            entity.NumeroDocumento = dataContract.NumeroDocumento;
            entity.NombreCompleto = dataContract.NombreCompleto;
            entity.UltimoUsuarioModifico = dataContract.UsuarioAccion;
            entity.FechaModificacion = DateTime.Now;
        }
    }
}
