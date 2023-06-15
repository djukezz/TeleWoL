using Telegram.Bot.Types.ReplyMarkups;

namespace TeleWoL;

internal record Response
{
    public Response(string text, InlineKeyboardMarkup? keyboardMarkup = null)
    {
        Text = text;
        KeyboardMarkup = keyboardMarkup;
    }

    public static readonly Response Unknown = "Unknown command";

    public string Text { get; }
    public InlineKeyboardMarkup? KeyboardMarkup { get; }

    public static implicit operator Response(string text) => new Response(text);
    public static implicit operator string(Response message) => message.Text;
}
