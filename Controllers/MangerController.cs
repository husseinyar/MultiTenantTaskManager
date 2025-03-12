

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultiTenantTaskManager.Data;
using MultiTenantTaskManager.Models;
using MultiTenantTaskManager.ViewModel;
using System.Threading.Tasks;


public class MangerController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MangerController> _logger;
    public MangerController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, ILogger<MangerController> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogWarning("User is not authenticated.");
            return RedirectToAction("Login", "Auth");
        }

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        if (!isAdmin)
        {
            _logger.LogWarning("User does not have the Admin role.");
            return Forbid();
        }

        var users = await _userManager.Users.ToListAsync();
        var userList = new List<UserViewModel>();

        foreach (var u in users)
        {
            var roles = await _userManager.GetRolesAsync(u);
            userList.Add(new UserViewModel
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                UserName = u.UserName,
                TenantId = u.TenantId,
                Roles = roles
            });
        }

        return View(userList);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> CreateUser()
    {
        var roles = await _roleManager.Roles.Select(r => new SelectListItem
        {
            Value = r.Name,
            Text = r.Name
        }).ToListAsync();

        var tenantIds = await _context.Tenants.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name
        }).ToListAsync();

        var model = new CreateUserViewModel
        {
            RegisterModel = new RegisterModel(),
            Roles = roles,
            TenantIds = tenantIds
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.RegisterModel.Username,
                Email = model.RegisterModel.Email,
                FullName = model.RegisterModel.FullName,
                TenantId = model.RegisterModel.TenantId
            };
            var result = await _userManager.CreateAsync(user, model.RegisterModel.Password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.RegisterModel.Role))
                {
                    await _userManager.AddToRoleAsync(user, model.RegisterModel.Role);
                }
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // Reload roles and tenant IDs in case of error
        model.Roles = await _roleManager.Roles.Select(r => new SelectListItem
        {
            Value = r.Name,
            Text = r.Name
        }).ToListAsync();

        model.TenantIds = await _context.Tenants.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name
        }).ToListAsync();

        return View(model);
    }


    
    [HttpGet]
    public async Task<IActionResult> EditUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var roles = await _roleManager.Roles.Select(r => new SelectListItem
        {
            Value = r.Name,
            Text = r.Name
        }).ToListAsync();

        var tenantIds = await _context.Tenants.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name
        }).ToListAsync();

        var model = new EditUserViewModel
        {
            Id = user.Id,
            Username = user.UserName,
            FullName = user.FullName,
            Email = user.Email,
            TenantId = user.TenantId,
            Roles = roles,
            TenantIds = tenantIds,
            SelectedRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(EditUserViewModel model)
    {


        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogWarning("User is not authenticated.");
            return RedirectToAction("Login", "Auth");
        }

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        if (!isAdmin)
        {
            _logger.LogWarning("User does not have the Admin role.");
            return Forbid();
        }





        if (ModelState.IsValid)
        {
            var users = await _userManager.FindByIdAsync(model.Id);
            if (users == null)
            {
                return NotFound();
            }

            user.UserName = model.Username;
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.TenantId = model.TenantId;

            var result = await _userManager.UpdateAsync(users);
            if (result.Succeeded)
            {
                var currentRoles = await _userManager.GetRolesAsync(users);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!string.IsNullOrEmpty(model.SelectedRole))
                {
                    await _userManager.AddToRoleAsync(users, model.SelectedRole);
                }
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // Reload roles and tenant IDs in case of error
        model.Roles = await _roleManager.Roles.Select(r => new SelectListItem
        {
            Value = r.Name,
            Text = r.Name
        }).ToListAsync();

        model.TenantIds = await _context.Tenants.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name
        }).ToListAsync();

        return View(model);
    }

   
    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogWarning("User is not authenticated.");
            return RedirectToAction("Login", "Auth");
        }

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        if (!isAdmin)
        {
            _logger.LogWarning("User does not have the Admin role.");
            return Forbid();
        }

        var users= await _userManager.FindByIdAsync(id);
        if (users == null)
        {
            return NotFound();
        }

        // Remove user from roles
        var roles = await _userManager.GetRolesAsync(users);
        if (roles.Any())
        {
            var result = await _userManager.RemoveFromRolesAsync(users, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Error removing user from roles.");
                return View();
            }
        }

        // Delete user
        var deleteResult = await _userManager.DeleteAsync(user);
        if (!deleteResult.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Error deleting user.");
            return View();
        }

        return RedirectToAction("Index");
    }

 

   



}
