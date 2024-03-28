namespace Bars.Gkh.RegOperator.Controllers.Import
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.RegOperator.DomainService.Import;
    
    /// <summary>
    /// Котроллер для импортов в закрытый период
    /// </summary>
    public class ClosedPeriodsImportController : BaseController
    {
        /// <summary>
        /// Сервис для работы с импортами в закрытый период
        /// </summary>
        public IClosedPeriodsImportService Service { get; set; }
        
        /// <summary>
        /// Подтвердить атоматическое сопоставление лицевого счёта
        /// </summary>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <returns>Удалось ли подтвердить</returns>
        public ActionResult ConfirmAutoComparePersonalAccount(BaseParams baseParams)
        {
            return this.Service.ConfirmAutoComparePersonalAccount(baseParams);
        }

        /// <summary>
        /// Получить список логов
        /// </summary>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <returns>Набор данных, пригодный для отображения в гриде</returns>
        public ActionResult LogsList(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.LogsList(baseParams));
        }

        /// <summary>
        /// Сопоставить лицевой счёт вручуню
        /// </summary>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <returns>Удалось ли сопоставить</returns>
        public ActionResult ManualComparePersonalAccount(BaseParams baseParams)
        {
            return this.Service.ManualComparePersonalAccount(baseParams);
        }        

        /// <summary>
        /// Повторить импорт
        /// </summary>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <returns>Удалось ли запустить</returns>
        public ActionResult ReImport(BaseParams baseParams)
        {
            return this.Service.ReImport(baseParams);
        }

        /// <summary>
        /// Получить список задач, выполняемых в текущий момент
        /// </summary>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <returns>Набор данных, пригодный для отображения в гриде</returns>
        public ActionResult RunningTasksList(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.RunningTasksList(baseParams));
        }        
    }
}