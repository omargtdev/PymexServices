﻿using Pymex.Services.Contracts.Operations;
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
    public interface IClienteService : 
        IGenericService<ClienteDC>, 
        IDeleteOperation<ClienteDC>,
        IListarPorCantidadOperation<ClienteDC>,
        IBuscarPorDocumentoOperation<ClienteDC>
    {

    }

    [DataContract]
    public class ClienteDC
    {
        [DataMember(Order = 1)] public int Id { get; set; }
        [DataMember(Order = 2)] public TipoDocumento TipoDocumento { get; set; }
        [DataMember(Order = 3)] public string NumeroDocumento { get; set; }
        [DataMember(Order = 4)] public string NombreCompleto { get; set; }
        [DataMember(Order = 5)] public HistorialSeguimientoDC HistorialSeguimiento { get; set; }
        [DataMember(Order = 6)] public string UsuarioAccion { get; set; }
    }
}
