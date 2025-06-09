using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipeX2.Application.Interfaces;
using ShipeX2.Identity.Context;
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

    #endregion
}
