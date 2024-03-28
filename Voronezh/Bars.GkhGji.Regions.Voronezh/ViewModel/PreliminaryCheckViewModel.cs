namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using Entities;

    public class PreliminaryCheckViewModel : BaseViewModel<PreliminaryCheck>
    {
        public override IDataResult List(IDomainService<PreliminaryCheck> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");

            var data = domainService.GetAll()
                .Where(x => x.AppealCits.Id == appealCitizensId)
                .Select(x => new
                    {
                        x.Id,
                        CheckDate = x.CheckDate != DateTime.MinValue ? x.CheckDate : null,
                        Contragent = x.Contragent.Name,
                        Inspector = x.Inspector.Fio,
                        x.FileInfo,
                        x.PreliminaryCheckNumber,
                        x.PreliminaryCheckResult,
                        x.Result
                    })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}