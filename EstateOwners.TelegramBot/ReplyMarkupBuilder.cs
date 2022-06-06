using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    public enum ReplyMarkupType
    {
        InlineKeyboard,
        Keyboard
    }

    public class ReplyMarkupBuilder
    {
        private readonly ReplyMarkupType _replyMarkupType;
        private readonly List<List<IKeyboardButton>> _buttons = new List<List<IKeyboardButton>>() { new List<IKeyboardButton>() };
        private int _rowIndex = 0;

        public ReplyMarkupBuilder(ReplyMarkupType replyMarkupType)
        {
            _replyMarkupType = replyMarkupType;
        }

        public static ReplyMarkupBuilder InlineKeyboard()
        {
            return new ReplyMarkupBuilder(ReplyMarkupType.InlineKeyboard);
        }

        public static ReplyMarkupBuilder Keyboard()
        {
            return new ReplyMarkupBuilder(ReplyMarkupType.Keyboard);
        }

        public ReplyMarkupBuilder ColumnWithCallbackData(string textAndCallbackData)
        {
            _buttons[_rowIndex].Add(InlineKeyboardButton.WithCallbackData(textAndCallbackData));

            return this;
        }

        public ReplyMarkupBuilder ColumnWithCallbackData(string text, string callbackData)
        {
            _buttons[_rowIndex].Add(InlineKeyboardButton.WithCallbackData(text, callbackData));

            return this;
        }

        public ReplyMarkupBuilder ColumnKeyboardButton(string text)
        {
            _buttons[_rowIndex].Add(new KeyboardButton(text));

            return this;
        }

        public ReplyMarkupBuilder Column(IKeyboardButton button)
        {
            _buttons[_rowIndex].Add(button);

            return this;
        }

        public ReplyMarkupBuilder NewRow()
        {
            _rowIndex++;
            _buttons.Add(new List<IKeyboardButton>());

            return this;
        }

        public IReplyMarkup ToMarkup()
        {
            if (_replyMarkupType == ReplyMarkupType.InlineKeyboard)
            {
                var array = new InlineKeyboardButton[_buttons.Count][];
                for (var i = 0; i < _buttons.Count; i++)
                    array[i] = _buttons[i].Cast<InlineKeyboardButton>().ToArray();

                return new InlineKeyboardMarkup(array);
            }
            else
            {
                var array = new KeyboardButton[_buttons.Count][];
                for (var i = 0; i < _buttons.Count; i++)
                    array[i] = _buttons[i].Cast<KeyboardButton>().ToArray();

                return new ReplyKeyboardMarkup(array, true);
            }
        }
    }
}
