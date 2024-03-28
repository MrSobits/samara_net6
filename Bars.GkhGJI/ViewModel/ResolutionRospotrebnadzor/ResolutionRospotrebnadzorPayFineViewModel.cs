namespace Bars.GkhGji.ViewModel.ResolutionRospotrebnadzor
{
    using System.Linq;

    using Bars.B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// View Постановление Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorPayFineViewModel
        : ResolutionRospotrebnadzorPayFineViewModel<ResolutionRospotrebnadzorPayFine>
    {
    }

    /// <summary>
    /// Generic View Постановление Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorPayFineViewModel<T> : BaseViewModel<T>
        where T : ResolutionRospotrebnadzorPayFine
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
            var loadParam = baseParams.GetLoadParam();
            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domainService.GetAll()
                .Where(x => x.Resolution.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.TypeDocumentPaid,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.Amount,
                    x.GisUip
                })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}