namespace Bars.Esia.OAuth20.App.Application
{
    using System.ComponentModel;

    using Bars.B4.Modules.Console.WindowsService;

    /// <summary>
    /// Installer сервиса модуля авторизации
    /// </summary>
    /// <remarks>
    /// Ставит модуль в виде службы-сервиса Windows
    /// </remarks>
    [RunInstaller(true)]
    public class AuthAppServiceInstaller : BaseServiceInstaller
    {
    }
}