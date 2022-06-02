using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot
{
    public static class EstateOwnersTelegramBotExtentions
    {
        public static void AddTelegramBot(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            BotOptions botOptions;

            if (isDevelopment)
            {
                botOptions = configuration
                    .GetSection("Telegram")
                    .GetSection("SlidTestBot")
                    .Get<BotOptions>();
            }
            else
            {
                botOptions = new BotOptions()
                {
                    ApiToken = Environment.GetEnvironmentVariable("BOT_KEY"),
                    Username = Environment.GetEnvironmentVariable("BOT_NAME"),
                };
            }

            services.AddTelegramBot<EstateOwnersBot>(botOptions);
                //.AddUpdateHandler<StartCommand>()
                //.AddUpdateHandler<MainDialog>()
                //.AddUpdateHandler<MenuDialog>()
                //.AddUpdateHandler<CallbackQueryHandler>()
                //.AddUpdateHandler<NewUserDialog>()
                //.AddUpdateHandler<NewEstateDialog>()
                //.Configure();

            // services.AddScoped<IBotManager<EstateOwnersBot>, BotManager<EstateOwnersBot>>();
        }

        public static void UseTelegramBot(this IApplicationBuilder app, bool isDevelopment)
        {
            if (isDevelopment)
            {
                //var a = app.ApplicationServices.GetRequiredService<IBotManager<EstateOwnersBot>>();
                // get bot updates from Telegram via long-polling approach during development
                // this will disable Telegram webhooks
                //app.UseTelegramBotLongPolling<EstateOwnersBot>();

                //Task.Factory.StartNew(async () =>
                //{
                //    using (var scope = app.ApplicationServices.CreateScope())
                //    {
                //        var botManager = scope.ServiceProvider.GetRequiredService<IBotManager<EstateOwnersBot>>();

                //        // make sure webhook is disabled so we can use long-polling
                //        await botManager.SetWebhookStateAsync(false);

                //        while (true)
                //        {
                //            await Task.Delay(1_000);
                //            await botManager.GetAndHandleNewUpdatesAsync();
                //        }
                //    }
                //}).ContinueWith(t =>
                //{
                //    if (t.IsFaulted) throw t.Exception;
                //});

                var botBuilder = new BotBuilder()
                    .Use<ExceptionHandler>()
                    .Use<StartCommand>()
                    .Use<MainDialog>()
                    .Use<MenuDialog>()
                    .Use<NewUserDialog>()
                    .Use<NewEstateDialog>()
                    .Build();

                // get bot updates from Telegram via long-polling approach during development
                // this will disable Telegram webhooks
                app.UseTelegramBotLongPolling<EstateOwnersBot>(botBuilder, TimeSpan.FromSeconds(2));
            }
            else
            {
                // use Telegram bot webhook middleware in higher environments
                app.UseTelegramBotWebhook<EstateOwnersBot>();
                // and make sure webhook is enabled
                app.EnsureWebhookSet<EstateOwnersBot>();
            }
        }
    }
}
