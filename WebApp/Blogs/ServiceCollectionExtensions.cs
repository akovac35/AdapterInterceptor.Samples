// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač


using AutoMapper;
using com.github.akovac35.AdapterInterceptor;
using com.github.akovac35.AdapterInterceptor.DependencyInjection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Blogs;

namespace WebApp.Blogs
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScopedBlogServiceAdapter<T>(this IServiceCollection services)
        {
            services.AddScoped<SqliteConnection>(fact =>
            {
                var tmp = new SqliteConnection("DataSource=:memory:");
                return tmp;
            });

            services.AddScoped<BlogContext>(fact =>
            {
                // This connection is explicitly provided so we have to manage it explicitly by
                // opening, closing and disposing it
                var connection = fact.GetService<SqliteConnection>();
                // Provided by logger frameworks as a singleton
                var loggerFactory = fact.GetService<ILoggerFactory>();
                var options = new DbContextOptionsBuilder<BlogContext>()
                .UseSqlite(connection)
                .UseLoggerFactory(loggerFactory)
                .Options;
                var context = new BlogContext(options);
                // In-memory database exists only for the duration of an open connection
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                return context;
            });

            services.AddScoped<BlogService>();

            // Register the adapter
            // The IBlogServiceAdapter interface inherits the IDisposable interface. When the scope is closed, the adapter instance will be disposed of by the DI framework, which will also invoke the Dispose() method on the target through the AdapterInterceptor. Note we have to release the AdapterInterceptor to release the target, it is never released by the Dispose() method invocation
            services.AddAdapter<IBlogServiceAdapter<T>, BlogService>(targetFact =>
            {
                // Obtain the target - the adaptee
                var blogService = targetFact.GetService<BlogService>();
                return blogService;
            }, (serviceProvider, target) =>
            {
                var mapperConfiguration = serviceProvider.GetService<MapperConfiguration>();
                var adapterMapper = new DefaultAdapterMapper(mapperConfiguration.CreateMapper());
#if DEBUG
                // AdapterInterceptor emits logs when TRACE logger level is enabled for com.github.akovac35.AdapterInterceptor. This can be quite verbose but enables method invocation diagnostics. It is possible to completely disable logging by simply not providing a logger factory
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                var adapterInterceptor = new CustomAdapterInterceptor<BlogService, Blog, T>(target, adapterMapper, loggerFactory);
#else
                var adapterInterceptor = new CustomAdapterInterceptor<BlogService, Blog, T>(target, adapterMapper);
#endif
                return adapterInterceptor;
            }, ServiceLifetime.Scoped);

            return services;
        }
    }
}
