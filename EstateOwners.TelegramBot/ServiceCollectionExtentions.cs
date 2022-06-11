using EstateOwners.TelegramBot.Dialogs;
using EstateOwners.TelegramBot.Dialogs.Documents;
using EstateOwners.TelegramBot.Dialogs.Estates;
using EstateOwners.TelegramBot.Dialogs.Polls;
using EstateOwners.TelegramBot.Dialogs.Signing;
using EstateOwners.TelegramBot.Dialogs.Support;
using EstateOwners.TelegramBot.Dialogs.Voting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Dialogs;

namespace EstateOwners.TelegramBot
{
    public static class ServiceCollectionExtentions
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

            services.AddTransient<IMenuRenderer, MenuRenderer>();

            services.AddTelegramDialogs();
            
            services.AddScoped<ProfileDialog>();
            services.AddScoped<NewUserDialog>();

            services.AddScoped<EstatesDialog>();
            services.AddScoped<AddEstateDialog>();

            services.AddScoped<CarsDialog>();
            services.AddScoped<AddCarDialog>();

            services.AddScoped<VoteMessagesDialog>();
            services.AddScoped<AddVoteMessageDialog>();
            services.AddScoped<AddPollDialog>();
            services.AddScoped<PollsDialog>();
            services.AddScoped<AddSignatureDialog>();
            services.AddScoped<CandidatesDialog>();

            services.AddScoped<SupportDialog>();
            services.AddScoped<IssueMessagesDialog>();
            services.AddScoped<AddIssueMessageDialog>();

            services.AddScoped<LibraryDialog>();
            services.AddScoped<AddDocumentMessageDialog>();
            services.AddScoped<DocumentMessagesDialog>();

            services.AddScoped<EmptyDialog>();

            services.AddTelegramBot<EstateOwnersBot>(botOptions);
        }

        public static void UseTelegramBot(this IApplicationBuilder app, bool isDevelopment)
        {
            //if (isDevelopment)
            //{
                var botBuilder = new BotBuilder()
                    .Use<ExceptionHandler>()
                    .Use<StartCommand>()
                    .Use<MenuCommand>()
                    .Use<MenuHandler>()
                    .UseDialogHandler()
                    .Build();

                // get bot updates from Telegram via long-polling approach during development
                // this will disable Telegram webhooks
                app.UseTelegramBotLongPolling<EstateOwnersBot>(botBuilder, TimeSpan.FromSeconds(2));
            //}
            //else
            //{
            //    // use Telegram bot webhook middleware in higher environments
            //    app.UseTelegramBotWebhook<EstateOwnersBot>();
            //    // and make sure webhook is enabled
            //    app.EnsureWebhookSet<EstateOwnersBot>();
            //}
        }
    }
}
