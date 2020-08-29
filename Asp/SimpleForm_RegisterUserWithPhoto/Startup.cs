using FileSignatureUtility;
using FileSignatureUtility.Services;
using FunctionalUtility.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleForm_RegisterUserWithPhoto.Interfaces;
using SimpleForm_RegisterUserWithPhoto.Models.Configs;
using SimpleForm_RegisterUserWithPhoto.Services;

namespace SimpleForm_RegisterUserWithPhoto {
    public class Startup {
        public Startup (IConfiguration configuration, IWebHostEnvironment env) {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddDatabase (Configuration, _env.IsDevelopment ())
                .AddScoped<IPersons, PersonsService> ()
                .AddSingleton<IFileSignature> (new InMemoryService ("Data.json"))
                .AddControllersWithViews ();

            Configuration.AddConfig<ProfileImageSetting> (services)
                .ThrowExceptionOnFail ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app) {
            if (_env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseExceptionHandler ("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }
            app.UseHttpsRedirection ();
            app.UseStaticFiles ();

            app.UseRouting ();

            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllerRoute (
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}