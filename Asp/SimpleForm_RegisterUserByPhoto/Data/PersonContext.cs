using Microsoft.EntityFrameworkCore;

namespace SimpleForm_RegisterUserByPhoto.Data {
    public class PersonContext : DbContext {
        public PersonContext (
            DbContextOptions<PersonContext> options) : base (options) { }

        public DbSet<Models.Person> Person { get; set; }
        public DbSet<Models.Gender> Gender { get; set; }
    }
}