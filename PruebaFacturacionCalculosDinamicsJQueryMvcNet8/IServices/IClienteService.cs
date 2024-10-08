﻿using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Entities;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices
{
    public interface IClienteService
    {
        Task<IList<ClienteViewModel>> GetClientesAsync();
    }
}
