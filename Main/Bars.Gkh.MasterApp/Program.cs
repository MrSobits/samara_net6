namespace Bars.Gkh.MasterApp
{
    using Bars.B4.Modules.Console;

    public static class Program
    {
        public static void Main(string[] args)
        {
            ConsoleRunner.Run<GkhApplication, GkhMasterAppContext, GkhWindowsService>(new[] { "-s" });
        }
    }
}