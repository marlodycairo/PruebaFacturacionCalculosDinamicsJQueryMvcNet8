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
            return View(await _facturaService.GetAllFacturasAsync());
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="factura"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async  Task<JsonResult> GetProductById(int id)
        {
            var product = await _productoService.GetProductByIdAsync(id);

            if (product is null)
            {
                return Json(new { success = false, message = "Producto no encontrado." });
            }

            return Json(new { success = true, product });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var factura = await _facturaService.GetFacturaByIdAsync(id);

            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var factura = await _facturaService.GetFacturaByIdAsync(id);

            if (factura is null)
            {
                return NotFound();
            }

            // Obtener productos disponibles para la edición
            ViewBag.ProductosDisponibles = new SelectList(await _productoService.GetProductsAsync(), "Id", "Name");


            // Podrías necesitar cargar alguna lista adicional, como productos disponibles
            return View(factura);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Factura factura)
        {
            if (ModelState.IsValid)
            {
                var facturaExistente = await _facturaService.GetFacturaByIdAsync(factura.Id);

                if (facturaExistente == null)
                {
                    return NotFound();
                }

                // Actualiza los detalles de la factura
                facturaExistente.FechaEmision = factura.FechaEmision;

                // Limpiar y actualizar los productos asociados a la factura
                facturaExistente.OrdenProductos.Clear();

                foreach (var item in factura.OrdenProductos)
                {
                    facturaExistente.OrdenProductos.Add(new OrdenProductoViewModel
                    {
                        ProductId = item.ProductId,
                        PrecioUnitario = item.PrecioUnitario,
                        Cantidad = item.Cantidad,
                        Subtotal = item.Subtotal
                    });
                }

                // Recalcular el total
                facturaExistente.Total = facturaExistente.OrdenProductos.Sum(x => x.Subtotal);

                var facturaUpdated = new Factura
                {
                    Id = facturaExistente.Id,
                    ClienteId = facturaExistente.ClienteId,
                    FacturaNumero = facturaExistente.FacturaNumero,
                    FechaEmision = facturaExistente.FechaEmision,
                    Total = facturaExistente.Total
                };

                // Guarda los cambios
                await _facturaService.UpdateFacturaAsync(facturaUpdated);

                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, recargar la lista de productos
            ViewBag.ProductosDisponibles = new SelectList(await _productoService.GetProductsAsync(), "Id", "Nombre");
            return View(factura);
        }
    }
}