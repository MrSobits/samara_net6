namespace Bars.Gkh.RegOperator.DomainService.Import.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.Gkh.RegOperator.Entities.Import;
    
    /// <summary>
    /// Сервис для работы с шапкой импорта оплаты в закрытый период
    /// </summary>
    class HeaderOfPaymentsToClosedPeriodsImportService : IHeaderOfClosedPeriodsImportService
    {
        /// <summary>Шапака импорта оплаты в закрытый период</summary>
        public IDomainService<HeaderOfPaymentsToClosedPeriodsImport> HeaderOfPaymentsToClosedPeriodsImportDomain { get; set; }

        /// <summary>
        /// Заполнить параметры для повторного импорта
        /// </summary>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <param name="task">Задача сервера вычислений, которая разбирала импорт</param>
        public void FillBaseParamsForReImport(BaseParams baseParams, TaskEntry task)
        {
            var header = this.HeaderOfPaymentsToClosedPeriodsImportDomain.GetAll()
                .Where(x => x.Task == task) // Связь с "шапкой" реализована через задачу на сервере вычислений, которая разбирала импорт
                .Select(x => new
                {
                    PeriodId = x.Period.Id,
                    x.IsUpdateSaldoIn,
                    x.ExternalRkcId
                })
                .FirstOrDefault();
            baseParams.Params["periodId"] = header.PeriodId;
            baseParams.Params["updateSaldoIn"] = header.IsUpdateSaldoIn;
            baseParams.Params["externalRkcId"] = header.ExternalRkcId;
        }
    }
}
