using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tulip_BlazorUI.Contratcs;
using Tulip_BlazorUI.Providers;
using Tulip_BlazorUI.Service;

namespace Tulip_BlazorUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredLocalStorage(); // -- Iyad
            services.AddBlazoredToast(); // -- Iyad
            services.AddHttpClient();  // -- Iyad 
            services.AddScoped<ApiAuthenticationStateProvider>(); // -- Iyad
            services.AddScoped<AuthenticationStateProvider>(p =>
                    p.GetRequiredService<ApiAuthenticationStateProvider>()); // -- Iyad
            services.AddScoped<JwtSecurityTokenHandler>(); // -- Iyad
            services.AddTransient<IAuthenticationRepository, AuthenticationRepository>(); // -- Iyad 
            services.AddTransient<ICategoryRepository, CategoryRepository>(); // -- Iyad 
            services.AddTransient<IProductRepository, ProductRepository>(); // -- Iyad 
            services.AddTransient<IFileUplaod, FileUpload>(); // -- Iyad 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
