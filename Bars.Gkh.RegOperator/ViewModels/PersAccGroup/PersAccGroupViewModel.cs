namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Вьюха для сущности <see cref="PersAccGroup"/>
    /// </summary>
    public class PersAccGroupViewModel : BaseViewModel<PersAccGroup>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<PersAccGroup> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var isSystem = baseParams.Params.GetAs<int>("isSystem", ignoreCase: true);

            var data = domainService.GetAll()
                .WhereIf(isSystem > 0, x => x.IsSystem == (YesNo) isSystem)
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
        }
    }
}
