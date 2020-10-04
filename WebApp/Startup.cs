// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using AutoMapper;
using com.github.akovac35.Logging.AspNetCore.Correlation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Shared;
using Shared.Blogs;
using WebApp.Blogs;

namespace WebApp
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
            services.AddSingleton(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Blog, BlogDto>().ReverseMap();
            }));

            services.AddScopedBlogServiceAdapter<BlogDto>();

            services.AddLoggingCorrelation();

            services.AddRazorPages();
            services.AddServerSideBlazor();
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
            }

            SamplesLoggingHelper.LoggerConfig(configActionNLog: () =>
            {
                app.UseLoggingCorrelation();
            }, configActionSerilog: () =>
            {
                app.UseLoggingCorrelation();
                app.UseSerilogRequestLogging();
            });

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
