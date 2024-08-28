using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models
{
    public class OrdenProductoViewModel
    {
        private decimal _subtotal;

        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal
        {
            get { return Cantidad * PrecioUnitario; }
            set { _subtotal = value; }
        }

        public int FacturaId { get; set; }

        public FacturaViewModel Factura { get; set; }

        //data dropdown list
        public int SelectedItemId { get; set; }
        public IEnumerable<SelectListItem> Items { get; set; }

        public ProductViewModel Product { get; set; }
    }
}
