using Pymex.Services.Contracts;
using Pymex.Services.Models;
using System.Collections.Generic;

namespace Pymex.Services.Mappers.Contracts
{
    public interface IInventarioMapper 
        :   IDataContractMapper<usp_ListarEntradas_Result, EntradaDC>, 
            IDataContractMapper<usp_ListarSalidas_Result, SalidaDC>,
            IDataContractMapper<EntradaProducto, EntradaDetalleDC>,
            IDataContractMapper<SalidaProducto, SalidaDetalleDC>

    {
        /// <summary>
        ///     Convierte la lista de detalle de los productos de una entrada
        ///     a un XML compatible en formato de cadena.
        /// </summary>
        /// <param name="detalleProductos">La lista de detalle de productos a mapear.</param>
        /// <returns>La cadena en formato en XML.</returns>
        string ToEntradaProductsXML(IEnumerable<EntradaDetalleDC> detalleProductos);

        /// <summary>
        ///     Convierte la lista de detalle de los productos de una salida 
        ///     a un XML compatible en formato de cadena.
        /// </summary>
        /// <param name="detalleProductos">La lista de detalle de productos a mapear.</param>
        /// <returns>La cadena en formato en XML.</returns>
        string ToSalidaProductsXML(IEnumerable<SalidaDetalleDC> detalleProductos);

        /// <summary>
        ///     Convierte el resultado del procedure usp_BuscarEntradaPorCodigo a la
        ///     respectiva data contractual con sus productos incluidos.
        /// </summary>
        /// <param name="entrada">El resultado del procedure a utilizar el mapeo.</param>
        /// <param name="detalleProductos">Los productos a utilizar para el mapeo.</param>
        /// <returns>La data contractual de entrada.</returns>
        EntradaDC ToEntradaWithProductsDataContract(usp_BuscarEntradaPorCodigo_Result entrada, IEnumerable<EntradaDetalleDC> detalleProductos);

        /// <summary>
        ///     Convierte el resultado del procedure usp_BuscarSalidaPorCodigo a la
        ///     respectiva data contractual con sus productos incluidos.
        /// </summary>
        /// <param name="entrada">El resultado del procedure a utilizar el mapeo.</param>
        /// <param name="detalleProductos">Los productos a utilizar para el mapeo.</param>
        /// <returns>La data contractual de salida.</returns>
        SalidaDC ToSalidaWithProductsDataContract(usp_BuscarSalidaPorCodigo_Result salida, IEnumerable<SalidaDetalleDC> detalleProductos);
    }
}
