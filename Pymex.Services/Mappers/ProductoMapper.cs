using Pymex.Services.Contracts;
using Pymex.Services.Mappers.Contracts;
using Pymex.Services.Models;
using Pymex.Services.ValueObjects;
using System;

namespace Pymex.Services.Mappers
{
    public class ProductoMapper : IProductoMapper
    {
        public ProductoDC ToDataContract(Producto entity)
        {
            return new ProductoDC
            {
                Id = entity.ProductoID,
                Codigo = entity.Codigo,
                Descripcion = entity.Descripcion,
                CategoriaId = entity.CategoriaID,
                AlmacenId = entity.AlmacenID,
                Activo = entity.Activo,
                UltimoPrecioCompra = entity.UltimoPrecioCompra.HasValue ? (decimal)entity.UltimoPrecioCompra : 0,
                UltimoPrecioVenta = entity.UltimoPrecioVenta.HasValue ? (decimal)entity.UltimoPrecioVenta : 0,
                Stock = entity.Stock.HasValue ? (int)entity.Stock : 0,
                HistorialSeguimiento = new HistorialSeguimientoDC
                {
                     FechaRegistro = entity.FechaRegistro,
                     UsuarioRegistro = entity.UsuarioRegistro,
                     FechaModificacion = entity.FechaModificacion,
                     UltimoUsuarioModificacion = entity.UltimoUsuarioModifico
                }
            };
        }

        public Producto ToCreateEntity(ProductoDC dataContract)
        {
            return new Producto
            {
                ProductoID = 0,
                Codigo = dataContract.Codigo,
                Descripcion = dataContract.Descripcion,
                CategoriaID = dataContract.CategoriaId,
                AlmacenID = dataContract.AlmacenId,
                Stock = 0,
                Activo = true,
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = dataContract.UsuarioAccion
            };
        }

        public void ToEditEntity(Producto entity, ProductoDC dataContract)
        {
            entity.Codigo = dataContract.Codigo;
            entity.Descripcion = dataContract.Descripcion;
            entity.CategoriaID = dataContract.CategoriaId;
            entity.AlmacenID = dataContract.AlmacenId;
            entity.UltimoUsuarioModifico = dataContract.UsuarioAccion;
            entity.FechaModificacion = DateTime.Now;
        }

        public void ToActivateProduct(Producto product, ProductoDC dataContract)
        {
            product.Activo = dataContract.Activo;
            product.UltimoUsuarioModifico = dataContract.UsuarioAccion;
            product.FechaModificacion = DateTime.Now;
        }
    }
}
