using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Pymex.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICategoriaService" in both code and config file together.
    [ServiceContract]
    public interface ICategoriaService : IGenericService<CategoriaDC>
    {

    }

    [DataContract]
    public class CategoriaDC
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string Descripcion { get; set; }
    }
}
