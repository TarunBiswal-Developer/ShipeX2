using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShipeX2.Web.Controllers
{
    [Authorize("Roles =Super Admin, Admin")]
    public class ImporterController : Controller
    {
        public IActionResult ManageImporter ()
        {
            return View();
        }
    }
}
