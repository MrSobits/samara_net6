namespace Bars.GkhCr.ViewModel.ObjectCr
{
    using System;
    using System.Linq;
    using B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;
    using Gkh.Domain;

    public class TypeWorkCrAddWorkViewModel : BaseViewModel<TypeWorkCrAddWork>
    {
        public override IDataResult List(IDomainService<TypeWorkCrAddWork> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var typeWorkId = baseParams.Params.GetAsId("typeWorkId");
            var required = baseParams.Params.GetAs<bool>("required",false);
            var data = domainService.GetAll()
                .Where(x => x.TypeWorkCr.Id == typeWorkId)
                .WhereIf(required, x=> x.Required)
                .Select(x => new
                {
                    x.Id,
                    AdditWorkName = x.AdditWork.Name,
                    x.DateEndWork,
                    x.DateStartWork,
                    x.Queue,
                    x.Required
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}
