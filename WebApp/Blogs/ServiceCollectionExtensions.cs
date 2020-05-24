// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač


using AutoMapper;
using Castle.DynamicProxy;
using com.github.akovac35.AdapterInterceptor;
using com.github.akovac35.Logging;
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
                Logger.Here(l => l.Entering());

                var tmp = new SqliteConnection("DataSource=:memory:");

                Logger.Here(l => l.ExitingSimpleFormat(tmp));
                return tmp;
            });

            services.AddScoped<BlogContext>(fact =>
            {
                Logger.Here(l => l.Entering());

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

                Logger.Here(l => l.ExitingSimpleFormat(context));
                return context;
            });

            services.AddScoped<IBlogServiceAdapter<T>>(fact =>
            {
                Logger.Here(l => l.Entering());

                var context = fact.GetService<BlogContext>();
                var blogService = new BlogService(context);
                var mapperConfiguration = fact.GetService<MapperConfiguration>();
                var mapper = mapperConfiguration.CreateMapper();
                var proxyGenerator = fact.GetService<ProxyGenerator>();
                var adapterMapper = new DefaultAdapterMapper(mapper);

#if DEBUG
                // AdapterInterceptor emits logs when TRACE logger level is enabled for com.github.akovac35.AdapterInterceptor. This can be quite verbose but enables method invocation diagnostics. It is possible to completely disable logging by simply not providing a logger factory
                var loggerFactory = fact.GetService<ILoggerFactory>();
                var adapterInterceptor = new AdapterInterceptor<BlogService, Blog, T>(blogService, adapterMapper, loggerFactory);
#else
                var adapterInterceptor = new AdapterInterceptor<BlogService, Blog, T>(blogService, adapterMapper);
#endif

                // In-memory database exists only for the duration of an open connection
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                // IBlogServiceAdapter interface inherits the IDisposable interface. When the scope is closed, the adapter instance will be disposed of by the DI framework, which will also invoke the Dispose() method on the target. Note we have to release the AdapterInterceptor to release the target, it is never released by the Dispose() method invocation
                var adapter = proxyGenerator.CreateInterfaceProxyWithoutTarget<IBlogServiceAdapter<T>>(adapterInterceptor);

                Logger.Here(l => l.ExitingSimpleFormat(adapter));
                return adapter;
            });
            return services;
        }

        // The logging framework must be fully configured before working logger instances can be created, so static initialization should not be used
        private static ILogger _logger;
        private static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = LoggerFactoryProvider.LoggerFactory.CreateLogger(typeof(ServiceCollectionExtensions));
                }

                return _logger;
            }
        }
    }
}
