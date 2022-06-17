using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework.Dialogs
{
    internal class DialogHandler : UpdateHandlerBase
    {
        private readonly IDialogManager _dialogManager;

        public DialogHandler(IDialogManager dialogManager)
        {
            _dialogManager = dialogManager;
        }

        public override bool CanHandle(IUpdateContext context)
        {
            return context.IsMessageUpdate() || context.IsCallbackQueryUpdate();
        }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            var chatId = context.Update.Message?.Chat.Id ?? context.Update.CallbackQuery?.Message.Chat.Id;

            if (chatId == null)
                throw new Exception("Cannot get chatId from Update message.");

            var state = _dialogManager.GetUserState(chatId.Value);

            if (state == null)
            {
                await next(context);
                return;
            }

            await _dialogManager.RunDialogAsyncInternal(state.ActiveDialog, state, context);

            await next(context);
        }
    }
}
