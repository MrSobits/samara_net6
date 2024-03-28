namespace Bars.GkhDi.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    public class RealityObjGroupService : IRealityObjGroupService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddRealityObj(BaseParams baseParams)
        {
            try
            {
                var groupDiId = baseParams.Params.GetAs<long>("groupDiId");

                if (!string.IsNullOrEmpty(baseParams.Params["objectIds"].ToStr()))
                {
                    var realObjIds = baseParams.Params["objectIds"].ToStr().Split(',').Select(x => x.ToLong());
                    var service = this.Container.Resolve<IDomainService<RealityObjGroup>>();
                    var existingRealityObjIds = service.GetAll().Where(x => x.GroupDi.Id == groupDiId).Select(x => x.RealityObject.Id).ToList().Distinct();

                    foreach (var realObjId in realObjIds)
                    {
                        if (existingRealityObjIds.Contains(realObjId))
                        {
                            continue;
                        }

                        var realityObjGroup = new RealityObjGroup
                        {
                            GroupDi = new GroupDi { Id = groupDiId },
                            RealityObject = new RealityObject { Id = realObjId }
                        };

                        service.Save(realityObjGroup);
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
