using System.Collections.Generic;
using System.ServiceModel;

namespace Pymex.Services.Contracts.Operations
{
    [ServiceContract]
    public interface IListarPorCantidadOperation<T> where T : class 
    {
        /// <summary>
        ///     Lista todas las entidades encontradas por la expresion definida a buscar con
        ///     la cantidad establecida.
        /// </summary>
        /// <param name="expresion">La cadena de caracteres a filtrar.</param>
        /// <param name="maxCantidad">La cantidad a traer.</param>
        /// <returns>Una respuesta con la lista de entidades.</returns>
        [OperationContract]
        ResponseWithDataDataContract<IEnumerable<T>> ListarPorExpresionYCantidad(string expresion, int maxCantidad);
    }
}
