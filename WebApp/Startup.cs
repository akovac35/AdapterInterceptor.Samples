// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using AutoMapper;
using Castle.DynamicProxy;
using com.github.akovac35.Logging.Correlation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            services.AddHttpContextAccessor();

            services.AddScoped<ICorrelationProvider, CorrelationProvider>();

            services.AddSingleton(new ProxyGenerator());

            services.AddSingleton(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Blog, BlogDto>().ReverseMap();
            }));

            services.AddScopedBlogServiceAdapter<BlogDto>();

            services.AddRazorPages();
            services.AddServerSideBlazor();
        }       

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            SamplesLoggingHelper.LoggerConfig(configActionNLog: () =>
            {
                // Update the LoggerFactoryProvider once logging config is fully complete
                com.github.akovac35.Logging.LoggerFactoryProvider.LoggerFactory = loggerFactory;
            }, configActionSerilog: () =>
            {
                app.UseSerilogRequestLogging();
                // Update the LoggerFactoryProvider once logging config is fully complete
                com.github.akovac35.Logging.LoggerFactoryProvider.LoggerFactory = loggerFactory;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

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
