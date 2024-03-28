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

    public class AppealCitsResolutionViewModel : BaseViewModel<AppealCitsResolution>
    {
        public override IDataResult List(IDomainService<AppealCitsResolution> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
            //var isFiltered = baseParams.Params.GetAs<bool>("isFiltered");

            var data = domainService.GetAll()
            .Where(x => x.AppealCits.Id == appealCitizensId)
            .Select(x => new
            {
                x.Id,
                x.ResolutionText,
                x.ResolutionTerm,
                x.ResolutionDate,
                x.ResolutionAuthor,
                x.ResolutionContent,
                x.Executed
                //x.ImportId,
                //x.ParentId
                //ParentResolutionData = !string.IsNullOrEmpty(x.ParentId) ? GetParentData(x.ParentId, x.AppealCits.Id) : ""
            })
            .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<AppealCitsResolution> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            int id = Convert.ToInt32(baseParams.Params.Get("id"));

            var data = domain.GetAll()
                 .WhereIf(id != 0, x => x.Id == id)
                   .Select(x => new
                   {
                       x.Id,
                       x.ResolutionText,
                       x.ResolutionTerm,
                       x.ResolutionDate,
                       x.ResolutionAuthor,
                       x.ResolutionContent,
                       x.Executed,
                       x.ObjectCreateDate,
                       x.ObjectEditDate,
                       x.ObjectVersion,
                       x.ImportId,
                       x.ParentId,
                       ParentResolutionData = !string.IsNullOrEmpty(x.ParentId) ? GetParentData(x.ParentId, x.AppealCits.Id) : ""
                   })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        private string GetParentData(string parentId, long appealCit)
        {
            var domain = Container.Resolve<IDomainService<AppealCitsResolution>>();
            try
            {
                return domain.GetAll()
                    .Where(x => x.AppealCits.Id == appealCit && x.ImportId == parentId)
                    .Select(x => $"{x.ResolutionAuthor}, {x.ResolutionText}, {x.ResolutionDate.ToShortDateString()}")
                    .FirstOrDefault();
            }
            finally
            {
                Container.Release(domain);
            }
        }
    }
}