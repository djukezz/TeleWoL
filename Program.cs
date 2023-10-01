using Ninject;
using TeleWoL;
using TeleWoL.IoC;
using TeleWoL.Settings;

Console.WriteLine($"Admin ids can be specified by parameters");

var kernel = new StandardKernel(new SettingsModule(), new WoLModule());
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

Console.WriteLine($"Press any key to terminate app");
Console.ReadKey();

bot.Stop();
