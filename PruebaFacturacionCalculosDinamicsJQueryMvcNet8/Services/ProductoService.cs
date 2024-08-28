using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Context;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Services
{
    public class ProductoService(ApplicationDbContext context) : IProductoService
    {
        private readonly ApplicationDbContext _context = context;

        public ProductViewModel GetProductById(int id)
        {
            return _context.Productos
                .Where(p => p.Id == id)
                .Select(p => new ProductViewModel 
                { 
                    Id = p.Id, Name = p.Name, UnitPrice = p.UnitPrice
                }).FirstOrDefault();
        }

        public List<ProductViewModel> GetProducts()
        {
            return [.. _context.Productos
                .Select(producto => new ProductViewModel 
                    { Id = producto.Id, Name = producto.Name, UnitPrice = producto.UnitPrice})];
        }
    }
}
