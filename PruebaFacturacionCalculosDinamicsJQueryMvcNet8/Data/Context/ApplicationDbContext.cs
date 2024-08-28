using Microsoft.EntityFrameworkCore;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Entities;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :DbContext(options)
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<OrdenProducto> OrdenProductos { get; set; }
        public DbSet<Producto> Productos { get; set; }
    }
}
