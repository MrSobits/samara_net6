namespace Bars.Gkh.RegOperator.ViewModels.Import
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.Import;

    /// <summary>
    /// Модель отображения сущностей "Предупреждение про существование начислений при импорте начислений в закрытый период"
    /// </summary>
    public class ExistWarningInChargesToClosedPeriodsImportViewModel : BaseViewModel<ExistWarningInChargesToClosedPeriodsImport>
    {
        /// <summary>
        /// Журнал импорта
        /// </summary>
        public IDomainService<LogImport> LogImport { get; set; }

        /// <summary>
        /// Получить список по идентификатору записи журнала
        /// </summary>
        /// <param name="domainService">Доменный сервис для ExistWarningInChargesToClosedPeriodsImport</param>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <returns>Набор данных для грида</returns>
        public override IDataResult List(IDomainService<ExistWarningInChargesToClosedPeriodsImport> domainService, BaseParams baseParams)
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
                    x.ChargeDescriptorName
                });

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }
    }
}