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

        public async Task<List<FacturaViewModel>> GetAllFacturasAsync()
        {
            var facturas = await _context.Facturas.Include(cliente => cliente.Cliente).ToListAsync();

            var facturasViewModel = facturas.Select(factura => new FacturaViewModel
            {
                Id = factura.Id,
                FechaEmision = factura.FechaEmision,
                FacturaNumero = factura.FacturaNumero,
                ClienteId = factura.ClienteId,
                Total = factura.Total,
                Cliente = new ClienteViewModel 
                { 
                    Id = factura.Cliente.Id, 
                    Firstname = factura.Cliente.
                    Firstname, Lastname = factura.Cliente.Lastname, 
                    Email = factura.Cliente.Email
                }
            }).ToList();

            return facturasViewModel;
        }

        public async Task<bool> UpdateFacturaAsync(Factura factura)
        {
            var facturaExistente = await _context.Facturas
                .Include(f => f.OrdenProductos)
                .FirstOrDefaultAsync(f => f.Id == factura.Id);

            if (facturaExistente == null)
            {
                return false; // Si la factura no existe, retornamos falso
            }

            // Actualizar las propiedades de la factura
            facturaExistente.FechaEmision = factura.FechaEmision;
            facturaExistente.Total = factura.Total;

            // Recorrer los productos existentes en la factura
            foreach (var productoExistente in facturaExistente.OrdenProductos)
            {
                // Buscar si el producto existe en la nueva lista de productos de la factura
                var productoActualizado = factura.OrdenProductos.FirstOrDefault(p => p.ProductId == productoExistente.ProductId);

                if (productoActualizado != null)
                {
                    // Si el producto existe, actualizamos sus propiedades
                    productoExistente.Cantidad = productoActualizado.Cantidad;
                    productoExistente.PrecioUnitario = productoActualizado.PrecioUnitario;
                    productoExistente.Subtotal = productoActualizado.Subtotal;
                }
            }

            // Añadir los nuevos productos que no estaban en la factura original
            foreach (var productoNuevo in factura.OrdenProductos)
            {
                var productoExistente = facturaExistente.OrdenProductos.FirstOrDefault(p => p.ProductId == productoNuevo.ProductId);
                if (productoExistente == null)
                {
                    // Si el producto no existe, lo añadimos a la factura
                    facturaExistente.OrdenProductos.Add(new OrdenProducto
                    {
                        ProductId = productoNuevo.ProductId,
                        Cantidad = productoNuevo.Cantidad,
                        PrecioUnitario = productoNuevo.PrecioUnitario,
                        Subtotal = productoNuevo.Subtotal
                    });
                }
            }

            // Guardar cambios en la base de datos
            await SaveChangesInDatabase();

            return true;
        }

        public async Task<FacturaViewModel> GetFacturaByIdAsync(int id)
        {
            var factura = await _context.Facturas.Include(cliente => cliente.Cliente).FirstOrDefaultAsync(x => x.Id == id);
            var orders = await _context.OrdenProductos.Where(x => x.FacturaId == id).Include(p => p.Producto).ToListAsync();
            
            var facturaViewModel = new FacturaViewModel
            {
                Id = factura.Id,
                FechaEmision = factura.FechaEmision,
                FacturaNumero = factura.FacturaNumero,
                ClienteId = factura.ClienteId,
                Total = factura.Total,
                Cliente = new ClienteViewModel
                {
                    Id = factura.Cliente.Id,
                    Firstname = factura.Cliente.
                    Firstname,
                    Lastname = factura.Cliente.Lastname,
                    Email = factura.Cliente.Email
                },
                ProductosDisponibles = _context.OrdenProductos.Select(pr => new SelectListItem
                {
                    Value = pr.Id.ToString(),
                    Text = pr.Producto.Name
                }).ToList(),
                OrdenProductos = orders.Select(orden => new OrdenProductoViewModel
                {
                    Id = orden.Id,
                    FacturaId = orden.Id,
                    ProductId = orden.ProductId,
                    PrecioUnitario = orden.PrecioUnitario,
                    Cantidad = orden.Cantidad,
                    Subtotal = orden.Subtotal,
                    Product = new ProductViewModel
                    {
                        Id = orden.Producto.Id,
                        Name = orden.Producto.Name,
                        UnitPrice = orden.Producto.UnitPrice
                    }
                }).ToList()
            };

            return facturaViewModel;
        }
    }
}
