using System;
using System.Linq;
using M2DevBot.Web.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace M2DevBot.Web
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBotCommands(this IServiceCollection services)
        {
            var commandTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => !x.IsInterface && typeof(IChatCommand).IsAssignableFrom(x));

            foreach(var commandType in commandTypes)
            {
                services.AddTransient(typeof(IChatCommand), commandType);
            }
        }
    }
}