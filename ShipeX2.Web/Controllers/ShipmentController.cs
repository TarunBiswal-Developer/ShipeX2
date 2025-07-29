using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipeX2.Application.Interfaces;
using ShipeX2.Identity.Context;
using ShipeX2.Web.Models;

namespace ShipeX2.Web.Controllers
{
    [Authorize(Roles = "Super Admin, Admin")]
    public class ShipmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IShipment _shipmentServices;
        public ShipmentController ( ApplicationDbContext context, IShipment shipment )
        {
            _context = context;
            _shipmentServices = shipment;
        }

        public async Task<IActionResult> QuickShip ()
        {
            ViewBag.ClientList = await AppModel.ClientListAsync(_context);
            ViewBag.LabelPrinters = await AppModel.RetrieveLabelPrinters(_context);
            ViewBag.InvoicePrinters = await AppModel.RetrieveInvoicePrinters(_context);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ClientsCareer (long clientId)
        {
            var clientsCareersList = await _shipmentServices.ClientsCareerAsync(clientId);
            return Json(clientsCareersList);
        }

        [HttpPost]
        public async Task<IActionResult> RetrieveDefaultAccountInfo (long careerId )
        {
            var defaultAccount = await _shipmentServices.ClientsDefaultAccountInfo(careerId);
            return Json(defaultAccount);
        }

        [HttpPost]
        public async Task<IActionResult> RetrieveShipServicesOfClient ( long careerId)
        {
            var shipServicesList = await _shipmentServices.ShipServiceListAsync(careerId);
            return Json(shipServicesList);
        }

        [HttpPost]
        public async Task<IActionResult> RetrievePackaging (string shipViaCode, decimal weight )
        {
            var shipPackagingList = await _shipmentServices.RetrievePackagingAsync(shipViaCode, weight);
            return Json(shipPackagingList);
        }
    }
}
