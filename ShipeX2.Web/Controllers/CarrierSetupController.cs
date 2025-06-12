using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipeX2.Application.Interfaces;

namespace ShipeX2.Web.Controllers
{
    [Authorize(Roles = "Super Admin,Admin")]
    public class CarrierSetupController : Controller
    {
        private readonly ICarrierSetupServices _carrierSetupServices;
        public CarrierSetupController(ICarrierSetupServices carrierSetupServices)
        {
            _carrierSetupServices = carrierSetupServices;
        }

        [HttpGet]
        public async Task<IActionResult> CarrierSetupList ()
        {
            var model = await _carrierSetupServices.GetCarrierSetupListAsync();
            return View(model);
        }
    }
}
