using Pymex.Services.Contracts;
using Pymex.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Pymex.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProductoService" in both code and config file together.
    public class ProductoService : IProductoService
    {
        public string DoWork()
        {
            using(PymexEntities db  = new PymexEntities())
            {
                var query = (from producto in db.Producto select producto).FirstOrDefault().Descripcion;
                return query;
            }
           
        }
    }
}
