using Pymex.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pymex.Services.ValueObjects
{

    /// <summary>
    ///     Clase para los objetos contractuales que presenten campos de seguimiento
    /// </summary>
    [DataContract]
    public class HistorialSeguimientoDC
    {
        [DataMember(Order = 1)] public DateTime FechaRegistro { get; set; }
        [DataMember(Order = 2)] public string UsuarioRegistro { get; set; }
        [DataMember(Order = 3)] public DateTime? FechaModificacion { get; set; }
        [DataMember(Order = 4)] public string UltimoUsuarioModificacion { get; set; }

    }
}
