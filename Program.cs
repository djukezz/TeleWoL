Console.WriteLine($"Admin ids can be specified by parameters");

using var bot = new Bot();
var admins = Environment.GetCommandLineArgs().Skip(1)
    .Select(id => (long.TryParse(id, out long v), v))
    .Where(p => p.Item1)
    .Select(p => p.Item2);
bot.AddAdmins(admins);
bot.Start();

Console.WriteLine($"Press any key to terminate app");
Console.ReadKey();

bot.Stop();
