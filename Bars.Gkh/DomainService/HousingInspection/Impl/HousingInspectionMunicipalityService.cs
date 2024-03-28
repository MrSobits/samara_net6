namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.HousingInspection;

    using Castle.Windsor;

    public class HousingInspectionMunicipalityService : IHousingInspectionMunicipalityService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<HousingInspectionMunicipality> Domain { get; set; }

        public IDomainService<HousingInspection> HousingInspDomain { get; set; }

        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        public IDataResult AddMunicipalities(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var housingInspectionId = baseParams.Params.GetAs<long>("housingInspectionId");
                    var muIds = baseParams.Params.GetAs<long[]>("muIds");

                    var existRecs = this.Domain.GetAll()
                        .Where(x => x.HousingInspection.Id == housingInspectionId)
                        .Select(x => x.Municipality.Id)
                        .Distinct()
                        .AsEnumerable()
                        .ToDictionary(x => x);

                    foreach (var id in muIds)
                    {
                        if (existRecs.ContainsKey(id))
                            continue;

                        var newObj = new HousingInspectionMunicipality
                        {
                            HousingInspection = new HousingInspection {Id = housingInspectionId},
                            Municipality = new Municipality {Id = id}
                        };

                        this.Domain.Save(newObj);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = e.Message};
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