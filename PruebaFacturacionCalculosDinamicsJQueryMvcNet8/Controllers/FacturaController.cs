using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Entities;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Models;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Controllers
{
    public class FacturaController(IClienteService clienteService, IFacturaService facturaService,
        IOrdenProductService ordenProductService, IProductoService productoService) : Controller
    {
        private readonly IClienteService _clienteService = clienteService;
        private readonly IFacturaService _facturaService = facturaService;
        private readonly IOrdenProductService _ordenProductService = ordenProductService;
        private readonly IProductoService _productoService = productoService;

        // GET: Factura
        public IActionResult Index()
        {
            return View(_facturaService.GetFacturas());
        }

        public IActionResult Create()
        {
            var factura = new FacturaViewModel
            {
                //FacturaNumero = Guid.NewGuid().ToString(),
                FechaEmision = DateTime.UtcNow,
                ProductosDisponibles = _productoService.GetProducts()
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                Clientes = _clienteService.GetClientes()
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Firstname} {c.Lastname}"}).ToList()
            };

            return View(factura);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Factura factura)
        {
            _facturaService.CreateFactura(factura);

            return Json(new { success = true, message = "Factura guardada con éxito" });
        }

        [HttpGet]
        public JsonResult GetProductById(int id)
        {
            var product = _productoService.GetProductById(id);

            if (product == null)
            {
                return Json(new { success = false, message = "Producto no encontrado." });
            }

            return Json(new { success = true, product });
        }
    }
}
