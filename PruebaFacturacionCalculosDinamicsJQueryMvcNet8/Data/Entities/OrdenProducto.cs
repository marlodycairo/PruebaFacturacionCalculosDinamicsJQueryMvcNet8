using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Entities
{
    public class OrdenProducto
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int FacturaId { get; set; }
        public decimal Subtotal { get; set; }

        [ForeignKey("FacturaId")]
        public Factura Factura { get; set; }

        [ForeignKey("ProductId")]
        public Producto Producto { get; set; }
    }
}
