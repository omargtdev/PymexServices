using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pymex.Services.Mappers.Contracts.Operations
{
    public interface IEditEntityMapper<E, DC> where E : class where DC : class
    {
        /// <summary>
        ///     Recibe la entidad y lo mapea para una actualización.
        /// </summary>
        /// <param name="entity">El valor de referencia de la entidad a actualizar.</param>
        /// <param name="dataContract">El objeto contractual con los valores a mapear.</param>
        void ToEditEntity(E entity, DC dataContract);
    }
}
