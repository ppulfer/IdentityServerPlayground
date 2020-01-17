using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace MvcClient
{
    public class Startup
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;

        public Startup(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {            
            Console.WriteLine(hostingEnvironment.ApplicationName);
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {                    
                    options.Authority = "http://identityserver:8888";
                    options.RequireHttpsMetadata = false;
                    options.Scope.Add("api1");

                    options.ClientId = configuration.GetValue<string>("CLIENT_ID");
                    options.ClientSecret = "secret";
                    options.ResponseType = OpenIdConnectResponseType.IdTokenToken;
                    options.SaveTokens = true;
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/MvcTest/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
