using Pymex.Services.Contracts.Operations;
using Pymex.Services.ValueObjects;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Pymex.Services.Contracts
{
    [ServiceContract]
    public interface IProductoService : 
        IGenericService<ProductoDC>,
        IListarPorCantidadOperation<ProductoDC>
    {
        /// <summary>
        ///     Obtiene el producto por su código. 
        /// </summary>
        /// <param name="codigo">El código por el cual buscar.</param>
        /// <returns>El producto encontrado.</returns>
        [OperationContract]
        ResponseWithDataDataContract<ProductoDC> ObtenerPorCodigo(string codigo);

        /// <summary>
        ///     Obtiene la lista de los productos que estén activos.
        /// </summary>
        /// <returns>Una lista con los productos activos.</returns>
        [OperationContract]
        ResponseWithDataDataContract<IEnumerable<ProductoDC>> ListarSoloActivos();

        /// <summary>
        ///     Activa/desactiva un producto por su codigo.
        /// </summary>
        /// <param name="producto">El producto espera que tenga 3 valores: el codigo, el usuario que ejecuta y el valor de Activo (true/false).</param>
        /// <returns>La respuesta de la operación.</returns>
        [OperationContract]
        ResponseDataContract ActivarPorCodigo(ProductoDC producto);

        /// <summary>
        ///     Lista los productos con un stock con una cantidad determinada. 
        /// </summary>
        /// <param name="expresion">Descripcion a considerar.</param>
        /// <param name="maxCantidad">Cantidad maxima a considerar.</param>
        /// <returns>La respuesta de la operación</returns>
        [OperationContract]
        ResponseWithDataDataContract<IEnumerable<ProductoDC>> ListarProductosConStockPorCantidad(string descripcion, int maxCantidad);
    }

    [DataContract]
    public class ProductoDC
    {
        [DataMember(Order = 1)] public int Id { get; set; }
        [DataMember(Order = 2)] public string Codigo { get; set; }
        [DataMember(Order = 3)] public string Descripcion { get; set; }
        [DataMember(Order = 4)] public CategoriaDC Categoria { get; set; }
        [DataMember(Order = 5)] public AlmacenDC Almacen { get; set; }
        [DataMember(Order = 6)] public bool Activo { get; set; }
        [DataMember(Order = 7)] public decimal UltimoPrecioCompra { get; set; }
        [DataMember(Order = 8)] public decimal UltimoPrecioVenta { get; set; }
        [DataMember(Order = 9)] public int Stock { get; set; }
        [DataMember(Order = 10)] public HistorialSeguimientoDC HistorialSeguimiento { get; set; }
        [DataMember(Order = 11)] public string UsuarioAccion { get; set; }
    }

}
