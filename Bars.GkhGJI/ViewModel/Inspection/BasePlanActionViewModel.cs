namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;
    using Enums;

    public class BasePlanActionViewModel : BasePlanActionViewModel<BasePlanAction>
    {
    }

    public class BasePlanActionViewModel<T> : BaseViewModel<T>
        where T : BasePlanAction
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /**
             * planId - идентификатор плана
             */

            var planId = baseParams.Params.GetAs<long>("planId");
            var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);
            
            var data = domainService.GetAll()
                .WhereIf(planId > 0, x => x.Plan.Id == planId)
                .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                .Select(x => new
                    {
                        x.Id,
                        Municipality = x.Contragent.Municipality.Name,
                        MoSettlement = x.Contragent.MoSettlement.Name,
                        x.Contragent.FiasJuridicalAddress.PlaceName,
                        SubjectName = x.PersonInspection == PersonInspection.Organization ? x.Contragent.Name: x.PhysicalPerson,
                        Plan = x.Plan.Name,
                        x.DateStart,
                        x.DateEnd,
                        x.State
                    })
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}