using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models
{
    public class FacturaViewModel
    {
        public int Id { get; set; }
        
        // Generar el número de factura automáticamente
        public string FacturaNumero { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime FechaEmision { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
        public int ClienteId { get; set; }
        public List<OrdenProductoViewModel> OrdenProductos { get; set; }
        public decimal Total => OrdenProductos?.Sum(x => x.Subtotal) ?? 0;

        public ClienteViewModel Cliente { get; set; }

        //data dropdown list
        public IEnumerable<SelectListItem> Clientes { get; set; }
        public IEnumerable<SelectListItem> ProductosDisponibles { get; set; }
    }
}
