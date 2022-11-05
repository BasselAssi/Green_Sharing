using GreenSharingAPI.Models;

using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;

namespace GreenSharingAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var executable = Assembly.GetExecutingAssembly().Location;
            var path = Path.GetDirectoryName(executable);
            AppDomain.CurrentDomain.SetData("Data", path);

            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var builder = new DbContextOptionsBuilder<GreenSharingContext>();
            var connectionString = configuration?.GetSection("ConnectionStrings")?["GreenSharingDatabase"];

            services.AddDbContext<GreenSharingContext>(options => options.UseSqlServer(connectionString));
            //services.AddDbContext<GreenSharingContext>(opt => opt.UseInMemoryDatabase("GreenSharing"));

            services.AddControllers();
            services.AddSwaggerDocument();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "GreenSharingAPI", Version = "v1" });
                c.OperationFilter<SwashbuckleOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            ApplyMigration<GreenSharingContext>(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();
            app.UseSwaggerUI();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GreenSharingAPI v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ApplyMigration<DbContextType>(IApplicationBuilder application)
        {
            // Migrate and seed the database during startup. Must be synchronous.
            try
            {
                using (var serviceScope = application.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var dbContext = (DbContext)serviceScope.ServiceProvider.GetRequiredService(typeof(DbContextType));

                    dbContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
               // I'm using Serilog here, but use the logging solution of your choice.
               // Logger.Error("Failed to migrate or seed database", ex);
            }
        }
    }
}