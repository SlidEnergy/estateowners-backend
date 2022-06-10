using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Slid.Auth.WebApi
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddSlidAuth(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddApplicationPart(Assembly.GetExecutingAssembly());

            return mvcBuilder;
        }
    }
}
