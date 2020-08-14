using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleForm_RegisterUserWithPhoto.Data;

namespace SimpleForm_RegisterUserWithPhoto.Models {
    public static class SeedData {
        public static void Initialize (IServiceProvider serviceProvider) {
            using (var context = new PersonContext (
                serviceProvider.GetRequiredService<DbContextOptions<PersonContext>> ())) {
                // Look for any movies.
                if (context.Person.Any ()) {
                    return; // DB has been seeded
                }

                context.Person.AddRange (
                    new Person {
                        Name = "Ali",
                            Family = "Majidi",
                            Phone = "+989100000000",
                            Age = 21,
                            Agreement = true,
                            RegisterDateTime = DateTime.Now.AddDays (-1)
                    },

                    new Person {
                        Name = "Mohammad",
                            Family = "Zamani",
                            Phone = "+989100000000",
                            Age = 23,
                            Agreement = true,
                            Description = "This is a description.",
                            RegisterDateTime = DateTime.Now.AddDays (-2)
                    },
                    new Person {
                        Name = "Javad",
                            Family = "Javadi",
                            Phone = "+989100000000",
                            Age = 21,
                            Agreement = true,
                            RegisterDateTime = DateTime.Now.AddDays (-3)
                    }
                );
                context.SaveChanges ();
            }
        }
    }
}