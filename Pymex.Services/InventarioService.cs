using Pymex.Services.Contracts;
using Pymex.Services.Models;
using Pymex.Services.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.PeerResolvers;
using System.Text;
using System.Xml.Linq;

namespace Pymex.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "InventarioService" in both code and config file together.
    public class InventarioService : IInventarioService
    {
        public ResponseDataContract RegistrarEntrada(EntradaDC entrada)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = false;

            if (entrada.ExistenCamposInvalidos())
            {
                response.Mensaje = "Se tiene que completar los campos requeridos";
                return response;
            }

            try
            {
                // Convirtiendo el detalle en un XML
                XDocument xml = new XDocument(new XElement("Productos")); // Root
                foreach (var detalle in entrada.DetalleProductos)
                {
                    var productoNode = new XElement("Producto");
                    productoNode.Add(
                        new XElement("ProductoID", detalle.Producto.Id),
                        new XElement("PrecioCompraUnidad", detalle.PrecioCompraUnidad),
                        new XElement("PrecioVentaUnidad", detalle.PrecioVentaUnidad),
                        new XElement("Cantidad", detalle.Cantidad)
                    );
                    xml.Root.Add(productoNode);
                }
                    
                using (PymexEntities db = new PymexEntities())
                {
                    int rowsAffected = db.usp_RegistrarEntrada(entrada.FechaRegistro, entrada.UsuarioAccion, entrada.Proveedor.Id, xml.ToString());
                    if (rowsAffected <= 0)
                        throw new Exception("No se registró ningún registro");

                    response.EsCorrecto = true;
                    response.Mensaje = "Se registró la entrada correctamente!";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups!. Ocurrio un error al tratar de registrar.";
            }

            return response;
        }

        public ResponseDataContract RegistrarSalida(SalidaDC salida)
        {
            var response = new ResponseDataContract();
            response.EsCorrecto = false;

            if (salida.ExistenCamposInvalidos())
            {
                response.Mensaje = "Se tiene que completar los campos requeridos";
                return response;
            }

            try
            {
                // Convirtiendo el detalle en un XML
                XDocument xml = new XDocument(new XElement("Productos")); // Root
                foreach (var detalle in salida.DetalleProductos)
                {
                    var productoNode = new XElement("Producto");
                    productoNode.Add(
                        new XElement("ProductoID", detalle.Producto.Id),
                        new XElement("PrecioVentaUnidad", detalle.PrecioVentaUnidad),
                        new XElement("Cantidad", detalle.Cantidad)
                    );
                    xml.Root.Add(productoNode);
                }

                using (PymexEntities db = new PymexEntities())
                {
                    int rowsAffected = db.usp_RegistrarSalida(salida.FechaRegistro, salida.UsuarioAccion, salida.Cliente.Id, xml.ToString());
                    if (rowsAffected <= 0)
                        throw new Exception("No se registró ningún registro");

                    response.EsCorrecto = true;
                    response.Mensaje = "Se registró la salida correctamente!";
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Ups!. Ocurrio un error al tratar de registrar.";
            }

            return response;
        }
    }
}
