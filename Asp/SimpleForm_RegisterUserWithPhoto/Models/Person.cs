using System;
using System.ComponentModel.DataAnnotations;
using ModelsValidation.Attributes;

namespace SimpleForm_RegisterUserWithPhoto.Models {
    public class Person {
        public string Id { get; set; } = null!;

        [Required]
        [StringLength (35)]
        public string Name { get; set; } = null!;

        [StringLength (35)]
        public string? Family { get; set; }

        [Required]
        [PhoneNumber]
        [Phone]
        [DataType (DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [Range (5, 120)]
        public int Age { get; set; }
        public string? Description { get; set; }

        [Agreement]
        public bool Agreement { get; set; }

        [Display (Name = "Register Date")]
        public DateTime RegisterDateTime { get; set; }

        public string? ProfileImageName { get; set; }
    }
}