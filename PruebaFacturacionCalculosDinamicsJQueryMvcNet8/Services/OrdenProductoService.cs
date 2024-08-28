using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Context;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Services
{
    public class OrdenProductoService(ApplicationDbContext context) : IOrdenProductService
    {
        private readonly ApplicationDbContext _context = context;

        public List<OrdenProductoViewModel> GetProductos()
        {
            var productos =  _context.OrdenProductos
                .Select(orden =>
                    new OrdenProductoViewModel
                    {
                        Id = orden.Id,
                        ProductId = orden.ProductId,
                        Cantidad = orden.Cantidad,
                        PrecioUnitario = orden.PrecioUnitario,
                        FacturaId = orden.FacturaId,
                        Subtotal = orden.Subtotal
                    }).ToList();

            return productos;
        }
    }
}
