using System.ComponentModel.DataAnnotations;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Entities
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
