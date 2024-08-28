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

        public void CreateFactura(Factura factura)
        {
            var crearFactura = new Factura
            {
                Id = factura.Id,
                FacturaNumero = Guid.NewGuid().ToString().ToUpper(),
                FechaEmision = factura.FechaEmision,
                ClienteId = factura.ClienteId,
                Total = factura.Total
            };

            _context.Facturas.Add(crearFactura);

            _context.SaveChanges();

            foreach (var item in factura.OrdenProductos)
            {
                var orden = new OrdenProducto
                {
                    Id = item.Id,
                    FacturaId = item.FacturaId,
                    ProductId = item.ProductId,
                    PrecioUnitario = item.PrecioUnitario,
                    Cantidad = item.Cantidad,
                    Subtotal = item.Subtotal
                };
                
                _context.OrdenProductos.Add(orden);
            }

            _context.SaveChanges();
        }

        public void DeleteFactura(string facturaId)
        {
            var factura = _context.Facturas.FirstOrDefault(f => f.FacturaNumero == facturaId);

            _context.Facturas.Remove(factura);

            _context.SaveChanges();
        }

        //public List<FacturaViewModel> GetAllFacturas()
        //{
        //    var facturas = _context.Facturas.Include(x => x.Cliente).ToList();
        //        .Select(f => new FacturaViewModel
        //        {
        //            Id = f.Id,
        //            FacturaNumero = f.FacturaNumero,
        //            FechaEmision = f.FechaEmision,
        //            ClienteId = f.ClienteId,
        //            Cliente = _context.Clientes
        //            .Select(c => new ClienteViewModel
        //            {
        //                Id = c.Id,
        //                Firstname = c.Firstname,
        //                Lastname = c.Lastname,
        //                Email = c.Email
        //            })
        //        })
        //}

        //public FacturaViewModel GetFacturaById(string facturaId)
        //{
        //    var factura = _context.Facturas.Where(x => x.FacturaNumero == x.FacturaNumero)
        //        .Select(factura => new FacturaViewModel 
        //        { 
        //            Id = factura.Id,
        //            FacturaNumero = factura.FacturaNumero,
        //            FechaEmision = factura.FechaEmision,
        //            ClienteId = factura.ClienteId,
        //            OrdenProductos = _context.OrdenProductos.Where(x => x.FacturaId == factura.Id)
        //                .Select(orden => new OrdenProducto
        //                {
        //                    Id = orden.Id,
        //                    FacturaId = orden.FacturaId,
        //                    i
        //                })
        //        })


        //}

        public List<FacturaViewModel> GetFacturas()
        {
            var facturas = _context.Facturas
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
                }).ToList();

            return facturas;
        }

        public void UpdateFactura(Factura factura)
        {
            throw new NotImplementedException();
        }
    }
}
