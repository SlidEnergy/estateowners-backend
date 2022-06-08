using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Framework.Dialogs
{
    public static class ServiceCollectionExtensions
    {
        public static DialogsBuilder AddTelegramDialogs(this IServiceCollection services)
        {
            services.AddSingleton<IDialogManager, DialogManager>();

            return new DialogsBuilder(services);
        }
    }
}
