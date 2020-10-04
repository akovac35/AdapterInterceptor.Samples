// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using Castle.DynamicProxy;
using com.github.akovac35.AdapterInterceptor;
using com.github.akovac35.AdapterInterceptor.Misc;
using com.github.akovac35.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;
using System.Threading.Tasks;

namespace TestApp
{
    public class CustomProxyImitatorInterceptor<TTarget> : ProxyImitatorInterceptor<TTarget>
        where TTarget : notnull
    {
        public CustomProxyImitatorInterceptor(TTarget target, ILoggerFactory? loggerFactory = null) : base(target, loggerFactory)
        {
            _logger = (loggerFactory?.CreateLogger<CustomProxyImitatorInterceptor<TTarget>>()) ?? (ILogger)NullLogger.Instance;
        }

        private ILogger _logger;

        protected override object? InvokeTargetSync(MethodInfo adapterMethod, AdapterInvocationInformation adapterInvocationInformation, object?[] targetArguments, IInvocation invocation)
        {
            _logger.Here(l => l.LogInformation("Hello from a custom interceptor."));
            var result = base.InvokeTargetSync(adapterMethod, adapterInvocationInformation, targetArguments, invocation);
            return result;
        }

        protected override async Task<TAdapter> InvokeTargetGenericTaskAsync<TAdapter>(MethodInfo adapterMethod, AdapterInvocationInformation adapterInvocationInformation, object?[] targetArguments)
        {
            _logger.Here(l => l.LogInformation("Hello from a custom interceptor."));
            var result = await base.InvokeTargetGenericTaskAsync<TAdapter>(adapterMethod, adapterInvocationInformation, targetArguments);
            return result;
        }
    }
}
