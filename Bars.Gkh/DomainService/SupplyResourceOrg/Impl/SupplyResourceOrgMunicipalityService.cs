namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;

    using Entities;

    using Castle.Windsor;

    public class SupplyResourceOrgMunicipalityService : ISupplyResourceOrgMunicipalityService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<SupplyResourceOrgMunicipality> Domain { get; set; }

        public IDomainService<SupplyResourceOrg> DomainSupplyResOrg { get; set; }

        public IDomainService<Municipality> DomainMu { get; set; }

        public IDataResult AddMunicipalities(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var supplyResOrgId = baseParams.Params.GetAs<long>("supplyResOrgId");
                    var muIds = baseParams.Params.GetAs<long[]>("muIds");

                    //Получаем существующие записи Муниципальных образований
                    var existRecs =
                        Domain.GetAll()
                              .Where(x => x.SupplyResourceOrg.Id == supplyResOrgId)
                              .Select(x => x.Municipality.Id)
                              .Distinct()
                              .AsEnumerable()
                              .ToDictionary(x => x);

                    var org = DomainSupplyResOrg.Load(supplyResOrgId);

                    // По переданным id инспекторов если их нет в списке существующих, то добавляем
                    foreach (var id in muIds)
                    {
                        if (existRecs.ContainsKey(id))
                            continue;

                        var newObj = new SupplyResourceOrgMunicipality
                        {
                            SupplyResourceOrg = org,
                            Municipality = DomainMu.Load(id)
                        };

                        Domain.Save(newObj);
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