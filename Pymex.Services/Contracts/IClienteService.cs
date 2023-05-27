using Pymex.Services.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Pymex.Services.Contracts
{
    [ServiceContract]
    public interface IClienteService : IGenericService<ClienteDC>
    {

    }

    [DataContract]
    public class ClienteDC
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public TipoDocumento TipoDocumento { get; set; }
        [DataMember] public string NumeroDocumento { get; set; }
        [DataMember] public string NombreCompleto { get; set; }
        [DataMember] public DateTime FechaRegistro { get; set; }
        [DataMember] public string UsuarioAccion { get; set; }
    }
}
