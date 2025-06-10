using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipeX2.Application.DTOs;
using ShipeX2.Application.Interfaces;
using ShipeX2.Identity.Context;
using ShipeX2.Models;
using ShipeX2.Web.Models;
using static ShipeX2.Identity.Context.ApplicationDbContext;

[Authorize(Roles = "Super Admin,Admin")]
public class UserController : Controller
{
    private readonly IDynamicDbContextFactory _dbFactory;
    private readonly ApplicationDbContext _context;
    private readonly IUserAuthenticationService _userAuthenticationService;

    public UserController ( IDynamicDbContextFactory dbFactory, ApplicationDbContext context, IUserAuthenticationService userAuthenticationService )
    {
        _dbFactory = dbFactory;
        _context = context;
        _userAuthenticationService = userAuthenticationService;
    }

    #region User Authentication and Authorization 

    [HttpGet]
    public async Task<IActionResult> UserList ()
    {
        var model = await _userAuthenticationService.GetUserListAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> CreateUser()
    {
        ViewBag.Roles = await AppModel.RetrieveRoles(_context);
        ViewBag.LabelPrinters = await AppModel.RetrieveLabelPrinters(_context);
        ViewBag.InvoicePrinters = await AppModel.RetrieveInvoicePrinters(_context);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser ( [FromBody] UserModelExtended userModel )
    {
        var result = await _userAuthenticationService.CreateUserAsync(userModel);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleUserStatus(long userId)
    {
        var model = await _userAuthenticationService.ActiveInactiveUser(userId);
        return Json(model);
    }



    #endregion
}
