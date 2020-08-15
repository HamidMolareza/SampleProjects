using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunctionalUtility.Extensions;
using FunctionalUtility.ResultDetails.Errors;
using FunctionalUtility.ResultUtility;
using Microsoft.EntityFrameworkCore;
using ModelsValidation.Utility;
using SimpleForm_RegisterUserWithPhoto.Data;
using SimpleForm_RegisterUserWithPhoto.Interfaces;
using SimpleForm_RegisterUserWithPhoto.Models;

namespace SimpleForm_RegisterUserWithPhoto.Services {
    public class PersonsService : IPersons {
        private readonly PersonContext _context;

        public PersonsService (PersonContext context) {
            _context = context;
        }

        public Task<List<Person>> GetAllAsync () =>
            _context.Person.ToListAsync ();

        public Task<Person?> GetAsync (int id) =>
            _context.Person.FirstOrDefaultAsync (m => m.Id == id) !;

        public async Task CreateAsync (Person person) {
            var standardPhone = PhoneUtility.GetPhone (person.Phone).ThrowExceptionOnFail ();
            person.Phone = standardPhone.Value;

            person.RegisterDateTime = DateTime.UtcNow;
            _context.Add (person);
            await _context.SaveChangesAsync ();
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
    }
}