using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackPal.Data;
using PackPal.Models;
using PackPal.ViewModels;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<Users> _userManager;
    private readonly AppDb _context;

    public ProfileController(UserManager<Users> userManager, AppDb context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var trips = await _context.Trips
             .Where(t => t.UserId == user.Id)
             .OrderByDescending(t => t.StartDate)
             .ToListAsync();

        var model = new ProfileViewModel
        {
            Email = user.Email,
            CustomUsername = user.CustomUsername,
            PhotoUrl = user.PhotoPath,
            Trips = trips
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(ProfileViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        var usernameTaken = _userManager.Users.Any(u => u.CustomUsername == model.CustomUsername && u.Id != user.Id);

        if (usernameTaken)
        {
            ModelState.AddModelError("CustomUsername", "This username is already taken.");
            return View(model);
        }

        user.CustomUsername = model.CustomUsername;

        if (model.Photo != null && model.Photo.Length > 0)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Photo.FileName);
            var filePath = Path.Combine("wwwroot/uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Photo.CopyToAsync(stream);
            }

            user.PhotoPath = "/uploads/" + fileName;
        }

        await _userManager.UpdateAsync(user);
        return RedirectToAction("Index");
    }
}
