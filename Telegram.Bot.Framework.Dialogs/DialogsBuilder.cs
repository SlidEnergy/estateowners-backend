using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Framework.Dialogs
{
    public class DialogsBuilder
    {
        private readonly IServiceCollection _services;

        public DialogsBuilder(IServiceCollection services)
        {
            _services = services;
        }

        //public DialogsBuilder Add<TDialog>() where TDialog : Dialog
        //{
        //    _services.AddScoped<TDialog>();

        //    return this;
        //}
    }
}
