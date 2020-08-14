using System;
using System.Linq;
using System.Threading.Tasks;
using FunctionalUtility.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsValidation.Utility;
using SimpleForm_RegisterUserWithPhoto.Data;
using SimpleForm_RegisterUserWithPhoto.Models;

namespace SimpleForm_RegisterUserWithPhoto.Controllers {
    public class PersonsController : Controller {
        private readonly PersonContext _context;

        public PersonsController (PersonContext context) {
            _context = context;
        }

        // GET: Persons
        public async Task<IActionResult> Index () {
            return View (await _context.Person.ToListAsync ());
        }

        // GET: Persons/Details/5
        public async Task<IActionResult> Details (int? id) {
            if (id == null) {
                return NotFound ();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync (m => m.Id == id);
            if (person == null) {
                return NotFound ();
            }

            return View (person);
        }

        // GET: Persons/Create
        public IActionResult Create () {
            return View ();
        }

        // POST: Persons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create ([Bind ("Id,Name,Family,Phone,Age,Description,Agreement")] Person person) {
            if (ModelState.IsValid) {
                var standardPhone = PhoneUtility.GetPhone (person.Phone).ThrowExceptionOnFail ();
                person.Phone = standardPhone.Value;

                person.RegisterDateTime = DateTime.UtcNow;
                _context.Add (person);
                await _context.SaveChangesAsync ();
                return RedirectToAction (nameof (Index));
            }
            return View (person);
        }

        // GET: Persons/Edit/5
        public async Task<IActionResult> Edit (int? id) {
            if (id == null) {
                return NotFound ();
            }

            var person = await _context.Person.FindAsync (id);
            if (person == null) {
                return NotFound ();
            }
            return View (person);
        }

        // POST: Persons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id, [Bind ("Id,Name,Family,Phone,Age,Description,Agreement")] Person person) {
            if (id != person.Id) {
                return NotFound ();
            }

            if (ModelState.IsValid) {
                try {
                    var registerDataTime = (await _context.Person
                        .Where (p => p.Id == id).AsNoTracking ().FirstOrDefaultAsync ())?.RegisterDateTime;
                    if (registerDataTime is null)
                        return NotFound ();
                    person.RegisterDateTime = (DateTime) registerDataTime;

                    var standardPhone = PhoneUtility.GetPhone (person.Phone).ThrowExceptionOnFail ();
                    person.Phone = standardPhone.Value;

                    _context.Update (person);
                    await _context.SaveChangesAsync ();
                } catch (DbUpdateConcurrencyException) {
                    if (!PersonExists (person.Id)) {
                        return NotFound ();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction (nameof (Index));
            }
            return View (person);
        }

        // GET: Persons/Delete/5
        public async Task<IActionResult> Delete (int? id) {
            if (id == null) {
                return NotFound ();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync (m => m.Id == id);
            if (person == null) {
                return NotFound ();
            }

            return View (person);
        }

        // POST: Persons/Delete/5
        [HttpPost, ActionName ("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (int id) {
            var person = await _context.Person.FindAsync (id);
            _context.Person.Remove (person);
            await _context.SaveChangesAsync ();
            return RedirectToAction (nameof (Index));
        }

        private bool PersonExists (int id) {
            return _context.Person.Any (e => e.Id == id);
        }
    }
}