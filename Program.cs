using Ninject;
using TeleWoL;
using TeleWoL.IoC;
using TeleWoL.Settings;

if (args.Length < 1)
{
    Console.WriteLine("Path to settings file must be specified");
    return;
}

string settingsFilePath = args[0];

var kernel = new StandardKernel(new SettingsModule(settingsFilePath), new WoLModule());
var settings  = kernel.Get<GlobalSettings>();
Console.WriteLine("Users:");
foreach (UserSettings user in settings.GetAll().OrderBy(u=>u.Permission))
    Console.WriteLine(user.ToString());

using var bot = new Bot(kernel);
var admins = Environment.GetCommandLineArgs().Skip(1)
    .Select(id => (long.TryParse(id, out long v), v))
    .Where(p => p.Item1)
    .Select(p => p.Item2);
bot.AddAdmins(admins);

var botUser = await bot.Start();
Console.WriteLine($"@{botUser.Username}");
Console.WriteLine($"t.me/{botUser.Username}");

ConsoleHost.WaitForShutdown();

bot.Stop();
