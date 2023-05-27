using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pymex.Services.Contracts
{
    [ServiceContract]
    public interface IGenericService<T> where T : class
    {
        [OperationContract]
        ResponseWithDataDataContract<IEnumerable<T>> Listar();

        [OperationContract]
        ResponseWithDataDataContract<T> ObtenerPorId(int id);

        [OperationContract]
        ResponseDataContract Crear(T entity);

        [OperationContract]
        ResponseDataContract Actualizar(T entity);

        [OperationContract]
        ResponseDataContract Eliminar(int id);
    }

    [DataContract]
    public class ResponseWithDataDataContract<T> : ResponseDataContract where T : class
    {
        [DataMember]
        public T Data { get; set; }
    }

    [DataContract]
    public class ResponseDataContract
    {
        [DataMember]
        public string Mensaje { get; set; }

        [DataMember]
        public bool EsCorrecto { get; set; }
    }
}
