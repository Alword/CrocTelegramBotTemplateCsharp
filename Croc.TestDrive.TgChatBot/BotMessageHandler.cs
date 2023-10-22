using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Croc.TestDrive.TgChatBot
{
	/// <summary>
	/// Создаётся новый экземпляр на каждое сообщение
	/// </summary>
	internal class BotMessageHandler
	{
		/// <summary>
		/// Если что-то понадобится добавить через DI
		/// Скорее всего потребуется сервис для хранения истории сообщений
		/// Ну или можно сделать в статической переменной
		/// </summary>
		public BotMessageHandler() { }

		/// <summary>
		/// Тут бот получает сообщения от пользователей, нужно считать или все входные данные сразу и выдать ответ
		/// Или считывать данные потихоньку запоминая всё например в словарь с состоянием и ключём chatId
		/// Стоит учесть что <see cref="BotMessageHandler"/> сейчас создаётся новый на каждое сообщение поэтому 
		/// или это нужно поменять в классе <see cref="Program"/> 
		/// или контроллер состояния, одиночку, через конструктор
		/// или сделать статическую переменную например
		/// Документация: https://telegrambots.github.io/book/
		/// </summary>
		/// <param name="telegramBot"></param>
		/// <param name="update"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task OnInputAsync(ITelegramBotClient telegramBot, Update update, CancellationToken cancellationToken)
		{
			InlineKeyboardMarkup mainMenu = new InlineKeyboardMarkup(new[]
			{
				new[] { InlineKeyboardButton.WithUrl("К документации!🚀","https://telegrambots.github.io/book/") },
			});

			if (update.Type != UpdateType.Message || update.Message is null) return;
			var message = update.Message;

			var chatId = message.Chat.Id;
			var messageText = message.Text;
			var firstName = message.From?.FirstName ?? "Неизвестный";
			Console.WriteLine($"[{DateTime.Now:hh:mm:ss}] {chatId}/{firstName}: {messageText}");
			if (message.Text == "/start")
			{
				Message sentMessage = await telegramBot.SendTextMessageAsync(
					chatId: chatId,
					text: $"Привет {firstName}. Я полезный бот, чем могу помочь?",
					replyMarkup: mainMenu,
					cancellationToken: cancellationToken
				);
			}
		}
	}
}
