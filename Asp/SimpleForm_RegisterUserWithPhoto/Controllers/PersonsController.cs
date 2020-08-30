using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunctionalUtility.Extensions;
using Microsoft.AspNetCore.Mvc;
using ModelsValidation.Extensions;
using SimpleForm_RegisterUserWithPhoto.Interfaces;
using SimpleForm_RegisterUserWithPhoto.Models;
using SimpleForm_RegisterUserWithPhoto.Models.Configs;
using SimpleForm_RegisterUserWithPhoto.ViewModels;

namespace SimpleForm_RegisterUserWithPhoto.Controllers {
    public class PersonsController : Controller {
        private readonly IPersons _personsService;
        private readonly ProfileImageSetting _profileImageSetting;

        public PersonsController (IPersons personsService, ProfileImageSetting profileImageSetting) {
            _personsService = personsService;
            _profileImageSetting = profileImageSetting;
        }

        // GET: Persons
        public async Task<IActionResult> Index () {
            var persons = await _personsService.GetAllAsync ();
            var personsViewModel = MapToViewModel (persons);
            return View (personsViewModel);
        }

        // GET: Persons/Details/5
        public async Task<IActionResult> Details (string? id) {
            if (id == null)
                return NotFound ();

            var person = await _personsService.GetAsync (id);
            if (person == null)
                return NotFound ();

            return View (MapToViewModel (person));
        }

        // GET: Persons/Create
        public IActionResult Create () {
            ViewData["ImageValidTypes"] = _profileImageSetting.ValidTypesStr;
            return View ();
        }

        // POST: Persons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (
            [Bind ("Id,Name,Family,Phone,Age,Description,Agreement,ProfilePhotoFile")] PersonViewModel personViewModel) {
            ViewData["ImageValidTypes"] = _profileImageSetting.ValidTypesStr;
            if (!ModelState.IsValid)
                return View (personViewModel);

            var createResult = await _personsService.CreateAsync (personViewModel);
            if (createResult.IsSuccess)
                return RedirectToAction (nameof (Index));

            ModelState.AddMethodResultError (createResult.Detail);
            return View (personViewModel);
        }

        // GET: Persons/Edit/5
        public async Task<IActionResult> Edit (string? id) {
            if (id == null)
                return NotFound ();

            var person = await _personsService.GetAsync (id);
            if (person == null)
                return NotFound ();

            var personViewModel = MapToViewModel (person);

            ViewData["ImageValidTypes"] = _profileImageSetting.ValidTypesStr;
            return View (personViewModel);
        }

        // POST: Persons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (string id, [Bind ("Id,Name,Family,Phone,Age,Description,Agreement,ProfilePhotoFile")] PersonViewModel personViewModel) {
            if (id != personViewModel.Id)
                return NotFound ();

            ViewData["ImageValidTypes"] = _profileImageSetting.ValidTypesStr;
            if (!ModelState.IsValid)
                return View (personViewModel);

            var updateResult = await _personsService.UpdateAsync (id, personViewModel);
            if (updateResult.IsSuccess) return RedirectToAction (nameof (Index));

            if (updateResult.IsNotFoundError ())
                return NotFound ();

            ModelState.AddMethodResultError (updateResult.Detail);
            return View (personViewModel);
        }

        // GET: Persons/Delete/5
        public async Task<IActionResult> Delete (string? id) {
            if (id == null)
                return NotFound ();

            var person = await _personsService.GetAsync (id);
            if (person == null)
                return NotFound ();

            return View (MapToViewModel (person));
        }

        // POST: Persons/Delete/5
        [HttpPost, ActionName ("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string id) {
            var result = await _personsService.DeleteAsync (id);
            if (!result.IsSuccess) {
                if (result.IsNotFoundError ())
                    return NotFound ();
                result.ThrowExceptionOnFail ();
            }

            return RedirectToAction (nameof (Index));
        }

        private PersonViewModel MapToViewModel (Person person) =>
            new PersonViewModel {
                Age = person.Age,
                Agreement = person.Agreement,
                Description = person.Description,
                Family = person.Family,
                Id = person.Id,
                Name = person.Name,
                Phone = person.Phone,
                RegisterDateTime = person.RegisterDateTime,
                ProfileUrl = person.ProfileImageName != null ?
                Url.ActionLink (nameof (DownloadController.Get),
                DownloadController.ControllerName, new { id = person.ProfileImageName }) : Url.Content ("~/NoImage.jpg")
            };

        private List<PersonViewModel> MapToViewModel (IEnumerable<Person> persons) =>
            persons.Select (MapToViewModel).ToList ();
    }
}