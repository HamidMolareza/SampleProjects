using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleForm_RegisterUserByPhoto.Data;

namespace SimpleForm_RegisterUserByPhoto.Services {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddDatabase (
                this IServiceCollection services, IConfiguration configuration, bool isDevelopment) =>
            isDevelopment ?
            services.AddDbContext<PersonContext> (option => option.UseInMemoryDatabase ("PersonDb")) :
            services.AddDbContext<PersonContext> (options =>
                options.UseSqlServer (
                    configuration.GetConnectionString ("PersonContext")));
    }
}