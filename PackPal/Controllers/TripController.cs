using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackPal.Data;
using PackPal.Models;
using PackPal.ViewModels;

namespace PackPal.Controllers
{
    public class TripController : Controller
    {
        private readonly UserManager<Users> userManager;
        private readonly AppDb _context;
        public TripController(UserManager<Users> userManager, AppDb context)
        {
            this.userManager = userManager;
            this._context = context;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TripViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var trip = new Trip
            {
                Destination = model.Destination,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Description = model.Description,
                Rating = model.Rating,
                UserId = user.Id
            };

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Profile");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await userManager.GetUserAsync(User);
            //var trip = await _context.Trips.FindAsync(id);
            //this is only for better security but the above line would still work
            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.TripId == id && t.UserId == user.Id);
            if (trip == null)
            {
                return NotFound();
            }
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Profile");

        }
        public async Task<IActionResult> Edit(TripViewModel model) //we cant only put post edit method we need a get to load the form for editing
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var existingTrip = await _context.Trips.FindAsync(model.TripId);
            if (existingTrip == null || existingTrip.UserId != userManager.GetUserId(User))
            {
                return NotFound();
            }
            existingTrip.Destination = model.Destination;
            existingTrip.StartDate = model.StartDate;
            existingTrip.EndDate = model.EndDate;
            existingTrip.Description = model.Description;
            existingTrip.Rating = model.Rating;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Profile");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.TripId == id && t.UserId == user.Id);

            if (trip == null)
            {
                return NotFound();
            }

            var model = new TripViewModel
            {
                TripId = trip.TripId,
                Destination = trip.Destination,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                Description = trip.Description,
                Rating = trip.Rating
            };

            return View(model);
        }

    }
}
