using EstateOwners.App;
using EstateOwners.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    internal class NewEstateDialog : DialogBase
    {
        EstateType _type;
        Building _building;
        string _number;
        List<Building> _buildings;
        ApplicationUser _user;

        private readonly IEstatesService _estatesService;
        private readonly IBuildingsService _buildingsService;
        private readonly IUsersService _usersService;

        public NewEstateDialog(IEstatesService estatesService, IBuildingsService buildingsService, IUsersService usersService)
        {
            _estatesService = estatesService;
            _buildingsService = buildingsService;
            _usersService = usersService;
        }

        public override bool CanHandle(IUpdateContext context)
        {
            return true;
        }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            if (!activate)
            {
                await next(context);
                return;
            }

            switch (step)
            {
                case 1:
                    await Step1(context, next);
                    break;
                case 2:
                    await Step2(context, next);
                    break;
                case 3:
                    await Step3(context, next);
                    break;
                case 4:
                    await Step4(context, next);
                    break;
                case 5:
                    await Step5(context, next);
                    break;

                default:
                    throw new System.Exception("Step not supported");
            }
        }

        public async Task Step1(IUpdateContext context, UpdateDelegate next)
        {
            var msg = context.GetMessage();

            var chatId = msg?.Chat.Id ?? context.Update.CallbackQuery.Message.Chat.Id;

            _user = await _usersService.GetByAuthTokenAsync(chatId.ToString(), AuthTokenType.TelegramChatId);

            if (_user == null)
            {
                step = 1;
                await next.ReplaceDialogAsync<NewUserDialog>(context);
                return;
            }

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

            step++;
        }

        public async Task Step2(IUpdateContext context, UpdateDelegate next)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            _type = (EstateType)Enum.Parse(typeof(EstateType), cq.Data);

            _buildings = await _buildingsService.GetListAsync();

            var buttons = new List<InlineKeyboardButton>();

            foreach (var building in _buildings)
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

            step++;
        }

        public async Task Step3(IUpdateContext context, UpdateDelegate next)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            _building = _buildings.Find(x => x.ShortAddress == cq.Data);

            var message = "";

            switch (_type)
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

            step++;
        }

        public async Task Step4(IUpdateContext context, UpdateDelegate next)
        {
            var msg = context.GetMessage();

            _number = msg.Text;

            var estate = await _estatesService.AddEstate(_user.Id, _building.Id, _type, _number);

            if (estate == null)
            {
                await context.Bot.Client.SendTextMessageAsync(
                    msg.Chat.Id,
                    "Не удалось добавить объект недвижимости, попробуйте позже или обратитесь к администратору.");

                step = 1;
                activate = false;

                await next.ReplaceDialogAsync<MainDialog>(context);

                await next(context);
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

            step++;
        }

        public async Task Step5(IUpdateContext context, UpdateDelegate next)
        {
            var cb = context.Update.CallbackQuery;

            if (cb.Data == "add")
            {
                step = 1;
                await Step1(context, next);
                return;
            }

            await context.Bot.Client.SendTextMessageAsync(
                cb.Message.Chat.Id,
                "Добавление завершено");

            activate = false;
            step = 1;

            await next.ReplaceDialogAsync<MainDialog>(context);
        }
    }
}
