namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System.Linq;
    using B4;
    using Gkh.Entities;
    using Entities;
    using Castle.Windsor;
    using Gkh.Domain;
    using Gkh.Entities.Dicts;
    using Gkh.Utils;
    using Overhaul.Entities;

    public class RealEstateTypeMunicipalityService : IRealEstateTypeMunicipalityService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddMunicipality(BaseParams baseParams)
        {
            var objectIds = baseParams.Params.GetAs<long[]>("objectIds");
            var retId = baseParams.Params.GetAsId("retId");

            var municipalityService = Container.Resolve<IDomainService<Municipality>>();
            var retService = Container.Resolve<IDomainService<RealEstateType>>();
            var service = Container.Resolve<IDomainService<RealEstateTypeMunicipality>>();

            try
            {
                Container.InTransaction(() =>
                {
                    var existRecs = service.GetAll()
                        .Where(x => x.RealEstateType.Id == retId)
                        .Where(x => objectIds.Contains(x.Municipality.Id))
                        .Select(x => x.Municipality.Id)
                        .ToHashSet();

                    var ret = retService.Load(retId);

                    foreach (var id in objectIds)
                    {
                        if (existRecs.Contains(id))
                        {
                            continue;
                        }

                        var newRec = new RealEstateTypeMunicipality
                        {
                            Municipality = municipalityService.Load(id),
                            RealEstateType = ret
                        };

                        service.Save(newRec);
                    }
                });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }

            return new BaseDataResult();
        }
    }
}