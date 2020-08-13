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
        public bool Agreement { get; set; }
        public DateTime RegisterDateTime { get; set; }
    }
}