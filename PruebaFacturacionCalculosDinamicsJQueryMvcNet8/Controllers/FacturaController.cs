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
                // Obtener la factura existente desde la base de datos usando el ID
                var facturaExistente = await _facturaService.GetFacturaByIdAsync(factura.Id);

                if (facturaExistente == null)
                {
                    return NotFound();
                }

                // Mapear los datos actualizados sobre la factura existente
                facturaExistente.FechaEmision = factura.FechaEmision;
                facturaExistente.Total = factura.Total;

                // Actualizar los productos asociados
                facturaExistente.OrdenProductos.Clear();
                foreach (var producto in factura.OrdenProductos)
                {
                    facturaExistente.OrdenProductos.Add(new OrdenProductoViewModel
                    {
                        Id = producto.Id,
                        ProductId = producto.ProductId,
                        PrecioUnitario = producto.PrecioUnitario,
                        Cantidad = producto.Cantidad,
                        Subtotal = producto.Subtotal
                    });
                }

                // Convertir de FacturaViewModel a Factura
                var facturaActualizada = new Factura
                {
                    Id = facturaExistente.Id,
                    FechaEmision = facturaExistente.FechaEmision,
                    ClienteId = facturaExistente.ClienteId,
                    Total = facturaExistente.Total,
                    OrdenProductos = facturaExistente.OrdenProductos.Select(op => new OrdenProducto
                    {
                        Id = op.Id,
                        ProductId = op.ProductId,
                        Cantidad = op.Cantidad,
                        PrecioUnitario = op.PrecioUnitario,
                        Subtotal = op.Subtotal
                    }).ToList()
                };

                // Actualizar la factura en la base de datos usando el servicio
                await _facturaService.UpdateFacturaAsync(facturaActualizada);

                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, recargar la vista con los datos actuales
            ViewBag.ProductosDisponibles = new SelectList(await _productoService.GetProductsAsync(), "Id", "Nombre");

            return View(factura);
        }
    }
}