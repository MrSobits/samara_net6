namespace Bars.Gkh.Controllers.SystemInfo
{
    using Bars.B4;

    using Microsoft.AspNetCore.Mvc;

    using Bars.Gkh.Domain;

    /// <summary>
    /// Контроллер для получения информации о системе
    /// </summary>
    public class SystemVersionInfoController : BaseController
    {
        /// <summary>
        /// Интерфейс сервиса для получения текущей версии сборки
        /// </summary>
        public ISystemVersionService SystemVersionService { get; set; }

        /// <summary>
        /// Метод возвращает информацию о версии приложения
        /// </summary>
        /// <returns>Информация о версии</returns>
        public ActionResult GetBuildInfo()
        {
            return this.SystemVersionService.GetVersionInfo().ToJsonResult();
        }
    }
}
