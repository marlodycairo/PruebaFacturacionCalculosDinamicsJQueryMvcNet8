using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices
{
    public interface IProductoService
    {
        List<ProductViewModel> GetProducts();
        ProductViewModel GetProductById(int id);
    }
}
