using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Entities;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices
{
    public interface IFacturaService
    {
        Task<List<FacturaViewModel>> GetFacturasAsync();
        Task<List<FacturaViewModel>> GetAllFacturasAsync();
        Task<FacturaViewModel> GetFacturaByIdAsync(int id);
        Task CreateFacturaAsync(Factura factura);
        Task DeleteFacturaAsync(string facturaId);
        Task<bool> UpdateFacturaAsync(Factura factura);
    }
}
