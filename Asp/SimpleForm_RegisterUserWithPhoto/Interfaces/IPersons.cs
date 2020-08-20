using System.Collections.Generic;
using System.Threading.Tasks;
using FunctionalUtility.ResultUtility;
using SimpleForm_RegisterUserWithPhoto.Models;
using SimpleForm_RegisterUserWithPhoto.ViewModels;

namespace SimpleForm_RegisterUserWithPhoto.Interfaces {
    public interface IPersons {
        Task<List<Person>> GetAllAsync ();
        Task<Person?> GetAsync (int id);
        Task CreateAsync (PersonViewModel person);
        Task<MethodResult> UpdateAsync (int id, Person person);
        Task<MethodResult> DeleteAsync (int id);
    }
}