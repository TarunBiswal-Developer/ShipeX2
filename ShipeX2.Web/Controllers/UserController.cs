using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipeX2.Identity.Context;
using ShipeX2.Models;
using static ShipeX2.Identity.Context.ApplicationDbContext;

[Authorize(Roles = "Super Admin,Admin")]
public class UserController : Controller
{
    private readonly IDynamicDbContextFactory _dbFactory;
    private readonly ApplicationDbContext _context;

    public UserController ( IDynamicDbContextFactory dbFactory, ApplicationDbContext context )
    {
        _dbFactory = dbFactory;
        _context = context;
    }

    public IActionResult Index ( string clientId )
    {
        return View();
    }
}
