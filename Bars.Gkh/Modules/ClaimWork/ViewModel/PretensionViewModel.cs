namespace Bars.Gkh.Modules.ClaimWork.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Utils;

    /// <inheritdoc />
    public class PretensionViewModel : BaseViewModel<PretensionClw>
    {

        /// <inheritdoc />
        public override IDataResult List(IDomainService<PretensionClw> domain, BaseParams baseParams)
        {

            var loadParams = this.GetLoadParam(baseParams);
            var dateStart = baseParams.Params.GetAs<DateTime?>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");
            var roId = baseParams.Params.GetAs<long>("address");
            
            var data = domain.GetAll()
                .Where(x => x.ClaimWork.ClaimWorkTypeBase == ClaimWorkTypeBase.Debtor)
                .WhereIf(dateStart.HasValue, x => x.DocumentDate.Value.Date >= dateStart.Value.Date)
                .WhereIf(dateEnd.HasValue, x => x.DocumentDate.Value.Date <= dateEnd.Value.Date)
                .WhereIf(roId != 0, x => x.ClaimWork.RealityObject.Id == roId)
                .Select(x => new
                {
                    x.Id,
                    x.ClaimWork,
                    x.ClaimWork.ClaimWorkTypeBase,
                    x.DocumentDate,
                    x.DateReview,
                    x.Sum,
                    x.Penalty,
                    x.ClaimWork.BaseInfo,
                    Municipality = x.ClaimWork.RealityObject.Municipality.Name,
                    x.ClaimWork.RealityObject.Address

                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}