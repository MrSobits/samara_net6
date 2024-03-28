namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;

    using Entities;

    using Castle.Windsor;

    public class SupplyResourceOrgRealtyObjectService : ISupplyResourceOrgRealtyObjectService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<SupplyResourceOrgRealtyObject> DomainSupplyResOrgRealObj { get; set; }

        public IDomainService<SupplyResourceOrg> DomainSupplyResOrg { get; set; }

        public IDomainService<RealityObject> DomainRealObj { get; set; }

        public IDataResult AddRealtyObjects(BaseParams baseParams)
        {
            var supplyResOrgId = baseParams.Params.GetAs<long>("supplyResOrgId");
            var roIds = baseParams.Params.GetAs<long[]>("roIds");

            // Получаем существующие записи Жилых домов
            var existRecs = this.DomainSupplyResOrgRealObj.GetAll()
                        .Where(x => x.SupplyResourceOrg.Id == supplyResOrgId)
                        .Select(x => x.RealityObject.Id)
                        .Distinct()
                        .AsEnumerable()
                        .ToDictionary(x => x);

            var org = this.DomainSupplyResOrg.Load(supplyResOrgId);

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    // По переданным id если их нет в списке существующих, то добавляем
                    foreach (var id in roIds)
                    {
                        if (existRecs.ContainsKey(id))
                        {
                            continue;
                        }

                        var newObj = new SupplyResourceOrgRealtyObject
                        {
                            SupplyResourceOrg = org,
                            RealityObject = this.DomainRealObj.Load(id)
                        };

                        this.DomainSupplyResOrgRealObj.Save(newObj);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = e.Message };
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