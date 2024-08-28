using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Entities
{
    public class Factura
    {
        [Key]
        public int Id { get; set; }

        // Generar el número de factura automáticamente
        public string FacturaNumero { get; set; }
        public DateTime FechaEmision { get; set; }
        public int ClienteId { get; set; }
        public decimal Total { get; set; } 

        public List<OrdenProducto> OrdenProductos { get; set; } = [];

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }
    }
}
