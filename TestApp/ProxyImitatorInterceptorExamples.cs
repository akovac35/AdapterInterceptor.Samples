// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Blogs;
using System;

namespace TestApp
{
    [TestFixture]
    public class ProxyImitatorInterceptorExamples
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            customOnWrite = writeContext =>
            {
                Console.WriteLine(writeContext);
            };

            customOnBeginScope = scopeContext =>
            {
                Console.WriteLine(scopeContext);
            };

            serviceCollection = new ServiceCollection();
            // Register TestLogger using extension method (uses TryAdd),
            // always register it as the first service for reliable registration
            serviceCollection.AddTestLogger(onWrite: customOnWrite, onBeginScope: customOnBeginScope);
            serviceCollection.AddScopedBlogServiceProxyImitator();
        }

        [SetUp]
        public void SetUp()
        {
        }

        private IServiceCollection serviceCollection = null!;

        private Action<WriteContext> customOnWrite = null!;
        private Action<ScopeContext> customOnBeginScope = null!;

        [Test]
        public void Test_WithLoggingToTestConsole_Works()
        {
            // It is easy to create a proxy which does not require any virtual methods on the target
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var proxy = serviceProvider.GetRequiredService<IBlogServiceProxyImitator>();
            proxy.Add();
            proxy.Context.SaveChanges();

            Assert.AreNotEqual(0, proxy.Count);
        }
    }
}
