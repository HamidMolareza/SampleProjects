using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileSignatureUtility;
using FunctionalUtility.Extensions;
using FunctionalUtility.ResultDetails.Errors;
using FunctionalUtility.ResultUtility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelsValidation;
using ModelsValidation.ResultDetails;
using ModelsValidation.Utility;
using SimpleForm_RegisterUserWithPhoto.Data;
using SimpleForm_RegisterUserWithPhoto.Interfaces;
using SimpleForm_RegisterUserWithPhoto.Models;
using SimpleForm_RegisterUserWithPhoto.Models.Configs;
using SimpleForm_RegisterUserWithPhoto.ViewModels;

namespace SimpleForm_RegisterUserWithPhoto.Services {
    public class PersonsService : IPersons {
        private readonly PersonContext _context;
        private readonly string _filePath = Path.Combine (Directory.GetCurrentDirectory (), "SecureRoot", "Persons"); //TODO: ***
        private readonly IFileSignature _fileSignatureService;
        private readonly ProfileImageSetting _profileImageSetting;

        public PersonsService (PersonContext context,
            IFileSignature fileSignatureService, ProfileImageSetting profileImageSetting) {
            _context = context;
            _fileSignatureService = fileSignatureService;
            _profileImageSetting = profileImageSetting;
        }

        public Task<List<Person>> GetAllAsync () =>
            _context.Person.ToListAsync ();

        public Task<Person?> GetAsync (string id) =>
            _context.Person.FirstOrDefaultAsync (m => m.Id == id) !;

        private async Task<MethodResult> ValidatePersonViewModel (PersonViewModel personViewModel) {
            var modelValidationResult = Method.MethodParametersMustValid (
                new [] { personViewModel }, showDefaultMessageToUser : false);
            if (!modelValidationResult.IsSuccess)
                return modelValidationResult;

            if (personViewModel.ProfilePhotoFile != null) {
                var fileValidationResult = await FileValidationAsync (
                    personViewModel.ProfilePhotoFile, nameof (personViewModel.ProfilePhotoFile));
                if (!fileValidationResult.IsSuccess)
                    return MethodResult.Fail (fileValidationResult.Detail);
            }

            return MethodResult.Ok ();
        }

        public Task<MethodResult> CreateAsync (PersonViewModel personViewModel) =>
            TryExtensions.TryAsync (async () => {
                var validationResult = await ValidatePersonViewModel (personViewModel);
                if (!validationResult.IsSuccess)
                    return validationResult;

                var standardPhoneResult = PhoneUtility.GetPhone (personViewModel.Phone);
                if (!standardPhoneResult.IsSuccess)
                    return MethodResult.Fail (standardPhoneResult.Detail);

                personViewModel.Phone = standardPhoneResult.Value;

                personViewModel.RegisterDateTime = DateTime.UtcNow;
                await _context.Person.AddAsync (MapToMainModel (personViewModel));

                if (personViewModel.ProfilePhotoFile != null) {
                    var processResult = await ProcessFile (personViewModel.ProfilePhotoFile, personViewModel.Id);
                    if (!processResult.IsSuccess)
                        return processResult;
                }

                await _context.SaveChangesAsync ();
                return MethodResult.Ok ();
            }, 1);

        private Task<MethodResult> ProcessFile (IFormFile file, string id) =>
            TryExtensions.TryAsync (async () => {
                Directory.CreateDirectory (_filePath);
                await using var stream = File.Create (GetProfilePath (id));
                await file.CopyToAsync (stream);
                return MethodResult.Ok ();
            });

        private Task<MethodResult<bool>> IsFileValidAsync (Stream fileStream, string[] validFiles) =>
            new Validation (_fileSignatureService)
            .TryMapAsync (fileValidation => fileValidation.ValidateAsync (fileStream, validFiles));

        private async Task<MethodResult> FileValidationAsync (IFormFile file, string parameterName) {
            if (file is null)
                return MethodResult.Fail (new BadRequestError (message: $"{nameof(file)} is null."));
            return await IsFileValidAsync (file!.OpenReadStream (),
                    _profileImageSetting.ValidTypes)
                .OnSuccessFailWhenAsync (isValid => !isValid,
                    new ArgumentValidationError (new KeyValuePair<string, string> (
                        parameterName, "The file type is not valid.")))
                .MapMethodResultAsync ();
        }

        public async Task<MethodResult> UpdateAsync (string id, PersonViewModel personViewModel) {
            var validationResult = await ValidatePersonViewModel (personViewModel);
            if (!validationResult.IsSuccess)
                return validationResult;

            var registerDataTime = (await _context.Person
                .Where (p => p.Id == id)
                .AsNoTracking ()
                .FirstOrDefaultAsync ())?.RegisterDateTime;

            if (registerDataTime is null)
                return MethodResult.Fail (new NotFoundError ());

            personViewModel.RegisterDateTime = (DateTime) registerDataTime;
            var standardPhone = PhoneUtility.GetPhone (personViewModel.Phone).ThrowExceptionOnFail ();
            personViewModel.Phone = standardPhone.Value;

            _context.Person.Update (MapToMainModel (personViewModel));

            if (personViewModel.ProfilePhotoFile != null) {
                var processResult = await ProcessFile (personViewModel.ProfilePhotoFile, personViewModel.Id);
                if (!processResult.IsSuccess)
                    return processResult;
            }

            await _context.SaveChangesAsync ();
            return MethodResult.Ok ();
        }

        public async Task<MethodResult> DeleteAsync (string id) {
            var person = await _context.Person.FindAsync (id);
            if (person is null)
                return MethodResult.Fail (new NotFoundError ());

            _context.Person.Remove (person);
            await _context.SaveChangesAsync ();
            return MethodResult.Ok ();
        }

        public string GetProfilePath (string id) =>
            Path.Combine (_filePath, id);

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