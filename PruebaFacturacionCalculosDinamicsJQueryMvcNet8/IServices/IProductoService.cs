using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices
{
    public interface IProductoService
    {
        Task<List<ProductViewModel>> GetProductsAsync();
        Task<ProductViewModel> GetProductByIdAsync(int id);
    }
}
