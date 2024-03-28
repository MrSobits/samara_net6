namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Entities;

    public class AppealCitsResolutionExecutorViewModel : BaseViewModel<AppealCitsResolutionExecutor>
    {
        public override IDataResult List(IDomainService<AppealCitsResolutionExecutor> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("AppealCitsResolution", 0L);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domainService.GetAll()
            .Where(x => x.AppealCitsResolution.Id == id)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Surname,
                x.Patronymic,
                x.PersonalTerm,
                x.Comment,
                x.IsResponsible
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}