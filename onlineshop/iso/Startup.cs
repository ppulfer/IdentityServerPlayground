using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace JavaScriptClient
{
    public class Startup
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration configuration;

        public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
