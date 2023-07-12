using System;
using System.Collections.Generic;
using System.Linq;
using Pymex.Services.Contracts;
using Pymex.Services.Exceptions;
using Pymex.Services.Mappers;
using Pymex.Services.Mappers.Contracts;
using Pymex.Services.Models;

namespace Pymex.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UsuarioService" in both code and config file together.
    public class UsuarioService : IUsuarioService
    {

        private readonly IGenericMapper<Usuario, UsuarioDC> _mapper = new UsuarioMapper();

        public ResponseDataContract ActualizarUsuario(UsuarioDC usuarioDC)
        {
            var response = new ResponseDataContract();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Usuario usuario = (from usuarioEntity in db.Usuario
                                           where usuarioEntity.UsuarioLogin == usuarioDC.Login
                                           select usuarioEntity).FirstOrDefault();

                    if (usuario == null)
                    {
                        response.Mensaje = "El usuario a actualizar no existe.";
                        return response;
                    }

                    _mapper.ToEditEntity(usuario, usuarioDC);
                    db.SaveChanges();
                }
                
                response.Mensaje = "Se actualizó el usuario correctamente.";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al actualizar el usuario.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseWithDataDataContract<UsuarioDC> Login(string username, string passwordEncriptado)
        {

            var response = new ResponseWithDataDataContract<UsuarioDC>();

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Usuario usuario = db.Usuario.Where(u => u.UsuarioLogin == username && passwordEncriptado == u.Clave).FirstOrDefault();
                    if (usuario == null)
                    {
                        response.Mensaje = "Credenciales incorrectas.";
                        return response;
                    }

                    // Obteniendo datos del usuario
                    response.Data = _mapper.ToDataContract(usuario);
                }

                response.Mensaje = "Autentificación correcta!";
                response.EsCorrecto = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups! Ocurrió un error al tratar de autenticarse.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<UsuarioDC>> ObtenerUsuarios(string usuarioSolicitante)
        {
            var response = new ResponseWithDataDataContract<IEnumerable<UsuarioDC>>();

            try
            {
                using(PymexEntities db = new PymexEntities())
                {
                    // Buscamos el usuario
                    Usuario usuario = db.Usuario.Where(u => u.UsuarioLogin == usuarioSolicitante).FirstOrDefault();
                    if(usuario == null)
                    {
                        response.Mensaje = "El usuario solicitante no existe.";
                        return response;
                    }

                    // Validamos que sea administrador
                    if ((ValueObjects.Perfil)usuario.PerfilID != ValueObjects.Perfil.Administrador)
                        throw new InsufficientPermissionsException("Este usuario no cuenta con permisos de administrador.", usuarioSolicitante);

                    // Obteniendo los usuarios
                    response.Data = (from usuarioEntity in db.Usuario
                                     select usuarioEntity).ToList()
                                    .Select(u => _mapper.ToDataContract(u));
                }

                response.Mensaje = "Datos encontrados.";
                response.EsCorrecto = true;
               
            }
            catch (InsufficientPermissionsException ex)
            {
                response.Mensaje = $"Usuario {ex.Username}: {ex.Message}.";
            }
            catch (Exception ex)
            {

                response.Mensaje = "Ups! Ocurrió un error al obtener los registros.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseDataContract CambiarPerfilUsuario(UsuarioDC dataContract, string usuarioSolicitante)
        {
            var response = new ResponseDataContract();

            try
            {
                using(PymexEntities db = new PymexEntities())
                {
                    // Buscamos el usuario que solicita
                    Usuario usuario = db.Usuario.Where(u => u.UsuarioLogin == usuarioSolicitante).FirstOrDefault();
                    if(usuario == null)
                    {
                        response.Mensaje = "El usuario solicitante no existe.";
                        return response;
                    }

                    // Validamos que sea administrador
                    if ((ValueObjects.Perfil)usuario.PerfilID != ValueObjects.Perfil.Administrador)
                        throw new InsufficientPermissionsException("Este usuario no cuenta con permisos de administrador.", usuarioSolicitante);

                    // Buscamos y actualizamos el usuario
                    Usuario usuarioToUpdate = db.Usuario.Where(u => u.UsuarioLogin == dataContract.Login).FirstOrDefault();
                    if(usuarioToUpdate == null)
                    {
                        response.Mensaje = "El usuario a actualizar no existe.";
                        return response;
                    }

                    usuarioToUpdate.PerfilID = (short)dataContract.Perfil;
                    db.SaveChanges();
                }

                response.Mensaje = "Se cambió el usuario correctamente.";
                response.EsCorrecto = true;
               
            }
            catch (InsufficientPermissionsException ex)
            {
                response.Mensaje = $"Usuario {ex.Username}: {ex.Message}.";
            }
            catch (Exception ex)
            {

                response.Mensaje = "Ups! Ocurrió un error al obtener los registros.";
                // Log Exception ...
            }

            return response;
        }
    }
}
