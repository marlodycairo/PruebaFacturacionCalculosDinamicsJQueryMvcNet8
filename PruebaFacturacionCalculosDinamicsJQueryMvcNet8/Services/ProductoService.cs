﻿using Microsoft.EntityFrameworkCore;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Context;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Services
{
    public class ProductoService(ApplicationDbContext context) : IProductoService
    {
        private readonly ApplicationDbContext _context = context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async  Task<ProductViewModel> GetProductByIdAsync(int id)
        {
            return await _context.Productos
                    .Where(p => p.Id == id)
                    .Select(p => new ProductViewModel 
                    { 
                        Id = p.Id, Name = p.Name, UnitPrice = p.UnitPrice
                    }).FirstOrDefaultAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductViewModel>> GetProductsAsync()
        {
            return await _context.Productos
                  .Select(producto => new ProductViewModel
                                { Id = producto.Id,
                                Name = producto.Name,
                                UnitPrice = producto.UnitPrice })
                  .ToListAsync();
        }
    }
}
