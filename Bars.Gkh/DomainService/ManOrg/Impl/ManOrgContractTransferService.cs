namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using B4;
    using Entities;

    using Castle.Windsor;

    public class ManOrgContractTransferService : IManOrgContractTransferService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            var contractId = baseParams.Params.GetAs<long>("contractId");

            if (contractId < 1)
                return new BaseDataResult { Success = false, Message = "Не удалось получить договор" };

            var manorg = Container.Resolve<IDomainService<ManOrgContractTransfer>>().GetAll()
                .Where(x => x.Id == contractId)
                .Select(x => x.ManOrgJskTsj.Contragent.Name)
                .FirstOrDefault();

            var robject = Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                .Where(x => x.ManOrgContract.Id == contractId)
                .Select(x => x.RealityObject.Address)
                .FirstOrDefault();

            return new BaseDataResult(new {robject, manorg});
        }
    }
}