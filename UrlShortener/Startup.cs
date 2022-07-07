using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using UrlShortener.Api.Configurations;
using UrlShortener.Api.Infrastructure;
using UrlShortener.DataAccess;
using UrlShortener.Services;

namespace UrlShortener
{
    public record Startup(IConfiguration Configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwagger();
            services.AddTransient<UrlService>();
            services.AddTransient<UserService>();
            services.AddDbContext<SqlContext>(x => x.UseSqlServer(Configuration.GetConnectionString("SqlContext")));
            services.AddJWTAuthorization(Configuration);
            services.AddAutoMapper(typeof(ApiMappingProfile));
            services.AddCors(o =>
               o.AddDefaultPolicy(b =>
                 b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseCustomSwagger();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
