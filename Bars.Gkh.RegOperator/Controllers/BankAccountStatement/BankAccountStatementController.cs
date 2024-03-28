namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using DomainService;
    using Entities;
    using Gkh.Domain;
    using ViewModels;

    /// <summary>
    /// Контроллер для банковской выписки
    /// </summary>
    public class BankAccountStatementController : B4.Alt.DataController<BankAccountStatement>
    {
        /// <summary>
        /// Сервис для банковской выписки
        /// </summary>
        public IBankAccountStatementService BankAccountStatementService { get; set; }

        /// <summary>
        /// Привязать документ.
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult LinkDocument(BaseParams baseParams)
        {
            return this.BankAccountStatementService.LinkDocument(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Показать детализацию.
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListDetail(BaseParams baseParams)
        {
            return this.BankAccountStatementService.ListDetail(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Создание записи
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Успешность операции</returns>
        public override ActionResult Create(BaseParams baseParams)
        {
            var result = this.BankAccountStatementService.SaveStatement(baseParams);
            return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Обновление записи
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Успешность операции</returns>
        public override ActionResult Update(BaseParams baseParams)
        {
            var result = this.BankAccountStatementService.SaveStatement(baseParams);
            return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Список доступных Р/С для распределения
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список Р/С</returns>
        public ActionResult ListAccountNumbers(BaseParams baseParams)
        {
            return this.BankAccountStatementService.ListAccountNumbers(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Экспорт в excel
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список Р/С</returns>
        public ActionResult Export(BaseParams baseParams)
        {
            IDataExportService export = null;
            try
            {
                export = this.Container.Resolve<IDataExportService>("BankAccountStatementDataExport");
                return export.ExportData(baseParams);
            }
            finally
            {
                if (export != null)
                {
                    this.Container.Release(export);
                }
            }
            //  return this.BankAccountStatementService.Export(this.ViewModel, baseParams);
        }
    }
}