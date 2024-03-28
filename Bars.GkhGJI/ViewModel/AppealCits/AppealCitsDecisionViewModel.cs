namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using Entities;

    public class AppealCitsDecisionViewModel : BaseViewModel<AppealCitsDecision>
    {
        public override IDataResult List(IDomainService<AppealCitsDecision> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");

            if (appealCitizensId > 0)
            {

                var data = domainService.GetAll()
                    .Where(x => x.AppealCits.Id == appealCitizensId)
                    .Select(x => new
                    {
                        x.Id,
                        x.DocumentName,
                        x.DocumentDate,
                        x.Apellant,
                        x.AppealNumber,
                        x.AppealDate,
                        x.DocumentNumber,
                        x.TypeDecisionAnswer
                    })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
            }
            else 
            {

                var data = domainService.GetAll()
                    .Where(x => x.DocumentDate > DateTime.Now.AddMonths(-6))
                    .Select(x => new
                    {
                        x.Id,
                        x.DocumentName,
                        x.DocumentDate,
                        x.Apellant,
                        x.AppealNumber,
                        x.AppealDate,
                        x.DocumentNumber,
                        x.TypeDecisionAnswer
                    })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
            }
        }
    }
}