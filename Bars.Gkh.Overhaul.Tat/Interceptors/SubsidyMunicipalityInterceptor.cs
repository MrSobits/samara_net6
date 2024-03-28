namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class SubsidyMunicipalityInterceptor : EmptyDomainInterceptor<SubsidyMunicipality>
    {
        public IDomainService<ProgramVersion> VersionDomain { get; set; }

        public IDomainService<SubsidyRecordVersionData> SubsidyRecordVersionDomain { get; set; }

        public override IDataResult AfterCreateAction(IDomainService<SubsidyMunicipality> service, SubsidyMunicipality entity)
        {
            //ToDo     тут неправильно на создании актуализировать Субсидирвоание поскольку Субсидирваоние уже могло существовать 
            //ToDo     а параметры ДПКР изменятся или изменится основная версия и эт онад оконтролирвоат ьпрост ов методе GetSubsidy
            /*
            var domainRecords = Container.Resolve<IDomainService<SubsidyMunicipalityRecord>>();

            var periodStart = OverhaullParamProvider.GetOverhaulParam<int>("ProgrammPeriodStart", Container);
            var periodEnd = OverhaullParamProvider.GetOverhaulParam<int>("ProgrammPeriodEnd", Container);

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
            */
            return Success();
        }
    }
}