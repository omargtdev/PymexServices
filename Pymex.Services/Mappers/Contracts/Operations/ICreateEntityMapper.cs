using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pymex.Services.Mappers.Contracts.Operations
{
    public interface ICreateEntityMapper<E, DC> where E : class where DC : class
    {
        /// <summary>
        ///     Recibe la data contractual y lo mapea para crear una entidad.
        /// </summary>
        /// <param name="dataContract">El objeto contractual con los valores a mapear.</param>
        /// <returns>La entidad mapeada.</returns>
        E ToCreateEntity(DC dataContract);
    }
}
