using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipeX2.Application.DTOs;
using ShipeX2.Application.Interfaces;
using ShipeX2.Identity.Context;
using ShipeX2.Web.Models;

namespace ShipeX2.Web.Controllers
{
    [Authorize(Roles = "Super Admin,Admin")]
    public class CarrierSetupController : Controller
    {
        private readonly ICarrierSetupServices _carrierSetupServices;
        private readonly ApplicationDbContext _context;
        public CarrierSetupController(ICarrierSetupServices carrierSetupServices, ApplicationDbContext context)
        {
            _carrierSetupServices = carrierSetupServices;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> CarrierSetupList() => View(await _carrierSetupServices.GetCarrierSetupListAsync());

        [HttpGet]
        public async Task<IActionResult> CreateShippingService()
        {
            ViewBag.ShipCarrierList = await AppModel.ShipCarrierListAsync(_context);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPackingList(int carrierId) => Json(await _carrierSetupServices.GetPackingListAsync(carrierId));
        [HttpGet]
        public async Task<IActionResult> GetCarrierPackingById(int packingId) => Json(await _carrierSetupServices.GetCarrierPackingById(packingId));
        [HttpPost]
        public async Task<IActionResult> UpdateCarrierPacking(PackingViewModel model) => Json(await _carrierSetupServices.UpdateCarrierPackingAsync(model));
        [HttpPost]
        public async Task<IActionResult> CreateCarrierPacking(PackingViewModel model) => Json(await _carrierSetupServices.CreateCarrierPackingAsync(model));
    }
}
