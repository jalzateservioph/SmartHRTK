using AutoMapper;
using BiometricsIntegrationWebAPI.Helpers;
using BiometricsIntegrationWebAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using BiometricsIntegrationWebAPI.Helpers;
namespace BiometricsIntegrationWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvc();

            services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            //services.AddDbContext<ApplicationContext>(item => item.UseSqlServer(Configuration.GetConnectionString("con")));
            services.AddDbContext<TKProcessor.Contexts.TKContext>(item => item.UseSqlServer(Configuration.GetConnectionString("tk")));
            services.AddAutoMapper(typeof(MapperProfile).GetTypeInfo().Assembly);
            services.AddScoped<TKAuthService, TKAuthService>();
            services.AddScoped<EmployeeService, EmployeeService>();
            services.AddScoped<WorkSiteService, WorkSiteService>();
            services.AddScoped<RawDataService, RawDataService>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        
        private static void UpdateDatabase<T>(IApplicationBuilder app)
           where T : DbContext
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                                                            .CreateScope();

            using var context = serviceScope.ServiceProvider.GetService<T>();

            context.Database.Migrate();
        }
    }
}
