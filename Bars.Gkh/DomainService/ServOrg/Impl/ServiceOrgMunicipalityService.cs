namespace Bars.Gkh.DomainService
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Entities;
    using Castle.Windsor;

    public class ServiceOrgMunicipalityService : IServiceOrgMunicipalityService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ServiceOrgMunicipality> Domain { get; set; }

        public IDomainService<ServiceOrganization> DomainServOrg { get; set; }

        public IDomainService<Municipality> DomainMu { get; set; }

        public IDataResult AddMunicipalities(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var servorgId = baseParams.Params.GetAs<long>("servorgId");
                    var muIds = baseParams.Params.GetAs<long[]>("muIds");

                    //Получаем существующие записи Муниципальных образований
                    var existRecs =
                        Domain.GetAll()
                              .Where(x => x.ServOrg.Id == servorgId)
                              .Select(x => x.Municipality.Id)
                              .Distinct()
                              .AsEnumerable()
                              .ToDictionary(x => x);

                    var org = DomainServOrg.Load(servorgId);

                    // По переданным id инспекторов если их нет в списке существующих, то добавляем
                    foreach (var id in muIds)
                    {
                        if (existRecs.ContainsKey(id))
                            continue;

                        var newObj = new ServiceOrgMunicipality
                        {
                            ServOrg = org,
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