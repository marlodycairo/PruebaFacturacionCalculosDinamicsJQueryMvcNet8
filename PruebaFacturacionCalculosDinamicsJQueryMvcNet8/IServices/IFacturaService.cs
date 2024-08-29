using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Entities;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices
{
    public interface IFacturaService
    {
        Task<List<FacturaViewModel>> GetFacturasAsync(); 
        Task CreateFacturaAsync(Factura factura);
        Task DeleteFacturaAsync(string facturaId);
        void UpdateFactura(Factura factura);
    }
}
