using System.Threading.Tasks;
using FunctionalUtility.Extensions;
using Microsoft.AspNetCore.Mvc;
using SimpleForm_RegisterUserWithPhoto.Interfaces;
using SimpleForm_RegisterUserWithPhoto.Models;
using SimpleForm_RegisterUserWithPhoto.ViewModels;

namespace SimpleForm_RegisterUserWithPhoto.Controllers {
    public class PersonsController : Controller {
        private readonly IPersons _personsService;

        public PersonsController (IPersons personsService) {
            _personsService = personsService;
        }

        // GET: Persons
        public async Task<IActionResult> Index () =>
            View (await _personsService.GetAllAsync ());

        // GET: Persons/Details/5
        public async Task<IActionResult> Details (int? id) {
            if (id == null)
                return NotFound ();

            var person = await _personsService.GetAsync ((int) id);
            if (person == null)
                return NotFound ();

            return View (person);
        }

        // GET: Persons/Create
        public IActionResult Create () => View ();

        // POST: Persons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (
            [Bind ("Id,Name,Family,Phone,Age,Description,Agreement,ProfilePhoto")] PersonViewModel personViewModel) {
            if (!ModelState.IsValid)
                return View (personViewModel);

            await _personsService.CreateAsync (personViewModel);
            return RedirectToAction (nameof (Index));
        }

        // GET: Persons/Edit/5
        public async Task<IActionResult> Edit (int? id) {
            if (id == null)
                return NotFound ();

            var person = await _personsService.GetAsync ((int) id);
            if (person == null)
                return NotFound ();

            return View (MapToViewModel(person));
        }

        // POST: Persons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id, [Bind ("Id,Name,Family,Phone,Age,Description,Agreement,ProfilePhoto")] PersonViewModel personViewModel) {
            if (id != personViewModel.Id)
                return NotFound ();

            if (!ModelState.IsValid)
                return View (personViewModel);

            var result = await _personsService.UpdateAsync (id, personViewModel);
            if (!result.IsSuccess) {
                if (result.IsNotFoundError ())
                    return NotFound ();
                result.ThrowExceptionOnFail ();
            }

            return RedirectToAction (nameof (Index));
        }

        // GET: Persons/Delete/5
        public async Task<IActionResult> Delete (int? id) {
            if (id == null)
                return NotFound ();

            var person = await _personsService.GetAsync ((int) id);
            if (person == null)
                return NotFound ();

            return View (person);
        }

        // POST: Persons/Delete/5
        [HttpPost, ActionName ("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (int id) {
            var result = await _personsService.DeleteAsync (id);
            if (!result.IsSuccess) {
                if (result.IsNotFoundError ())
                    return NotFound ();
                result.ThrowExceptionOnFail ();
            }

            return RedirectToAction (nameof (Index));
        }

        private static PersonViewModel MapToViewModel(Person person) =>
            new PersonViewModel
            {
                Age = person.Age,
                Agreement = person.Agreement,
                Description = person.Description,
                Family = person.Family,
                Id = person.Id,
                Name = person.Name,
                Phone = person.Phone,
                RegisterDateTime = person.RegisterDateTime
            };
    }
}