namespace Bars.Gkh.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Контроллер для Контрагент
    /// </summary>
    public class ContragentController : B4.Alt.DataController<Contragent>
    {
        public IContragentService ContragentService { get; set; }

        /// <summary>
        /// Экспортировать в Excel
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("ContragentDataExport");

            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally
            {
                this.Container.Release(export);
            }
        }

        /// <summary>
        /// Получить список без потомков
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult ListExceptChildren(BaseParams baseParams)
        {
            return this.ContragentService.ListExceptChildren(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Получить список контрагентов для специального счета
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult ListForSpecialAccount(BaseParams baseParams)
        {
            return this.ContragentService.ListForSpecialAccount(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Получить контакт действующего руководителя для контрагента
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult GetActualManagerContact(BaseParams baseParams)
        {
            return this.ContragentService.GetActualManagerContact(baseParams).ToJsonResult();
        }

        public ActionResult GetContactsFromDL(BaseParams baseParams, Int64 contragentId)
        {
            return this.ContragentService.GetContactsFromDL(baseParams, contragentId).ToJsonResult();
        }

        public ActionResult UpdateContactsFromDL(BaseParams baseParams, Int64 contragentId)
        {
            return this.ContragentService.UpdateContactsFromDL(baseParams, contragentId).ToJsonResult();
        }

        /// <summary>
        /// Вернуть сгенерированный код поставщика
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult GenerateProviderCode(BaseParams baseParams)
        {
            return this.ContragentService.GenerateProviderCode(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Вернуть всех активных контрагентов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        public ActionResult GetAllActiveContragent(BaseParams baseParams)
        {
            return this.ContragentService.GetAllActiveContragent(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Получить дополнительные роли контрагента
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        public ActionResult ListADditionRole(BaseParams baseParams)
        {
            return this.ContragentService.ListAdditionRole(baseParams).ToJsonResult();
        }
    }
}