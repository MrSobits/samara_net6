namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class SupplyResourceOrgServService : ISupplyResourceOrgServService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddTypeServiceObjects(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var supplyResOrgId = baseParams.Params.GetAs<long>("supplyResOrgId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds");

                    var service = this.Container.Resolve<IDomainService<SupplyResourceOrgService>>();
                    var serviceType = Container.Resolve<IDomainService<TypeService>>();
                    var serviceOrg = Container.Resolve<IDomainService<SupplyResourceOrg>>();

                    // получаем у контроллера существующие типы услуг что бы не добавлять их повторно
                    var existRecs =
                        service.GetAll()
                               .Where(x => x.SupplyResourceOrg.Id == supplyResOrgId)
                               .Select(x => x.TypeService.Id)
                               .Distinct()
                               .AsEnumerable()
                               .ToDictionary(x => x);

                    var org = serviceOrg.Load(supplyResOrgId);

                    foreach (var id in objectIds)
                    {
                        if (existRecs.ContainsKey(id))
                            continue;

                        var newRec = new SupplyResourceOrgService
                            {
                                SupplyResourceOrg = org,
                                TypeService = serviceType.Load(id)
                            };

                        service.Save(newRec);
                    }
                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException exc)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = exc.Message};
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}