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
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Patients.Include(p => p.Doctors);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Patients/SearchPatients
        public async Task<IActionResult> SearchPatients()
        {
            return View("SearchPatients");
        }

        // GET: Patients/ShowResult
        public async Task<IActionResult> ShowResult(string SSN, string LastName, string Diagnosis)
        {
            var query = _context.Patients.Include(p => p.Doctors).AsQueryable();

            // Apply filters only if values are provided
            if (!string.IsNullOrWhiteSpace(SSN))
            {
                query = query.Where(p => p.SSN.ToString().Contains(SSN));
            }

            if (!string.IsNullOrWhiteSpace(LastName))
            {
                query = query.Where(p => p.LastName.Contains(LastName));
            }

            if (!string.IsNullOrWhiteSpace(Diagnosis))
            {
                query = query.Where(p => p.Diagnosis.Contains(Diagnosis));
            }

            var result = await query.ToListAsync();

            // Return the result to the view
            return View("Index", result);
        }


        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patients = await _context.Patients
                .Include(p => p.Doctors)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patients == null)
            {
                return NotFound();
            }

            return View(patients);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            ViewBag.DoctorsId = new SelectList(_context.Doctors
                .Select(d => new { d.Id, FullName = "Dr. " + d.FirstName + " " + d.LastName }),
                "Id", "FullName");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SSN,FirstName,LastName,Meds,Diagnosis,DoctorsId")] Patients patients)
        {
           
            _context.Add(patients);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patients = await _context.Patients.FindAsync(id);
            if (patients == null)
            {
                return NotFound();
            }
            ViewBag.DoctorsId = new SelectList(_context.Doctors
                .Select(d => new { d.Id, FullName = "Dr. " + d.FirstName + " " + d.LastName }),
                "Id", "FullName", patients.DoctorsId);
            return View(patients);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SSN,FirstName,LastName,Meds,Diagnosis,DoctorsId")] Patients patients)
        {
            if (id != patients.Id)
            {
                return NotFound();
            }
            try
            {
                _context.Update(patients);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientsExists(patients.Id))
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

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patients = await _context.Patients
                .Include(p => p.Doctors)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patients == null)
            {
                return NotFound();
            }

            return View(patients);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patients = await _context.Patients.FindAsync(id);
            if (patients != null)
            {
                _context.Patients.Remove(patients);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientsExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
