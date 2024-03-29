﻿using EstateOwners.App;
using EstateOwners.Domain;
using Lers.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs
{
    internal class AddEstateDialog : Dialog<EstateDialogStore>
    {
        private readonly IEstatesService _estatesService;
        private readonly IBuildingsService _buildingsService;

        public AddEstateDialog(IEstatesService estatesService, IBuildingsService buildingsService)
        {
            _estatesService = estatesService;
            _buildingsService = buildingsService;

            AddStep(Step1);
            AddStep(Step2);
            AddStep(Step3);
            AddStep(Step4);
            AddStep(Step5);
            AddStep(Step6);
            AddStep(Step7);
        }

        public async Task Step1(DialogContext<EstateDialogStore> context, CancellationToken cancellationToken)
        {
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
                    context.ChatId,
                    "Какая у вас недвижимость?",
                    replyMarkup: myInlineKeyboard);

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<EstateDialogStore> context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            context.Store.Type = (EstateType)Enum.Parse(typeof(EstateType), cq.Data);

            context.Store.Buildings = await _buildingsService.GetListAsync(); ;

            var buttons = new List<InlineKeyboardButton>();

            foreach (var building in context.Store.Buildings)
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

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step3(DialogContext<EstateDialogStore> context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            context.Store.Building = context.Store.Buildings.Find(x => x.ShortAddress == cq.Data);

            var message = "";

            switch (context.Store.Type)
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

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step4(DialogContext<EstateDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            context.Store.Number = msg.Text;

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Введите площадь вашего объекта недвижимости");

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step5(DialogContext<EstateDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            context.Store.Area = Convert.ToSingle(FormatUtils.ReplaceDecimalSeparator(msg.Text));

            
            var input = $"{context.Store.Building} {context.Store.Type.GetDescription().ToLower()} {context.Store.Number} площадью {context.Store.Area} м²";

            var myInlineKeyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Верно", "valid"),
                        InlineKeyboardButton.WithCallbackData("Повторить ввод данных", "retry"),
                    }
                }
            );

            await context.Bot.Client.SendTextMessageAsync(
                context.ChatId,
                "Проверьте и подтвердите введенные данные:" + Environment.NewLine + input,
                replyMarkup: myInlineKeyboard);

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput, new string[] { "valid", "retry"})]
        public async Task Step6(DialogContext<EstateDialogStore> context, CancellationToken cancellationToken)
        {
            var cb = context.Update.CallbackQuery;

            if (cb.Data == "retry")
            {
                await context.ExecuteStepAsync(Step1, cancellationToken);
                return;
            }

            var model = new Estate(context.Store.Type, context.Store.Building.Id, context.Store.Number)
            {
                Area = context.Store.Area
            };

            var estate = await _estatesService.AddEstateAsync(context.Store.User.Id, model);

            if (estate == null)
            {
                await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Не удалось добавить объект недвижимости, попробуйте позже или обратитесь к администратору.");

                context.EndDialog();
                return;
            }

            await context.Bot.Client.SendTextMessageAsync(
                context.ChatId,
                "Ваш объект недвижимости добавлен.");

            var replyMarkup = ReplyMarkupBuilder.InlineKeyboard()
                .ColumnWithCallbackData("Добавить", "add")
                .ColumnWithCallbackData("Нет", "no")
                .ToMarkup();

            await context.Bot.Client.SendTextMessageAsync(
                context.ChatId,
                "Хотите добавить еще объект недвижимости?",
                replyMarkup: replyMarkup);

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step7(DialogContext<EstateDialogStore> context, CancellationToken cancellationToken)
        {
            var cb = context.Update.CallbackQuery;

            if (cb.Data == "add")
            {
                await context.ExecuteStepAsync(Step1, cancellationToken);
                return;
            }

            await context.Bot.Client.SendTextMessageAsync(
                cb.Message.Chat.Id,
                "Добавление завершено");

            await context.SendEstateListAsync(context.ChatId);

            context.EndDialog();
        }
    }
}
