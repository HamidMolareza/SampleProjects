using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleForm_RegisterUserWithPhoto.Data;

namespace SimpleForm_RegisterUserWithPhoto.Models {
    public static class SeedData {
        public static void Initialize (IServiceProvider serviceProvider) {
            using var context = new PersonContext (
                serviceProvider.GetRequiredService<DbContextOptions<PersonContext>> ());

            // Look for any movies.
            if (context.Person.Any ()) {
                return; // DB has been seeded
            }

            context.Person.AddRange (
                new Person {
                    Id = "02d43d9bbbed47c6978b3146f945bcca",
                        Name = "Ali",
                        Family = "Majidi",
                        Phone = "+989100000000",
                        Age = 21,
                        Agreement = true,
                        RegisterDateTime = DateTime.Now.AddDays (-1)
                },
                new Person {
                    Id = "85cf54ae7a464b2b8ed962cd33e13a3e",
                        Name = "Mohammad",
                        Family = "Zamani",
                        Phone = "+989100000000",
                        Age = 23,
                        Agreement = true,
                        Description = "This is a description.",
                        RegisterDateTime = DateTime.Now.AddDays (-2)
                },
                new Person {
                    Id = "89b54b718ec9473e9ef13b36caffedc2",
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