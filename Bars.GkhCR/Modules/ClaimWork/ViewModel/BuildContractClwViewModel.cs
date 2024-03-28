using Bars.GkhCr.Modules.ClaimWork.Entities;

namespace Bars.GkhCr.Modules.ClaimWork.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.DomainService;

    public class BuildContractClwViewModel : BaseViewModel<BuildContractClaimWork>
    {
        public override IDataResult List(IDomainService<BuildContractClaimWork> domainService, BaseParams baseParams)
        {
            var service = Container.Resolve<IBaseClaimWorkService<BuildContractClaimWork>>();

            try
            {
                var totalCount = 0;
                var result = service.GetList(baseParams, true, ref totalCount);
                return new ListDataResult(result, totalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public override IDataResult Get(IDomainService<BuildContractClaimWork> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var value = domainService.GetAll().Where(x => x.Id == id )
                .Select(x => new
                {
                    x.Id,
                    Builder = x.BuildContract.Builder.Contragent.Name,
                    BuildContract = x.BuildContract.Id,
                    x.BuildContract.Builder.Contragent.Inn,
                    x.BuildContract.DocumentNum,
                    x.BuildContract.DocumentDateFrom,
                    x.BuildContract.DateEndWork,
                    x.ClaimWorkTypeBase,
                    x.CreationType,
                    x.CountDaysDelay,
                    ObjCrId = x.BuildContract.ObjectCr.Id,
                    x.BuildContract.ObjectCr.RealityObject.Address,
                    x.State,
                    StateName = x.State != null ? x.State.Name : string.Empty,
                    x.IsDebtPaid,
                    x.DebtPaidDate
                }).FirstOrDefault();

            return new BaseDataResult(value);
        }
    }
}