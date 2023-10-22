using Croc.TestDrive.TgChatBot;
using Microsoft.Extensions.DependencyInjection;

/// <summary>Нужно получить токен у https://telegram.me/BotFather</summary>
const string TOKEN = "";


var serviceProvider = ConfigureServices();
var bot = serviceProvider.GetRequiredService<TelegramBot>();
bot.StartAsync().Wait();
static IServiceProvider ConfigureServices()
{
	var services = new ServiceCollection();
	services.AddSingleton(sp => new TelegramBot(TOKEN, sp));
	// Тот самый класс который всё слушает BotMessageHandler
	services.AddTransient<BotMessageHandler>();
	// Другие сервисы и зависимости могут быть добавлены сюда
	return services.BuildServiceProvider();
}