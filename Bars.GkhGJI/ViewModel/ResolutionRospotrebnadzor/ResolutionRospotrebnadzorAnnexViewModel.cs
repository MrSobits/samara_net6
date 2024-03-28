namespace Bars.GkhGji.ViewModel.ResolutionRospotrebnadzor
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    /// <summary>
    /// View Приложение постановления Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorAnnexViewModel
        : ResolutionRospotrebnadzorAnnexViewModel<ResolutionRospotrebnadzorAnnex>
    {
    }

    /// <summary>
    /// Generic View Приложение постановления Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorAnnexViewModel<T> : BaseViewModel<T>
        where T : ResolutionRospotrebnadzorAnnex
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
            var loadParams = baseParams.GetLoadParam();
            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domainService.GetAll()
                .Where(x => x.Resolution.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentName,
                    x.Description,
                    x.File,
                    x.SignedFile,
                    x.MessageCheck
                })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}