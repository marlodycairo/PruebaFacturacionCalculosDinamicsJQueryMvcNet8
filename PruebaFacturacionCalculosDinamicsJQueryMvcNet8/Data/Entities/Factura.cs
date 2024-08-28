using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Entities
{
    public class Factura
    {
        private decimal _total;

        [Key]
        public int Id { get; set; }

        // Generar el número de factura automáticamente
        public string FacturaNumero { get; set; }
        public DateTime FechaEmision { get; set; }
        public int ClienteId { get; set; }
        public decimal Total
        {
            get { return OrdenProductos?.Sum(x => x.Subtotal) ?? 0; }
            set { _total = value;  }
        }

        public List<OrdenProducto> OrdenProductos { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }
    }
}
