namespace Bars.Gkh.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using B4.DataAccess;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class BelayPolicyMkdService : IBelayPolicyMkdService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddPolicyMkdObjects(BaseParams baseParams)
        {
            using (var tx = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var belayPolicyId = baseParams.Params.GetAs<long>("belayPolicyId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds");

                    var service = Container.Resolve<IDomainService<BelayPolicyMkd>>();
                    var serviceRobject = Container.Resolve<IDomainService<RealityObject>>();
                    var servicePolicy = Container.Resolve<IDomainService<BelayPolicy>>();

                    var existRecs =
                        service.GetAll()
                            .Where(x => x.BelayPolicy.Id == belayPolicyId)
                            .Where(x => objectIds.Contains(x.RealityObject.Id))
                            .Select(x => x.RealityObject.Id)
                            .Distinct()
                            .ToList();

                    var policy = servicePolicy.Load(belayPolicyId);

                    foreach (var id in objectIds)
                    {
                        if (existRecs.Contains(id))
                            continue;

                        var newRec = new BelayPolicyMkd
                            {
                                RealityObject = serviceRobject.Load(id),
                                BelayPolicy = policy,
                                IsExcluded = false
                            };

                        service.Save(newRec);
                    }

                    tx.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException exc)
                {
                    tx.Rollback();
                    return new BaseDataResult {Success = false, Message = exc.Message};
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
}