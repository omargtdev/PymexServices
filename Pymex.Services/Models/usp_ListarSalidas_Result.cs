//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pymex.Services.Models
{
    using System;
    
    public partial class usp_ListarSalidas_Result
    {
        public int SalidaID { get; set; }
        public string Codigo { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public int ClienteID { get; set; }
        public Nullable<byte> TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string NombreCompleto { get; set; }
        public Nullable<System.DateTime> ClienteFechaRegistro { get; set; }
        public string ClienteUsuarioRegistro { get; set; }
        public Nullable<System.DateTime> ClienteFechaModificacion { get; set; }
        public string ClienteUltimoUsuarioModifico { get; set; }
        public System.DateTime SalidaFechaRegistro { get; set; }
        public string SalidaUsuarioRegistro { get; set; }
    }
}
