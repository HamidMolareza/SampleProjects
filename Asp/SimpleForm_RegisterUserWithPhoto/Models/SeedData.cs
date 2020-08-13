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
                            Age = 21,
                            Agreement = true,
                            RegisterDateTime = new DateTime (2020, 08, 14, 3, 20, 12)
                    },

                    new Person {
                        Name = "Mohammad",
                            Family = "Zamani",
                            Age = 23,
                            Agreement = true,
                            Description = "This is a description.",
                            RegisterDateTime = new DateTime (2020, 07, 12, 1, 20, 12)
                    },
                    new Person {
                        Name = "Javad",
                            Family = "Javadi",
                            Age = 21,
                            Agreement = true,
                            RegisterDateTime = new DateTime (2020, 01, 12, 3, 10, 1)
                    }
                );
                context.SaveChanges ();
            }
        }
    }
}