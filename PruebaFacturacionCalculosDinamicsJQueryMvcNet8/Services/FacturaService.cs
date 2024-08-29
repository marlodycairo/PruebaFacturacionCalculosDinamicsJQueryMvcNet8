using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Context;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Entities;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Services
{
    public class FacturaService(ApplicationDbContext context) : IFacturaService
    {
        private readonly ApplicationDbContext _context = context;


        /// <summary>
        /// Creates a new factura (invoice) in the database asynchronously, including its associated order products.
        /// </summary>
        /// <param name="factura">The factura object containing the details to be created, including related order products.</param>
        /// <returns>A task representing the asynchronous operation of creating a new factura and saving it to the database.</returns>

        public async Task CreateFacturaAsync(Factura factura)
        {
            factura.FacturaNumero = Guid.NewGuid().ToString().ToUpper();

            await _context.Facturas.AddAsync(factura);

            await _context.OrdenProductos.AddRangeAsync(factura.OrdenProductos);

            await SaveChangesInDatabase();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task SaveChangesInDatabase()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a factura (invoice) from the database asynchronously based on its ID.
        /// </summary>
        /// <param name="facturaId">The ID of the factura to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteFacturaAsync(string facturaId)
        {
            var factura = await _context.Facturas.FirstOrDefaultAsync(invoice  => invoice.FacturaNumero == facturaId);

            _context.Facturas.Remove(factura);

            await SaveChangesInDatabase();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async  Task<List<FacturaViewModel>> GetFacturasAsync()
        {
            var facturas = await _context.Facturas

                            .Select(factura => new FacturaViewModel
                            {
                                Id = factura.Id,
                                FacturaNumero = factura.FacturaNumero,
                                FechaEmision = factura.FechaEmision,
                                ClienteId = factura.ClienteId,
                                Clientes = _context.Clientes.Select(cl => new SelectListItem
                                {
                                    Value = cl.Id.ToString(),
                                    Text = $"{cl.Firstname} {cl.Lastname}"
                                }).ToList(),

                            ProductosDisponibles = _context.OrdenProductos.Select(pr => new SelectListItem
                            {
                                Value = pr.Id.ToString(),
                                Text = pr.Producto.Name
                            }).ToList()
                        }).ToListAsync();

            return facturas;
        }

        public void UpdateFactura(Factura factura)
        {
            throw new NotImplementedException();
        }
    }
}
