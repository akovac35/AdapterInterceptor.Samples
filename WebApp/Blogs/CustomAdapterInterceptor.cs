// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using Castle.DynamicProxy;
using com.github.akovac35.AdapterInterceptor;
using com.github.akovac35.AdapterInterceptor.Misc;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApp.Blogs
{
    public class CustomAdapterInterceptor<TTarget, TSource1, TDestination1>: AdapterInterceptor<TTarget, TSource1, TDestination1>
    {
        public CustomAdapterInterceptor(TTarget target, IAdapterMapper adapterMapper, ILoggerFactory loggerFactory = null) : base(target, adapterMapper, loggerFactory) { }

        protected override object InvokeTargetSync(MethodInfo adapterMethod, AdapterInvocationInformation adapterInvocationInformation, object[] targetArguments, IInvocation invocation)
        {
            return base.InvokeTargetSync(adapterMethod, adapterInvocationInformation, targetArguments, invocation);
        }

        protected override async Task<TAdapter> InvokeTargetGenericTaskAsync<TAdapter>(MethodInfo adapterMethod, AdapterInvocationInformation adapterInvocationInformation, object[] targetArguments)
        {
            return await base.InvokeTargetGenericTaskAsync<TAdapter>(adapterMethod, adapterInvocationInformation, targetArguments);
        }

        // And similarly for Task and (generic) ValueTask
    }
}
