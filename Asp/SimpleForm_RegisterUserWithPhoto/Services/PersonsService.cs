using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FunctionalUtility.Extensions;
using FunctionalUtility.ResultDetails.Errors;
using FunctionalUtility.ResultUtility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelsValidation.Utility;
using SimpleForm_RegisterUserWithPhoto.Data;
using SimpleForm_RegisterUserWithPhoto.Interfaces;
using SimpleForm_RegisterUserWithPhoto.Models;
using SimpleForm_RegisterUserWithPhoto.ViewModels;

namespace SimpleForm_RegisterUserWithPhoto.Services {
    public class PersonsService : IPersons {
        private readonly PersonContext _context;
        private readonly string _filePath = Path.Combine (Directory.GetCurrentDirectory (), "SecureRoot");

        public PersonsService (PersonContext context) {
            _context = context;
        }

        public Task<List<Person>> GetAllAsync () =>
            _context.Person.ToListAsync ();

        public Task<Person?> GetAsync (int id) =>
            _context.Person.FirstOrDefaultAsync (m => m.Id == id) !;

        public async Task CreateAsync (PersonViewModel personViewModel) {
            var standardPhone = PhoneUtility.GetPhone (personViewModel.Phone).ThrowExceptionOnFail ();
            personViewModel.Phone = standardPhone.Value;

            personViewModel.RegisterDateTime = DateTime.UtcNow;
            await _context.Person.AddAsync (MapToMainModel (personViewModel));

            if (personViewModel.ProfilePhoto != null)
                await ProcessFile (personViewModel.ProfilePhoto, personViewModel.Id);

            await _context.SaveChangesAsync ();
        }

        private async Task ProcessFile (IFormFile file, int id) {
            await using var stream = File.Create (Path.Combine (_filePath, id.ToString ()));
            await file.CopyToAsync (stream);
        }

        public async Task<MethodResult> UpdateAsync (int id, Person person) {
            var registerDataTime = (await _context.Person
                .Where (p => p.Id == id)
                .AsNoTracking ()
                .FirstOrDefaultAsync ())?.RegisterDateTime;

            if (registerDataTime is null)
                return MethodResult.Fail (new NotFoundError ());

            person.RegisterDateTime = (DateTime) registerDataTime;
            var standardPhone = PhoneUtility.GetPhone (person.Phone).ThrowExceptionOnFail ();
            person.Phone = standardPhone.Value;

            _context.Update (person);
            await _context.SaveChangesAsync ();
            return MethodResult.Ok ();
        }

        public async Task<MethodResult> DeleteAsync (int id) {
            var person = await _context.Person.FindAsync (id);
            if (person is null)
                return MethodResult.Fail (new NotFoundError ());

            _context.Person.Remove (person);
            await _context.SaveChangesAsync ();
            return MethodResult.Ok ();
        }

        private static Person MapToMainModel (PersonViewModel personViewModel) =>
            new Person {
                Age = personViewModel.Age,
                Agreement = personViewModel.Agreement,
                Description = personViewModel.Description,
                Family = personViewModel.Family,
                Id = personViewModel.Id,
                Name = personViewModel.Name,
                Phone = personViewModel.Phone,
                RegisterDateTime = personViewModel.RegisterDateTime
            };
    }
}