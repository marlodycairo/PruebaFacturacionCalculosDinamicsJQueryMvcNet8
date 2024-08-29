using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Entities;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Controllers
{
    public class FacturaController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly IFacturaService _facturaService;
        private readonly IOrdenProductService _ordenProductService;
        private readonly IProductoService _productoService;

        public FacturaController(IClienteService clienteService, IFacturaService facturaService,
            IOrdenProductService ordenProductService, IProductoService productoService)
        {
            _clienteService = clienteService;
            _facturaService = facturaService;
            _ordenProductService = ordenProductService;
            _productoService = productoService;
        }


        // GET: Factura
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _facturaService.GetFacturasAsync());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Create()
        {
            var factura = new FacturaViewModel
            {
                FechaEmision = DateTime.UtcNow,
                ProductosDisponibles = (await _productoService.GetProductsAsync())
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                Clientes = (await _clienteService.GetClientesAsync())
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Firstname} {c.Lastname}" }).ToList()
            };

            return View(factura);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Factura factura)
        {
            if (factura is null || factura.OrdenProductos is null || factura.OrdenProductos.Count == 0)
            {
                return Json(new { success = false, message = "La factura o los productos no son válidos." });
            }

            await _facturaService.CreateFacturaAsync(factura);

            return Json(new { success = true, message = "Factura guardada con éxito" });
        }

        [HttpGet]
        public JsonResult GetProductById(int id)
        {
            var product = _productoService.GetProductByIdAsync(id);

            if (product == null)
            {
                return Json(new { success = false, message = "Producto no encontrado." });
            }

            return Json(new { success = true, product });
        }
    }
}
