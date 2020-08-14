using Microsoft.EntityFrameworkCore;

namespace SimpleForm_RegisterUserWithPhoto.Data {
    public class PersonContext : DbContext {
        public PersonContext (
            DbContextOptions<PersonContext> options) : base (options) { }

        public DbSet<Models.Person> Person { get; set; } = null!;
    }
}