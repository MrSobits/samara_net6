namespace Bars.Gkh.Executor
{
    using Bars.B4.Modules.Console;
    using Bars.B4.Modules.Tasks.Executor.App;
    using Bars.B4.Modules.Tasks.Executor.Daemon;

    public static class Program
    {
        public static void Main(string[] args)
        {
            ConsoleRunner.Run<Application, TasksAppContext, WindowsService>(new[] { "-s" });
        }
    }
}