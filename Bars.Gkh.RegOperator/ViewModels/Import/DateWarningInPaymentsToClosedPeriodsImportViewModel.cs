namespace Bars.Gkh.RegOperator.ViewModels.Import
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;    
    using Bars.Gkh.RegOperator.Entities.Import;

    /// <summary>
    /// Модель отображения сущностей "Предупреждение про даты при импорте в закрытый период"
    /// </summary>
    class DateWarningInPaymentsToClosedPeriodsImportViewModel : BaseViewModel<DateWarningInPaymentsToClosedPeriodsImport>
    {
        /// <summary>
        /// Журнал импорта
        /// </summary>
        public IDomainService<LogImport> LogImport { get; set; }

        /// <summary>
        /// Получить список по идентификатору записи журнала
        /// </summary>
        /// <param name="domainService">Доменный сервис для DateWarningInPaymentsToClosedPeriodsImport</param>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <returns>Набор данных для грида</returns>
        public override IDataResult List(IDomainService<DateWarningInPaymentsToClosedPeriodsImport> domainService, BaseParams baseParams)
        {            
            var loadParam = this.GetLoadParam(baseParams);

            // Получить задачу сервера вычислений (по ней реализована связь)
            var logImportId = baseParams.Params.GetAsId("logImportId");
            var task = this.LogImport.Get(logImportId)?.Task;

            var data = domainService.GetAll()
                .Where(x => x.Task == task)
                .Filter(loadParam, this.Container)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.Message,
                    x.PaymentDate
                });

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }
    }
}
