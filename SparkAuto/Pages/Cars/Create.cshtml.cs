﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SparkAuto.Data;
using SparkAuto.Model;

namespace SparkAuto.Pages.Cars
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Car Car { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet(string userId = null)
        {
            Car = new Car();

            if (userId == null)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                userId = claim.Value;
            }

            Car.UserId = userId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _db.Car.Add(Car);
            await _db.SaveChangesAsync();
            StatusMessage = "Car has been added sucessfully.";
            return RedirectToPage("Index", new { userId = Car.UserId });
        }
    }
}