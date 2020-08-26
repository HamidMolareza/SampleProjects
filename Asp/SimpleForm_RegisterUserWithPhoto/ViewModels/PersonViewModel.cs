using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using ModelsValidation.Attributes;
using SimpleForm_RegisterUserWithPhoto.Utility;

namespace SimpleForm_RegisterUserWithPhoto.ViewModels {
    public class PersonViewModel {
        public PersonViewModel () {
            Id = MyGuid.Generate ();
        }

        public string Id { get; set; }

        [Required]
        [StringLength (35)]
        public string Name { get; set; } = null!;

        [StringLength (35)]
        public string? Family { get; set; }

        [Required]
        [PhoneNumber]
        public string Phone { get; set; } = null!;

        [Range (5, 120)]
        public int Age { get; set; }
        public string? Description { get; set; }

        [Agreement]
        public bool Agreement { get; set; }

        [Display (Name = "Register Date")]
        public DateTime RegisterDateTime { get; set; }

        [Display (Name = "Profile Photo")]
        public IFormFile? ProfilePhoto { get; set; }
    }
}