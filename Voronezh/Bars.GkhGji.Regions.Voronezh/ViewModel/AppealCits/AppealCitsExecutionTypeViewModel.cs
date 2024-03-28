namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using B4;

    using Bars.B4.Utils;

    using Entities;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Slepov.Russian.Morpher;
    using System.Security.Cryptography.X509Certificates;

    public class AppealCitsExecutionTypeViewModel : BaseViewModel<AppealCitsExecutionType>
    {
        public override IDataResult List(IDomainService<AppealCitsExecutionType> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");

            var data = domainService.GetAll()
            .Where(x => x.AppealCits.Id == appealCitizensId)
            .Select(x => new
            {
                x.Id,
                x.AppealExecutionType.Name,
              
            })
            .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }        

    }
}