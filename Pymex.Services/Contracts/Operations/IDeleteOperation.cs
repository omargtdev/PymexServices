using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pymex.Services.Contracts.Operations
{
    /// <summary>
    ///     Contrato que contiene la operación de eliminar la entidad dada.
    /// </summary>
    /// <typeparam name="T">Entidad a recibir para eliminar</typeparam>
    [ServiceContract]
    public interface IDeleteOperation<T> where T : class
    {
        /// <summary>
        ///     Elimina una entidad
        /// </summary>
        /// <param name="id">El id de la entidad a eliminar</param>
        /// <returns></returns>
        [OperationContract]
        ResponseDataContract Eliminar(int id);
    }
}
