using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Pymex.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAlmacenService" in both code and config file together.
    [ServiceContract]
    public interface IAlmacenService : IGenericService<AlmacenDC>
    {
    }

    [DataContract]
    public class AlmacenDC
    {
        [DataMember(Order = 1)] public int Id { get; set; }
        [DataMember(Order = 2)] public string Descripcion { get; set; }
        [DataMember(Order = 3)] public string Direccion { get; set; }
        [DataMember(Order = 4)] public string Telefono { get; set; }
        [DataMember(Order = 5)] public int Aforo { get; set; }
    }
}
