using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Tulip_API.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Tulip_API.Contracts;
using Tulip_API.Services;
using AutoMapper;
using Tulip_API.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Tulip_API
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()//options => options.SignIn.RequireConfirmedAccount = true) for email confirmation -- Iyad
                .AddRoles<IdentityRole>() //-- Iyad
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Configure CORS "Cross Origin Resource Sharing" (for global interaction)- Iyad
            services.AddCors(o =>
            {
                o.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            // Setup AutoMapper -- Iyad
            services.AddAutoMapper(typeof(Maps));

            // Configure JWT -- Iyad
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });



            // Add Swagger  -- Iyad
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { Title="Tulip Clothing Store API",
                    Version= "v1",
                    Description = "This is an educational API for Tulip Clothing Store"
                });

                // Adding XML file to swagger
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //D:\C#Projects\Tulip\Tulip-API\Tulip-API.xml (API Properties -> Build)
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Setup nLog system
            services.AddSingleton<ILoggerService, LoggerService>();

            // Add Category Repository and Service to DI -- Iyad
            services.AddScoped<ICategoryRepository, CategoryRepository>(); 
            
            // Add Product Repository and Service to DI -- Iyad
            services.AddScoped<IProductRepository, ProductRepository>();

            // change from AddRazorPages to AddControllers - Iyad
            //services.AddRazorPages();
            //services.AddControllers();
            // After installing 'Microsoft.AspNetCore.Mvc.NewtonsoftJson' package
            // to include related records (example -> Category and its products)
            services.AddControllers().AddNewtonsoftJson(options =>
            
                options.SerializerSettings.ReferenceLoopHandling
                = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger(); // - Iyad
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tulip Clothing Store API");
                c.RoutePrefix = "";
            });

            app.UseHttpsRedirection();
            //app.UseStaticFiles(); // we don't need static files like js/css in the API, 
            // becasue the API doesn't have an interface - Iyad
            // Delete the wwwroot, Areas and Pages folders

            // Configure CORS "Cross Origin Resource Sharing" - Iyad
            app.UseCors("CorsPolicy");

            // Add admin user and roles
            SeedData.Seed(userManager, roleManager).Wait();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages(); 
                endpoints.MapControllers(); // -- Iyad
            });
        }
    }
}
