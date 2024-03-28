using System;
using System.Linq;
using Bars.B4;
using Bars.GkhGji.Regions.Tatarstan.Entities;

namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    public class GisGmpPatternViewModel : BaseViewModel<GisGmpPattern>
    {
        public override IDataResult List(IDomainService<GisGmpPattern> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll().Select(x => new
            {
                x.Id,
                Municipality = x.Municipality.Name,
                x.DateStart,
                x.DateEnd,
                x.PatternCode
            }).Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}