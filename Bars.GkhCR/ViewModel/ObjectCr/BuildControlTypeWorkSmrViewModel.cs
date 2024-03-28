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

    public class BuildControlTypeWorkSmrViewModel : BaseViewModel<BuildControlTypeWorkSmr>
    {
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }
        public IGkhUserManager UserManager { get; set; }
        public override IDataResult List(IDomainService<BuildControlTypeWorkSmr> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var typeWorkId = baseParams.Params.GetAsId("typeWorkId");
            Operator thisOperator = UserManager.GetActiveOperator();
            var contragent = thisOperator?.Contragent;
            var contragentList = OperatorContragentDomain.GetAll()
             .Where(x => x.Contragent != null)
             .Where(x => x.Operator == thisOperator)
             .Select(x => x.Contragent.Id).Distinct().ToList();
            if (contragent != null)
            {
                if (!contragentList.Contains(contragent.Id))
                {
                    contragentList.Add(contragent.Id);
                }
            }

            var data = domainService.GetAll()
                .Where(x => x.TypeWorkCr.Id == typeWorkId)
                .WhereIf(contragentList.Count>0, x=> contragentList.Contains(x.Controller.Id))
                .Select(x => new
                {
                    x.Id,
                    x.CostSum,
                    TypeWorkCrAddWork = x.TypeWorkCrAddWork != null? x.TypeWorkCrAddWork.AdditWork.Name:"",
                    Contragent = x.Contragent.Name,
                    Controller = x.Controller.Name,
                    MonitoringDate = x.ObjectEditDate,
                    x.ObjectCreateDate,
                    x.Description,
                    x.Longitude,
                    x.Latitude,
                    x.PercentOfCompletion,
                    x.VolumeOfCompletion
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<BuildControlTypeWorkSmr> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var value = domainService.GetAll().Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Contragent,
                    x.ObjectCreateDate,
                    x.ObjectEditDate,
                    x.ObjectVersion,
                    x.TypeWorkCr,
                    x.Controller,
                    x.CostSum,
                    x.Longitude,
                    x.Latitude,
                    x.DeadlineMissed,
                    x.Description,
                    x.ImportEntityId,
                    MonitoringDate = x.ObjectEditDate,
                    x.PercentOfCompletion,
                    x.State,
                    TypeWorkCrAddWork = x.TypeWorkCrAddWork != null? new { x.TypeWorkCrAddWork.Id, AdditWorkName = x.TypeWorkCrAddWork.AdditWork.Name }:null,
                    x.VolumeOfCompletion
                }).FirstOrDefault();

            return new BaseDataResult(value);
        }
    }
}
