namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Castle.Windsor;
    using Gkh.Entities.Dicts;
    using Overhaul.Entities;

    public class RealEstateTypeMunicipalityService : IRealEstateTypeMunicipalityService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddMunicipality(BaseParams baseParams)
        {
            var objectIds = baseParams.Params.GetAs<long[]>("objectIds");
            var retId = baseParams.Params.GetAs<long>("retId");

            var municipalityService = Container.Resolve<IDomainService<Municipality>>();
            var retService = Container.Resolve<IDomainService<RealEstateType>>();
            var service = Container.Resolve<IDomainService<RealEstateTypeMunicipality>>();

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var existRecs = service.GetAll()
                        .Where(x => x.RealEstateType.Id == retId)
                        .Where(x => objectIds.Contains(x.Municipality.Id))
                        .Select(x => x.Municipality.Id)
                        .ToList();

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

                    tr.Commit();
                }
                catch (ValidationException e)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }
    }
}