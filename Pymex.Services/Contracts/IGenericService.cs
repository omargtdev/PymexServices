using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Pymex.Services.Contracts
{
    /// <summary>
    ///     Contrato genérico con las operaciones de CRUD a excepción de eliminar.
    /// </summary>
    /// <typeparam name="T">El tipo a implementar.</typeparam>
    [ServiceContract]
    public interface IGenericService<T> where T : class
    {
        /// <summary>
        ///     Lista todas las entidades definidas en el contrato.
        /// </summary>
        /// <returns>Una respuesta con la lista de entidades.</returns>
        [OperationContract]
        ResponseWithDataDataContract<IEnumerable<T>> Listar();

        /// <summary>
        ///     Obtiene la entidad por su id.
        /// </summary>
        /// <param name="id">El id a buscar.</param>
        /// <returns>Una respuesta con la entidad encontrada.</returns>
        [OperationContract]
        ResponseWithDataDataContract<T> ObtenerPorId(int id);

        /// <summary>
        ///     Crea una entidad a través de la data contractual. 
        /// </summary>
        /// <param name="dataContract">La data contractual que representa a la entidad.</param>
        /// <returns>Una respuesta de la operación.</returns>
        [OperationContract]
        ResponseDataContract Crear(T dataContract);

        /// <summary>
        ///     Actualiza una entidad a través de la data contractual.
        /// </summary>
        /// <param name="dataContract">La data contractual que representa a la entidad.</param>
        /// <returns>Una respuesta de la operación.</returns>
        [OperationContract]
        ResponseDataContract Actualizar(T dataContract);

    }

    [DataContract]
    public class ResponseWithDataDataContract<T> : ResponseDataContract where T : class
    {
        [DataMember(Order = 3)]
        public T Data { get; set; }
    }

    [DataContract]
    public class ResponseDataContract
    {

        private bool _esCorrecto = false;

        [DataMember(Order = 1)]
        public bool EsCorrecto { get => _esCorrecto; set => _esCorrecto = value; }

        [DataMember(Order = 2)]
        public string Mensaje { get; set; }
    }
}
