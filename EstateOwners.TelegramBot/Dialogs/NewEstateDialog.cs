using EstateOwners.App;
using EstateOwners.Domain;
using EstateOwners.TelegramBot.Dialogs.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    internal class NewEstateDialog : DialogBase
    {
        private readonly IEstatesService _estatesService;
        private readonly IBuildingsService _buildingsService;
        private readonly IUsersService _usersService;

        public NewEstateDialog(IEstatesService estatesService, IBuildingsService buildingsService, IUsersService usersService)
        {
            _estatesService = estatesService;
            _buildingsService = buildingsService;
            _usersService = usersService;

            AddStep(Step1);
            AddStep(Step2);
            AddStep(Step3);
            AddStep(Step4);
            AddStep(Step5);
        }

        public async Task Step1(DialogContext context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            var chatId = msg?.Chat.Id ?? context.Update.CallbackQuery.Message.Chat.Id;

            var user = await _usersService.GetByAuthTokenAsync(chatId.ToString(), AuthTokenType.TelegramChatId);

            if (user == null)
            {
                context.ReplaceDialog<NewUserDialog>();
                return;
            }

            context.Values["user"] = user;

            var myInlineKeyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                 {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Апартамент", EstateType.Apartment.ToString()),
                        InlineKeyboardButton.WithCallbackData("Паркоместо", EstateType.ParkingSpace.ToString()),
                        InlineKeyboardButton.WithCallbackData("Кладовка", EstateType.Storeroom.ToString()),
                    }
                 }
            );

            await context.Bot.Client.SendTextMessageAsync(
                    chatId,
                    "Какая у вас недвижимость?",
                    replyMarkup: myInlineKeyboard);

            context.NextStep();
        }

        public async Task Step2(DialogContext context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            context.Values["type"] = (EstateType)Enum.Parse(typeof(EstateType), cq.Data);

            var buildings = await _buildingsService.GetListAsync();

            context.Values["buildings"] = buildings;

            var buttons = new List<InlineKeyboardButton>();

            foreach (var building in buildings)
                buttons.Add(InlineKeyboardButton.WithCallbackData(building.ShortAddress));

            var myInlineKeyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                 {
                    buttons.ToArray()
                 }
            );

            await context.Bot.Client.SendTextMessageAsync(
                    cq.Message.Chat.Id,
                    "В каком литере у вас недвижимость?",
                    replyMarkup: myInlineKeyboard);

            context.NextStep();
        }

        public async Task Step3(DialogContext context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            var buildings = (List<Building>)context.Values["buildings"];
            var type = context.Values["type"];
            context.Values["building"] = buildings.Find(x => x.ShortAddress == cq.Data);

            var message = "";

            switch (type)
            {
                case EstateType.Apartment:
                    message = "Введите номер апартамента";
                    break;

                case EstateType.ParkingSpace:
                    message = "Введите номер парковочного места";
                    break;

                case EstateType.Storeroom:
                    message = "Введите номер кладовки";
                    break;

                default:
                    throw new Exception("EstateType is not supported.");
            }

            await context.Bot.Client.SendTextMessageAsync(
                    cq.Message.Chat.Id,
                    message);

            context.NextStep();
        }

        public async Task Step4(DialogContext context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            var user = (ApplicationUser)context.Values["user"];
            var type = (EstateType)context.Values["type"];
            var building = (Building)context.Values["building"];
            var number = msg.Text;

            var estate = await _estatesService.AddEstate(user.Id, building.Id, type, number);

            if (estate == null)
            {
                await context.Bot.Client.SendTextMessageAsync(
                    msg.Chat.Id,
                    "Не удалось добавить объект недвижимости, попробуйте позже или обратитесь к администратору.");

                context.EndDialog();
                return;
            }

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Ваш объект недвижимости добавлен.");

            var myInlineKeyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Добавить", "add"),
                        InlineKeyboardButton.WithCallbackData("Нет", "no"),
                    }
                }
            );

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Хотите добавить еще объект недвижимости?",
                replyMarkup: myInlineKeyboard);

            context.NextStep();
        }

        public async Task Step5(DialogContext context, CancellationToken cancellationToken)
        {
            var cb = context.Update.CallbackQuery;

            if (cb.Data == "add")
            {
                await context.ExecuteStep(Step1, cancellationToken);
                return;
            }

            await context.Bot.Client.SendTextMessageAsync(
                cb.Message.Chat.Id,
                "Добавление завершено");

            context.EndDialog();
        }
    }
}
