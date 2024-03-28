namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Enum;
    using Bars.Gkh.Overhaul.Hmao.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EconFeasibilityCalcResultViewModel : BaseViewModel<EconFeasibilityCalcResult>
    {
        public override IDataResult List(IDomainService<EconFeasibilityCalcResult> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Adress = x.RoId.Address,
                    Municipality = x.RoId.Municipality.Name,
                    MoSettlement = x.RoId.MoSettlement != null? x.RoId.MoSettlement.Name: "",
                    x.YearStart,
                    x.YearEnd,
                    x.TotatRepairSumm,
                    SquareCost = x.SquareCost.Cost,
                    x.TotalSquareCost,
                    x.CostPercent,
                    x.Decision //= EnumToTextHelper.EconFeasibilityToStrinng(x.Decision)
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
      
    }

}
