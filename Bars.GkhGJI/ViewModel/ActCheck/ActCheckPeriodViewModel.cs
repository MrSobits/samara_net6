namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Модель-представление для работы с периодами проверки
    /// </summary>
    public class ActCheckPeriodViewModel : BaseViewModel<ActCheckPeriod>
    {
        /// <summary>
        /// Метод формирующий список периодов проверки по заданному ID документа
        /// </summary>
        /// <param name="domainService">Домен-сервис периодов</param>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IDataResult List(IDomainService<ActCheckPeriod> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId")
                                 ? baseParams.Params["documentId"].ToLong()
                                 : 0;

            var data = domainService.GetAll()
                .Where(x => x.ActCheck.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DateCheck,
                    x.DateStart,
                    x.DateEnd,
                    x.DurationDays,
                    x.DurationHours
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }

        /// <summary>
        /// Метод возвращающий данные периода проверки по ID
        /// </summary>
        /// <param name="domainService">Домен-сервис периодов</param>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IDataResult Get(IDomainService<ActCheckPeriod> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return obj != null ? new BaseDataResult(
                new
                    {
                        obj.Id,
                        obj.ActCheck,
                        obj.DateCheck,
                        obj.DateStart,
                        obj.DateEnd,
                        obj.DurationDays,
                        obj.DurationHours,
                        TimeStart = obj.DateStart.HasValue ? obj.DateStart.Value.ToString("HH:mm") : string.Empty,
                        TimeEnd = obj.DateEnd.HasValue ? obj.DateEnd.Value.ToString("HH:mm") : string.Empty,
                    }) : new BaseDataResult();
        }
    }
}