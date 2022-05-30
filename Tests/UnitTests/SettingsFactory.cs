﻿using EstateOwners.App;

namespace EstateOwners.WebApi.UnitTests
{
	/// <summary>
	/// Создает объекты с настройками приложения.
	/// </summary>
	public class SettingsFactory
	{
		public static AuthSettings CreateAuth()
		{
			return new AuthSettings() {
				Issuer = "Test issuer",
				Audience = "Test audience",
				Key = "Very very very long secret #1",
				LifetimeMinutes = 60
			};
		}

		public static TelegramBotSettings CreateTelegramBot()
		{
			return new TelegramBotSettings()
			{
				BotUserName = "SlidTestBot",
				ApiToken = "1062716492:AAE74DajBXwMfMkquCyoFG1PcFJcUgQ4zW0"
			};
		}
	}
}
