namespace Bars.Gkh1468.DomainService
{
    using System.Linq;
    using Bars.Gkh.Entities;
    using Castle.Windsor;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh1468.Entities;

    class PublicServiceOrgMunicipalityService : IPublicServiceOrgMunicipalityService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<PublicServiceOrgMunicipality> Domain { get; set; }

        public IDomainService<PublicServiceOrg> DomainPublicServOrg { get; set; }

        public IDomainService<Municipality> DomainMu { get; set; }

        public IDataResult AddMunicipalities(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var publicServOrgId = baseParams.Params.GetAs<long>("publicServOrgId");
                    var muIds = baseParams.Params.GetAs<long[]>("muIds");

                    //Получаем существующие записи Муниципальных образований
                    var existRecs =
                        Domain.GetAll()
                            .Where(x => x.PublicServiceOrg.Id == publicServOrgId)
                            .Select(x => x.Municipality.Id)
                            .Distinct()
                            .AsEnumerable()
                            .ToDictionary(x => x);

                    var org = DomainPublicServOrg.Load(publicServOrgId);

                    foreach (var id in muIds)
                    {
                        if (existRecs.ContainsKey(id))
                            continue;

                        var newObj = new PublicServiceOrgMunicipality
                        {
                            PublicServiceOrg = org,
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