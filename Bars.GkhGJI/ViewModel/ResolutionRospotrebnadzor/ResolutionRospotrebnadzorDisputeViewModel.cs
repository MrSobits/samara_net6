namespace Bars.GkhGji.ViewModel.ResolutionRospotrebnadzor
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// View Оспаривание постановления Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorDisputeViewModel
        : ResolutionRospotrebnadzorDisputeViewModel<ResolutionRospotrebnadzorDispute>
    {
    }

    /// <summary>
    /// Generic View Оспаривание постановления Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorDisputeViewModel<T> : BaseViewModel<T>
        where T : ResolutionRospotrebnadzorDispute
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domainService.GetAll()
                .Where(x => x.Resolution.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNum,
                    x.DocumentDate,
                    x.ProsecutionProtest,
                    x.Description,
                    x.Appeal,
                    x.File,
                    Court = x.Court.Name,
                    Instance = x.Instance.Name,
                    CourtVerdict = x.CourtVerdict.Name,
                    Lawyer = x.Lawyer.Fio
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}