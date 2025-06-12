using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipeX2.Application.Interfaces;

namespace ShipeX2.Web.Controllers
{
    [Authorize(Roles = "Admin,  Super Admin")]
    public class ImporterController : Controller
    {
        private readonly IImporterService _importerService;
        public ImporterController(IImporterService importerService)
        {
            _importerService = importerService;
        }

        [HttpGet]
        public async Task<IActionResult> ManageImporter ()
        {
            var model = await _importerService.GetAllImporters();
            return View(model);
        }
    }
}
