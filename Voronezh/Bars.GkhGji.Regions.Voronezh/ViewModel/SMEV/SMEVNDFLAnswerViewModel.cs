namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class SMEVNDFLAnswerViewModel : BaseViewModel<SMEVNDFLAnswer>
    {
        public override IDataResult List(IDomainService<SMEVNDFLAnswer> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("smevNDFL", 0L);

            var data = domainService.GetAll()
                .Where(x=> x.SMEVNDFL.Id == id)
                .Select(x=> new
                {
                    x.Id,
                    x.SMEVNDFL,
                    x.INNUL,
                    x.KPP,
                    x.OrgName,
                    x.Rate,
                    x.RevenueCode,
                    x.Month,
                    x.RevenueSum,
                    x.RecoupmentCode,
                    x.RecoupmentSum,
                    x.DutyBase,
                    x.DutySum,
                    x.UnretentionSum,
                    x.RevenueTotalSum

        })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


     
    }
}
