namespace Bars.Gkh.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.MetaValueConstructor.DomainModel;

    /// <summary>
    /// Представление для <see cref="EfficiencyRatingPeriod"/>
    /// </summary>
    public class EfficiencyRatingPeriodViewModel : BaseViewModel<EfficiencyRatingPeriod>
    {
        /// <summary>
        /// Домен-сервис <see cref="DataMetaInfo"/>
        /// </summary>
        public IDomainService<DataMetaInfo> DataMetaInfoDomainService { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<EfficiencyRatingPeriod> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var excludedIds = baseParams.Params.GetAs<long[]>("excludedIds");
            var fromEfRating = baseParams.Params.GetAs<bool>("fromEfRating");

            var dataMetaInfoQuery = this.DataMetaInfoDomainService.GetAll().Where(x => x.Level == 2);

            var data = domainService.GetAll()
                .WhereIf(excludedIds.IsNotEmpty(), x => !excludedIds.Contains(x.Id))
                .WhereIf(fromEfRating, x => dataMetaInfoQuery.Any(y => y.Group.Id == x.Group.Id))    // только сконфигурированные конструкторы (есть хотя бы 1 атрибут)
                .Filter(loadParams, this.Container);

            var count = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), count);
        }
    }
}