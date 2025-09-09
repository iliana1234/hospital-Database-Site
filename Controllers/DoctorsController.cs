using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalDataBase.Data;
using HospitalDataBase.Models;
using Microsoft.AspNetCore.Authorization;

namespace HospitalDataBase.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Doctors
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Get all doctors and their patients
            var doctorsWithPatients = await _context.Doctors
                .Include(d => d.Patients) // Include their Patients
                .ToListAsync();

            return View(doctorsWithPatients);
        }

        // GET: Doctors/SearchDoctors
        public async Task<IActionResult> SearchDoctors()
        {
            return View("SearchDoctors");
        }

        // GET: Doctors/ShowResult
        public async Task<IActionResult> ShowResult(int? Id, string LastName, string Position)
        {
            var query = _context.Doctors.AsQueryable();

            if (Id.HasValue && Id > 0)
            {
                query = query.Where(d => d.Id == Id.Value);
            }

            if (!string.IsNullOrWhiteSpace(LastName))
            {
                query = query.Where(d => d.LastName.Contains(LastName));
            }

            if (!string.IsNullOrWhiteSpace(Position))
            {
                query = query.Where(d => d.Position.Contains(Position));
            }

            // Include patients for the search results
            var doctorsWithPatients = await query.Include(d => d.Patients).ToListAsync();

            return View("Index", doctorsWithPatients);
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctors = await _context.Doctors
                .Include(d => d.Patients)  // Get the found patients related to the doctor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctors == null)
            {
                return NotFound();
            }

            return View(doctors);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Position,AboutInfo,ImagePath")] Doctors doctors, IFormFile DoctorImage)
        {
            if (DoctorImage != null && DoctorImage.Length > 0)
            {
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                // If the directory doesn't exist create it
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Create a file path
                var filePath = Path.Combine(directory, DoctorImage.FileName);

                // Save the image
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await DoctorImage.CopyToAsync(stream);
                }
                // Save the image path in the database
                doctors.ImagePath = "/images/" + DoctorImage.FileName;
            }
            else
            {
                // In case there is no image uploaded inset null value ass the file path
                doctors.ImagePath = null;
            }
            _context.Add(doctors);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctors = await _context.Doctors.FindAsync(id);
            if (doctors == null)
            {
                return NotFound();
            }
            return View(doctors);
        }
        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Position,AboutInfo,ImagePath")] Doctors doctors, IFormFile DoctorImage)
        {
            if (id != doctors.Id)
            {
                return NotFound();
            }

            try
            {
                // Get the existing data of the doctor from the database
                var existingDoctor = await _context.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
                if (existingDoctor == null)
                {
                    return NotFound();
                }

                // Save the image
                if (DoctorImage != null && DoctorImage.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", DoctorImage.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await DoctorImage.CopyToAsync(stream);
                    }
                    // Save the image path in the database
                    doctors.ImagePath = "/images/" + DoctorImage.FileName;
                }
                else
                {
                    // If no new emage was set return the old one
                    doctors.ImagePath = existingDoctor.ImagePath;
                }
                _context.Update(doctors);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorsExists(doctors.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctors = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctors == null)
            {
                return NotFound();
            }

            return View(doctors);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctors = await _context.Doctors.FindAsync(id);
            if (doctors != null)
            {
                _context.Doctors.Remove(doctors);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorsExists(int id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
