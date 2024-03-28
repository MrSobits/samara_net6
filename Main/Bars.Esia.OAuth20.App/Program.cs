namespace Bars.Esia.OAuth20.App
{
    using Bars.B4.Modules.Console;
    using Bars.Esia.OAuth20.App.Application;

    public class Program
    {
        public static void Main(string[] args)
        {
            ConsoleRunner.Run<AuthApp, AuthAppContext, AuthAppWindowsService>(args);
        }
    }
}