using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Context;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Services
{
    public class ClienteService(ApplicationDbContext context) : IClienteService
    {
        private readonly ApplicationDbContext _context = context;

        public List<ClienteViewModel> GetClientes()
        {
            var clientes = _context.Clientes
                .Select(cliente => new ClienteViewModel 
                { 
                    Id = cliente.Id, 
                    Firstname = cliente.Firstname, 
                    Lastname = cliente.Lastname, 
                    Email = cliente.Email
                }).ToList();

            return clientes;
        }
    }
}
