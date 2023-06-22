using Pymex.Services.Contracts;
using Pymex.Services.Mappers.Contracts;
using Pymex.Services.Models;

namespace Pymex.Services.Mappers
{
    public class AlmacenMapper : IGenericMapper<Almacen, AlmacenDC>
    {
        public Almacen ToCreateEntity(AlmacenDC dataContract)
        {
            return new Almacen
            {
                AlmacenID = 0,
                Descripcion = dataContract.Descripcion,
                Direccion = dataContract.Direccion,
                Telefono = dataContract.Telefono,
                Aforo = dataContract.Aforo
            };

        }

        public AlmacenDC ToDataContract(Almacen entity)
        {
            return new AlmacenDC
            {
                Id = entity.AlmacenID,
                Descripcion = entity.Descripcion,
                Direccion = entity.Direccion,
                Telefono = entity.Telefono,
                Aforo = entity.Aforo ?? 0
            };
        }

        public void ToEditEntity(Almacen entity, AlmacenDC dataContract)
        {
            entity.Descripcion = dataContract.Descripcion;
            entity.Direccion = dataContract.Direccion;
            entity.Telefono = dataContract.Telefono;
            entity.Aforo = dataContract.Aforo;
        }
    }
}
