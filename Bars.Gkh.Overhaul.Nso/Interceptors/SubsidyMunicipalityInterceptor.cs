namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System.Linq;
    using B4;

    using Bars.Gkh.Overhaul.Nso.ConfigSections;

    using Entities;
    using Gkh.Utils;

    public class SubsidyMunicipalityInterceptor : EmptyDomainInterceptor<SubsidyMunicipality>
    {
        public IDomainService<ProgramVersion> VersionDomain { get; set; }

        public IDomainService<SubsidyRecordVersionData> SubsidyRecordVersionDomain { get; set; }

        public override IDataResult AfterCreateAction(IDomainService<SubsidyMunicipality> service, SubsidyMunicipality entity)
        {
            var domainRecords = Container.Resolve<IDomainService<SubsidyMunicipalityRecord>>();

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var periodEnd = config.ProgrammPeriodEnd;

            var versions = VersionDomain.GetAll()
                .Select(x => x.Id)
                .ToList();

            while (periodStart <= periodEnd)
            {
                var newRec = new SubsidyMunicipalityRecord
                {
                    SubsidyYear = periodStart,
                    SubsidyMunicipality = entity
                };

                domainRecords.Save(newRec);

                foreach (var version in versions)
                {
                    SubsidyRecordVersionDomain.Save(new SubsidyRecordVersionData
                    {
                        Version = new ProgramVersion { Id = version },
                        SubsidyRecordUnversioned = newRec
                    });
                }

                periodStart++;
            }

            Container.Release(domainRecords);

            return Success();
        }
    }
}