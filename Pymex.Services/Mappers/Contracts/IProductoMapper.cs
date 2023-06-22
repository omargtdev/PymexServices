using Pymex.Services.Contracts;
using Pymex.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pymex.Services.Mappers.Contracts
{
    public interface IProductoMapper : IGenericMapper<Producto, ProductoDC>
    {
        /// <summary>
        ///     Recibe el producto y lo mapea para una activación/desactivación.
        /// </summary>
        /// <param name="product">El valor de referencia del producto a actualizar.</param>
        /// <param name="dataContract">El objeto contractual con los valores a mapear.</param>
        void ToActivateProduct(Producto product, ProductoDC dataContract);
    }
}
