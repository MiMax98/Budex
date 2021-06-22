using BuildingMaterialRent.Data;
using BuildingMaterialRent.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BuildingMaterialRent
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BuildingMaterialRent", Version = "v1" });
            });

            services.AddDbContext<RentDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();


            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 4;
            }).AddEntityFrameworkStores<RentDbContext>();

            services.AddAuthentication();

            services.ConfigureApplicationCookie(o =>
            {
                o.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = (ctx) =>
                    {
                        ctx.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = (ctx) =>
                    {
                        ctx.Response.StatusCode = 403;
                        return Task.CompletedTask;
                    }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BuildingMaterialRent v1"));
            }

            app.UseHttpsRedirection();

            app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/js"), GetStaticFilesHandler("/js", "js", env));
            app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/css"), GetStaticFilesHandler("/css", "css", env));
            app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/img"), GetStaticFilesHandler("/img", "img", env));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            var folder = Path.Combine(env.WebRootPath, "html");
            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/app/{fileName}/{id?}", async ctx =>
                {
                    var fileName = ctx.Request.RouteValues["fileName"] as string;
                    ctx.Response.ContentType = "text/html;charset=utf-8";
                    await ctx.Response.SendFileAsync(Path.Combine(folder, $"{fileName}.html"));
                });
                endpoints.MapControllers();
            });
        }

        private static Action<IApplicationBuilder> GetStaticFilesHandler(string path, string folder, IWebHostEnvironment env)
        {
            var rootPath = env.WebRootPath;
            return app =>
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    RequestPath = path,
                    FileProvider = new PhysicalFileProvider(Path.Combine(rootPath, folder))
                });
            };
        }
    }
}
