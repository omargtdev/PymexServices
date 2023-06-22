﻿using Pymex.Services.ValueObjects;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Pymex.Services.Contracts
{
    [ServiceContract]
    public interface IProductoService : IGenericService<ProductoDC>
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
    }

    [DataContract]
    public class ProductoDC
    {
        [DataMember(Order = 1)] public int Id { get; set; }
        [DataMember(Order = 2)] public string Codigo { get; set; }
        [DataMember(Order = 3)] public string Descripcion { get; set; }
        [DataMember(Order = 4)] public short CategoriaId { get; set; }
        [DataMember(Order = 5)] public short AlmacenId { get; set; }
        [DataMember(Order = 6)] public bool Activo { get; set; }
        [DataMember(Order = 7)] public decimal UltimoPrecioCompra { get; set; }
        [DataMember(Order = 8)] public decimal UltimoPrecioVenta { get; set; }
        [DataMember(Order = 9)] public int Stock { get; set; }
        [DataMember(Order = 10)] public HistorialSeguimientoDC HistorialSeguimiento { get; set; }
        [DataMember(Order = 11)] public string UsuarioAccion { get; set; }
    }

}
