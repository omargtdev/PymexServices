﻿using Pymex.Services.Contracts.Operations;
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
    public interface ICategoriaService : IGenericService<CategoriaDC>, IDeleteOperation<CategoriaDC>
    {

    }

    [DataContract]
    public class CategoriaDC
    {
        [DataMember(Order = 1)] public int Id { get; set; }
        [DataMember(Order = 2)] public string Descripcion { get; set; }
    }
}
