using Pymex.Services.Contracts;
using Pymex.Services.Mappers.Contracts;
using Pymex.Services.Models;
using Pymex.Services.ValueObjects;
using System;
using System.Runtime.InteropServices;

namespace Pymex.Services.Mappers
{
    public class ProductoMapper : IProductoMapper
    {

        private readonly IDataContractMapper<Almacen, AlmacenDC> _almacenMapper;
        private readonly IDataContractMapper<Categoria, CategoriaDC> _categoriaMapper;

        public ProductoMapper(IDataContractMapper<Almacen, AlmacenDC> almacenMapper, IDataContractMapper<Categoria, CategoriaDC> categoriaMapper)
        {
            _almacenMapper = almacenMapper;
            _categoriaMapper = categoriaMapper; 
        }


        public ProductoDC ToDataContract(Producto entity)
        {
            return new ProductoDC
            {
                Id = entity.ProductoID,
                Codigo = entity.Codigo,
                Descripcion = entity.Descripcion,
                Categoria = _categoriaMapper.ToDataContract(entity.Categoria),
                Almacen = _almacenMapper.ToDataContract(entity.Almacen),
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
                CategoriaID = (short)dataContract.Categoria.Id,
                AlmacenID = (short)dataContract.Almacen.Id,
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
            entity.CategoriaID = (short)dataContract.Categoria.Id;
            entity.AlmacenID = (short)dataContract.Almacen.Id;
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
