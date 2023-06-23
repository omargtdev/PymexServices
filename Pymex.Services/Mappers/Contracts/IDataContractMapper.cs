using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pymex.Services.Mappers.Contracts
{
    public interface IDataContractMapper<E, DC> where E : class where DC : class
    {
        /// <summary>
        ///     Recibe la entidad y lo mapea para crear una data contractual.
        /// </summary>
        /// <param name="entity">La entidad con los valores a mapear.</param>
        /// <returns>La data contractual mapeada.</returns>
        DC ToDataContract(E entity);
    }
}
