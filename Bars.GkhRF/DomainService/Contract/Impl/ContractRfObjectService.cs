namespace Bars.GkhRf.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    using Castle.Windsor;

    public class ContractRfObjectService : IContractRfObjectService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddContractObjects(BaseParams baseParams)
        {
            try
            {
                var contractRfId = baseParams.Params["contractRfId"].ToLong();

                var objectIds = baseParams.Params["objectIds"].ToString().Split(',');

                if (objectIds.Length > 0)
                {
                    var contractRf = Container.Resolve<IDomainService<ContractRf>>().Load(contractRfId);

                    var service = Container.Resolve<IDomainService<ContractRfObject>>();

                    // получаем у контроллера дома что бы не добавлять их повторно
                    var exsistingContractRfObjects = service.GetAll().Where(x => x.ContractRf.Id == contractRfId).Select(x => x.RealityObject.Id).ToList();

                    foreach (var id in objectIds.Select(x => x.ToLong()))
                    {
                        if (exsistingContractRfObjects.Contains(id.ToLong()))
                            continue;

                        var newContractRfObject = new ContractRfObject
                        {
                            RealityObject = new RealityObject { Id = id },
                            ContractRf = new ContractRf { Id = contractRfId },
                            IncludeDate = contractRf.DateBegin,
                            TypeCondition = TypeCondition.Include
                        };

                        service.Save(newContractRfObject);
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }
    }
}