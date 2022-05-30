using EstateOwners.App;
using EstateOwners.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    internal class NewEstateDialog : IUpdateHandler
    {
        int step = 1;
        public static bool activate = false;

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
            step = 1;
            activate = false;
            _estatesService = estatesService;
            _buildingsService = buildingsService;
            _usersService = usersService;
        }

        public bool CanHandleUpdate(IBot bot, Update update)
        {
            return true;
        }

        public async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            if (!activate)
                return UpdateHandlingResult.Continue;

            switch (step)
            {
                case 1:
                    return await Step1(bot, update);
                case 2:
                    return await Step2(bot, update);
                case 3:
                    return await Step3(bot, update);
                case 4:
                    return await Step4(bot, update);
                case 5:
                    return await Step5(bot, update);

                default:
                    throw new System.Exception("Step not supported");
            }
        }

        public async Task<UpdateHandlingResult> Step1(IBot bot, Update update)
        {
            var chatId = update.Message?.Chat.Id ?? update.CallbackQuery.Message.Chat.Id;

            _user = await _usersService.GetByAuthTokenAsync(chatId.ToString(), AuthTokenType.TelegramChatId);

            if (_user == null)
            {
                step = 1;
                NewUserDialog.activate = true;
                return UpdateHandlingResult.Continue;
            }

            var myInlineKeyboard = new InlineKeyboardMarkup()
            {
                InlineKeyboard = new InlineKeyboardButton[][]
                 {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Апартамент", EstateType.Apartment.ToString()),
                        InlineKeyboardButton.WithCallbackData("Паркоместо", EstateType.ParkingSpace.ToString()),
                        InlineKeyboardButton.WithCallbackData("Кладовка", EstateType.Storeroom.ToString()),
                    }
                 }
            };

            await bot.Client.SendTextMessageAsync(
                    chatId,
                    "Какая у вас недвижимость?",
                    replyMarkup: myInlineKeyboard);

            step++;
            return UpdateHandlingResult.Handled;
        }

        public async Task<UpdateHandlingResult> Step2(IBot bot, Update update)
        {
            CallbackQuery cq = update.CallbackQuery;

            _type = (EstateType)Enum.Parse(typeof(EstateType), cq.Data);

            _buildings = await _buildingsService.GetListAsync();

            var buttons = new List<InlineKeyboardButton>();

            foreach (var building in _buildings)
                buttons.Add(InlineKeyboardButton.WithCallbackData(building.ShortAddress));

            var myInlineKeyboard = new InlineKeyboardMarkup()
            {
                InlineKeyboard = new InlineKeyboardButton[][]
                 {
                    buttons.ToArray()
                 }
            };

            await bot.Client.SendTextMessageAsync(
                    cq.Message.Chat.Id,
                    "В каком литере у вас недвижимость?",
                    replyMarkup: myInlineKeyboard);

            step++;
            return UpdateHandlingResult.Handled;
        }

        public async Task<UpdateHandlingResult> Step3(IBot bot, Update update)
        {
            CallbackQuery cq = update.CallbackQuery;

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

            await bot.Client.SendTextMessageAsync(
                    cq.Message.Chat.Id,
                    message);

            step++;

            return UpdateHandlingResult.Handled;
        }

        public async Task<UpdateHandlingResult> Step4(IBot bot, Update update)
        {
            _number = update.Message.Text;

            var estate = await _estatesService.AddEstate(_user.Id, _building.Id, _type, _number);

            if (estate == null)
            {
                await bot.Client.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    "Не удалось добавить объект недвижимости, попробуйте позже или обратитесь к администратору.");

                step = 1;
                activate = false;

                MainDialog.activate = true;

                return UpdateHandlingResult.Continue;
            }

            await bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Ваш объект недвижимости добавлен.");

            var myInlineKeyboard = new InlineKeyboardMarkup()
            {
                InlineKeyboard = new InlineKeyboardButton[][]
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Добавить", "add"),
                        InlineKeyboardButton.WithCallbackData("Нет", "no"),
                    }
                }
            };

            await bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Хотите добавить еще объект недвижимости?",
                replyMarkup: myInlineKeyboard);

            step++;
            return UpdateHandlingResult.Handled;
        }

        public async Task<UpdateHandlingResult> Step5(IBot bot, Update update)
        {
            var cb = update.CallbackQuery;

            if (cb.Data == "add")
            {
                step = 1;
                return await Step1(bot, update);
            }


            await bot.Client.SendTextMessageAsync(
                cb.Message.Chat.Id,
                "Добавление завершено");

            activate = false;
            step = 1;

            MainDialog.activate = true;

            return UpdateHandlingResult.Handled;
        }
    }
}
