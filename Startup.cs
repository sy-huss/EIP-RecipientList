using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using RecipientList.Services;

[assembly: FunctionsStartup(typeof(RecipientList.Startup))]

namespace RecipientList
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IMessageConstruction, MessageConstruction>();
        }
    }
}