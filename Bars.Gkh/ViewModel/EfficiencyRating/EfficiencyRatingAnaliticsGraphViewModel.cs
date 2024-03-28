namespace Bars.Gkh.ViewModel.EfficiencyRating
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.EfficiencyRating;
    using Bars.Gkh.Enums.EfficiencyRating;

    /// <summary>
    /// Представление для <see cref="EfficiencyRatingAnaliticsGraph"/>
    /// </summary>
    public class EfficiencyRatingAnaliticsGraphViewModel : BaseViewModel<EfficiencyRatingAnaliticsGraph>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<EfficiencyRatingAnaliticsGraph> domainService, BaseParams baseParams)
        {
            var entity = domainService.Get(baseParams.Params.GetAsId());
            if (entity.IsNull())
            {
                return BaseDataResult.Error("Не найден график!");
            }

            var data = new
            {
                entity.Id,
                entity.Name,
                entity.Periods,
                entity.AnaliticsLevel,
                entity.ViewParam,
                entity.Category,
                entity.FactorCode,
                ManagingOrganizations = entity.ManagingOrganizations.Select(x => new { x.Id, ManagingOrganization = x.Contragent.Name }).ToArray(),
                entity.Municipalities,
                entity.Data,
                entity.DiagramType
            };

            return new BaseDataResult(data);
        }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<EfficiencyRatingAnaliticsGraph> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var category = baseParams.Params.GetAs<Category>("category");

            var query = domainService.GetAll()
                .WhereIf(category > 0, x => x.Category == category)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Category,
                    x.DiagramType,
                    x.FactorCode,
                    x.ViewParam,
                    x.AnaliticsLevel
                })
                .Filter(loadParams, this.Container);

            var count = query.Count();

            return new ListDataResult(query.Order(loadParams).Paging(loadParams).ToList(), count);
        }
    }
}