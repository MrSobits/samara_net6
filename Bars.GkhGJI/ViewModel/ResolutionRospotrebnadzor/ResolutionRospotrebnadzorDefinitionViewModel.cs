namespace Bars.GkhGji.ViewModel.ResolutionRospotrebnadzor
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    /// <summary>
    /// View Определение постановления Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorDefinitionViewModel
        : ResolutionRospotrebnadzorDefinitionViewModel<ResolutionRospotrebnadzorDefinition>
    {
    }

    /// <summary>
    /// Generic View Определение постановления Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorDefinitionViewModel<T> : BaseViewModel<T>
        where T : ResolutionRospotrebnadzorDefinition
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
                    x.DocumentNum,
                    x.DocumentDate,
                    x.TypeDefinition,
                    x.ExecutionDate,
                    x.Description,
                    IssuedDefinition = x.IssuedDefinition.Fio
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}