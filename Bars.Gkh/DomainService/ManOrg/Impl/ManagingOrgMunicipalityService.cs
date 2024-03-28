namespace Bars.Gkh.DomainService
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Entities;
    using Castle.Windsor;

    public class ManagingOrgMunicipalityService : IManagingOrgMunicipalityService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ManagingOrgMunicipality> Domain { get; set; }

        public IDomainService<ManagingOrganization> DomainManOrg { get; set; }

        public IDomainService<Municipality> DomainMu { get; set; }

        public IDataResult AddMunicipalities(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var manorgId = baseParams.Params.GetAs<long>("manorgId");
                    var muIds = baseParams.Params.GetAs<long[]>("muIds");

                    //Получаем существующие записи Муниципальных образований
                    var existRecs =
                        Domain.GetAll()
                              .Where(x => x.ManOrg.Id == manorgId)
                              .Select(x => x.Municipality.Id)
                              .Distinct()
                              .AsEnumerable()
                              .ToDictionary(x => x);

                    var org = DomainManOrg.Load(manorgId);

                    // По переданным id инспекторов если их нет в списке существующих, то добавляем
                    foreach (var id in muIds)
                    {
                        if (existRecs.ContainsKey(id))
                            continue;

                        var newObj = new ManagingOrgMunicipality
                        {
                            ManOrg = org,
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