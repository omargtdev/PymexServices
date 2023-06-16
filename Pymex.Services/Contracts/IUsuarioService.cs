using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Pymex.Services.ValueObjects;

namespace Pymex.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUsuarioService" in both code and config file together.
    [ServiceContract]
    public interface IUsuarioService
    {
        /// <summary>
        ///     Metodo para autentificarse. Caso la operación sea correcta
        ///     devolverá el respectivo usuario con las credenciales dadas.
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="passwordEncriptado">Contraseña encriptada</param>
        /// <returns>El usuario de las credenciales.</returns>
        [OperationContract]
        ResponseWithDataDataContract<UsuarioDC> Login(string username, string passwordEncriptado);

        /// <summary>
        ///     Obtiene todos los usuarios del sistema. Solo la información es otorgada a usuarios de tipo
        ///     Administrador.
        /// </summary>
        /// <param name="usuarioSolicitante">El nombre de usuario que está solciitando</param>
        /// <returns>La lista de los usuarios.</returns>
        [OperationContract]
        ResponseWithDataDataContract<IEnumerable<UsuarioDC>> ObtenerUsuarios(string usuarioSolicitante);

        /// <summary>
        ///     Actualiza un usuario.
        /// </summary>
        /// <param name="usuario">El usuario con los datos a actualizar.</param>
        /// <returns>Una respuesta contractual</returns>
        [OperationContract]
        ResponseDataContract ActualizarUsuario(UsuarioDC usuario);
    }

    [DataContract]
    public class UsuarioDC
    {
        [DataMember(Order = 1)] public int Id { get; set; }
        [DataMember(Order = 2)] public string Login { get; set; }
        [DataMember(Order = 3)] public string Nombre { get; set; }
        [DataMember(Order = 4)] public string Apellidos { get; set; }
        [DataMember(Order = 5)] public Perfil Perfil { get; set; }
    }

}
