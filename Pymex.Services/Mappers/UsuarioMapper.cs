using Pymex.Services.Contracts;
using Pymex.Services.Mappers.Contracts;
using Pymex.Services.Models;
using System;

namespace Pymex.Services.Mappers
{
    public class UsuarioMapper : IGenericMapper<Usuario, UsuarioDC>
    {
        public Usuario ToCreateEntity(UsuarioDC dataContract)
        {
            throw new NotImplementedException();
        }

        public UsuarioDC ToDataContract(Usuario entity)
        {
            return new UsuarioDC
            {
                Id = entity.UsuarioID,
                Login = entity.UsuarioLogin,
                Nombre = entity.Nombre,
                Apellidos = entity.Apellidos,
                Perfil = (ValueObjects.Perfil)entity.PerfilID
            };
        }

        public void ToEditEntity(Usuario entity, UsuarioDC dataContract)
        {
            entity.Nombre = dataContract.Nombre;
            entity.Apellidos = dataContract.Apellidos;
            entity.PerfilID = (short)dataContract.Perfil;
        }
    }
}
