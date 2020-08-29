using System.Collections.Generic;
using System.Threading.Tasks;
using FunctionalUtility.ResultUtility;
using SimpleForm_RegisterUserWithPhoto.Models;
using SimpleForm_RegisterUserWithPhoto.ViewModels;

namespace SimpleForm_RegisterUserWithPhoto.Interfaces {
    public interface IPersons {
        Task<List<Person>> GetAllAsync ();
        Task<Person?> GetAsync (string id);
        Task<MethodResult> CreateAsync (PersonViewModel person);
        Task<MethodResult> UpdateAsync (string id, PersonViewModel personViewModel);
        Task<MethodResult> DeleteAsync (string id);
        string GetProfilePath (string id);
    }
}