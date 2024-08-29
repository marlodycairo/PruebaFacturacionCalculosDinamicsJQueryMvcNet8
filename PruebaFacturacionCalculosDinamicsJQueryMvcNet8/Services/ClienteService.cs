using Microsoft.EntityFrameworkCore;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Context;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Services
{
    public class ClienteService(ApplicationDbContext context) : IClienteService
    {
        private readonly ApplicationDbContext _context = context;


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async  Task<IList<ClienteViewModel>> GetClientesAsync()
        {
            var clientes = await _context.Clientes
                .Select(cliente => new ClienteViewModel 
                { 
                    Id = cliente.Id, 
                    Firstname = cliente.Firstname, 
                    Lastname = cliente.Lastname, 
                    Email = cliente.Email
                }).ToListAsync();

            return clientes;
        }
    }
}
