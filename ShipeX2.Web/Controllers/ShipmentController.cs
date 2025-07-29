using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipeX2.Application.Interfaces;
using ShipeX2.Identity.Context;
using ShipeX2.Web.Models;
using static ShipeX2.Identity.Context.ApplicationDbContext;

namespace ShipeX2.Web.Controllers
{
    [Authorize(Roles = "Super Admin, Admin")]
    public class ShipmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ShipmentController ( ApplicationDbContext context )
        {
            _context = context;
        }

        public async Task<IActionResult> QuickShip ()
        {
            ViewBag.ClientList = await AppModel.ClientListAsync(_context);
            ViewBag.LabelPrinters = await AppModel.RetrieveLabelPrinters(_context);
            ViewBag.InvoicePrinters = await AppModel.RetrieveInvoicePrinters(_context);
            return View();
        }
    }
}
