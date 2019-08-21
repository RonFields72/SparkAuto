using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Data;
using SparkAuto.Model;
using SparkAuto.Utility;

namespace SparkAuto.Pages.Users
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // validate the guid param
            if (id.Trim().Length == 0)
            {
                return NotFound();
            }

            ApplicationUser = await _db.ApplicationUser.FirstOrDefaultAsync(u => u.Id == id);

            if (ApplicationUser == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                var UserFromDb = await _db.ApplicationUser.SingleOrDefaultAsync(u => u.Id == ApplicationUser.Id);
                if (UserFromDb == null)
                {
                    return NotFound();
                }
                else
                {
                    UserFromDb.Name = ApplicationUser.Name;
                    UserFromDb.PhoneNumber = ApplicationUser.PhoneNumber;
                    UserFromDb.Address = ApplicationUser.Address;
                    UserFromDb.City = ApplicationUser.City;
                    UserFromDb.PostalCode = ApplicationUser.PostalCode;

                    await _db.SaveChangesAsync();

                    return RedirectToPage("Index");
                }
            }
        }
    }
}