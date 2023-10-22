using Croc.TestDrive.TgChatBot;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

public class TelegramBot
{
	private readonly TelegramBotClient _botClient;
	private readonly IServiceProvider _service;

	public TelegramBot(string token, IServiceProvider service)
	{
		_botClient = new TelegramBotClient(token);
		_service = service;
	}

	public async Task StartAsync()
	{
		var me = await _botClient.GetMeAsync();
		Console.WriteLine($"Идентификатор бота: {me.Id}, ник: @{me.Username}");
		_botClient.StartReceiving(
			OnUpdateAsync,
			OnErrorAsync
		);
		Console.WriteLine("Бот запущен. Нажмите CTRL+C чтобы выйти.");
		Thread.Sleep(Timeout.Infinite);
	}
	async Task OnUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
	{
		try
		{
			using var scope = _service.CreateAsyncScope();
			var messageHandler = scope.ServiceProvider.GetRequiredService<BotMessageHandler>();
			await messageHandler.OnInputAsync(bot, update, cancellationToken);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
			throw;
		}
	}
	Task OnErrorAsync(ITelegramBotClient bot, Exception e, CancellationToken cancellationToken)
	{
		var errorMessage = e switch
		{
			ApiRequestException apiRequest => $"Ошибка Telegram API: {apiRequest.ErrorCode}\n{apiRequest.Message}",
			_ => e.ToString()
		};
		Console.WriteLine(errorMessage);
		return Task.CompletedTask;
	}
}
