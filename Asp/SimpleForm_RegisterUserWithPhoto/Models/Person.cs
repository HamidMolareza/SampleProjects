using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleForm_RegisterUserWithPhoto.Models {
    public class Person {
        public int Id { get; set; }

        [StringLength (35)]
        [Required]
        public string Name { get; set; }

        [StringLength (35)]
        public string Family { get; set; }

        [Range (5, 120)]
        public int Age { get; set; }
        public string Description { get; set; }

        [Utility.Attribute.Agreement]
        public bool Agreement { get; set; }

        [Display (Name = "Register Date")]
        public DateTime RegisterDateTime { get; set; }
    }
}