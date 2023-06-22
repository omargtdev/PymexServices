using Pymex.Services.Contracts;
using Pymex.Services.Mappers.Contracts;
using Pymex.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pymex.Services.Mappers
{
    public class CategoriaMapper : IGenericMapper<Categoria, CategoriaDC>
    {
        public Categoria ToCreateEntity(CategoriaDC dataContract)
        {
            return new Categoria
            {
                CategoriaID = 0,
                Descripcion = dataContract.Descripcion
            };
        }

        public CategoriaDC ToDataContract(Categoria entity)
        {
            return new CategoriaDC
            {
                Id = entity.CategoriaID,
                Descripcion = entity.Descripcion
            };
        }

        public void ToEditEntity(Categoria entity, CategoriaDC dataContract)
        {
            entity.Descripcion = dataContract.Descripcion;
        }
    }
}
