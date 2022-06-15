namespace EstateOwners.WebApi.Telegram.Connect
{
    public class TelegramBotSettings
    {
        public string Username { get; set; }
        public string ApiToken { get; set; }
        public string Aes256Key { get; set; }
        public string DrawSignatureUrl { get; set; }
    }
}
