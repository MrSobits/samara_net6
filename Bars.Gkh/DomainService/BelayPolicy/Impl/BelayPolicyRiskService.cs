namespace Bars.Gkh.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class BelayPolicyRiskService : IBelayPolicyRiskService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddKindRisk(BaseParams baseParams)
        {
            using (var tx = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var belayPolicyId = baseParams.Params.GetAs<long>("belayPolicyId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds");

                    var service = Container.Resolve<IDomainService<BelayPolicyRisk>>();
                    var serviceKindRisk = Container.Resolve<IDomainService<KindRisk>>();
                    var servicePolicy = Container.Resolve<IDomainService<BelayPolicy>>();

                    var existRecs = service.GetAll()
                        .Where(x => x.BelayPolicy.Id == belayPolicyId)
                        .Where(x => objectIds.Contains(x.KindRisk.Id))
                        .Select(x => x.KindRisk.Id)
                        .Distinct()
                        .ToArray();

                    var policy = servicePolicy.Load(belayPolicyId);

                    foreach (var id in objectIds)
                    {
                        if(existRecs.Contains(id))
                            continue;

                        var newRec = new BelayPolicyRisk
                            {
                                KindRisk = serviceKindRisk.Load(id),
                                BelayPolicy = policy
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