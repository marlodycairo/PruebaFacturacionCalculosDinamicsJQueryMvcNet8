using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Entities;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices
{
    public interface IFacturaService
    {
        List<FacturaViewModel> GetFacturas();
        List<FacturaViewModel> GetAllFacturas();
        void CreateFactura(Factura factura);
        //FacturaViewModel GetFacturaById(string facturaId);
        void DeleteFactura(string facturaId);
        void UpdateFactura(Factura factura);
    }
}
