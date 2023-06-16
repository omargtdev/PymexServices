using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Pymex.Services.Contracts;
using Pymex.Services.Exceptions;
using Pymex.Services.Models;
using Pymex.Services.ValueObjects;

namespace Pymex.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UsuarioService" in both code and config file together.
    public class UsuarioService : IUsuarioService
    {
        public ResponseDataContract ActualizarUsuario(UsuarioDC usuario)
        {
            throw new NotImplementedException();
        }

        public ResponseWithDataDataContract<UsuarioDC> Login(string username, string passwordEncriptado)
        {

            var response = new ResponseWithDataDataContract<UsuarioDC>();
            response.EsCorrecto = true;

            try
            {
                using (PymexEntities db = new PymexEntities())
                {
                    Usuario usuario = db.Usuario.Where(u => u.UsuarioLogin == username && passwordEncriptado == u.Clave).FirstOrDefault();
                    if (usuario != null)
                    {
                        response.Mensaje = "Autentificación correcta!";
                        // Obteniendo datos del usuario
                        response.Data = new UsuarioDC
                        {
                            Id = usuario.UsuarioID,
                            Login = usuario.UsuarioLogin,
                            Nombre = usuario.Nombre,
                            Apellidos = usuario.Apellidos,
                            Perfil  = (ValueObjects.Perfil) usuario.PerfilID
                        };
                    }
                    else
                    {
                        response.EsCorrecto = false;
                        response.Mensaje = "Credenciales incorrectas.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = "Ups!. Ocurrio un error al tratar de autenticarse.";
                // Log Exception ...
            }

            return response;
        }

        public ResponseWithDataDataContract<IEnumerable<UsuarioDC>> ObtenerUsuarios(string usuarioSolicitante)
        {
            var response = new ResponseWithDataDataContract<IEnumerable<UsuarioDC>>();
            response.EsCorrecto = false;

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
                                    select new UsuarioDC
                                    {
                                        Id = usuarioEntity.UsuarioID,
                                        Login = usuario.UsuarioLogin,
                                        Nombre = usuario.Nombre,
                                        Apellidos = usuario.Apellidos,
                                        Perfil = (ValueObjects.Perfil)usuario.PerfilID
                                    }).ToList();
                    response.EsCorrecto = true;

                }
               
            }
            catch (InsufficientPermissionsException ex)
            {
                response.Mensaje = $"Usuario {ex.Username}: {ex.Message}";
            }
            catch (Exception ex)
            {

                response.Mensaje = "Ups!. Ocurrio un error al obtener los registros.";
                // Log Exception ...
            }

            return response;
        }
    }
}
