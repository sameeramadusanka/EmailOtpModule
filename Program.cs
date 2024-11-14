// See https://aka.ms/new-console-template for more information
using EmailOtpModule.Services;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Star Console Application");

var services = new ServiceCollection();

#region Register services
services.AddTransient<IConsoleService, ConsoleService>();
services.AddTransient<IOtpService, OtpService>();
#endregion


var serviceBuilder = services.BuildServiceProvider();
var consoleService = serviceBuilder.GetRequiredService<IConsoleService>();
consoleService.start();
