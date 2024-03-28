namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.ConfigSections.Overhaul;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    using Overhaul.Domain;
    using Entities;

    using Castle.Windsor;
    using Gkh.Utils;

    public class DefaultPlanCollectionInfoService : IDefaultPlanCollectionInfoService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<DefaultPlanCollectionInfo> DefaultPlanCollectionInfoDomain { get; set; }

        public IDomainService<SubsidyRecordVersion> SubsidyRecordVersionDomain { get; set; }

        public IDomainService<ProgramVersion> VersionDomain { get; set; }

        public IDomainService<SubsidyRecord> SubsidyRecordDomain { get; set; }

        public IDataResult UpdatePeriod(BaseParams baseParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var shortTermProgPeriod = config.ShortTermProgPeriod;
            var periodEnd = periodStart + shortTermProgPeriod;

            if (periodStart == 0 || shortTermProgPeriod == 0)
            {
                return new BaseDataResult(false, "Не задан период");
            }

            var data = DefaultPlanCollectionInfoDomain.GetAll().Where(x => x.Year >= periodStart && periodEnd > x.Year).ToList();

            var listToSave = new List<DefaultPlanCollectionInfo>();
            var currentYear = periodStart;
            while (currentYear < periodEnd)
            {
                var rec = data.FirstOrDefault(x => x.Year == currentYear);

                if (rec == null)
                {
                    listToSave.Add(new DefaultPlanCollectionInfo { Year = currentYear });
                }

                currentYear++;
            }

            using (var tx = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    listToSave.ForEach(x => DefaultPlanCollectionInfoDomain.Save(x));
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }

        public IDataResult CopyCollectionInfo(BaseParams baseParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var shortTermProgPeriod = config.ShortTermProgPeriod;
            var periodEnd = periodStart + shortTermProgPeriod;

            if (periodStart == 0 || shortTermProgPeriod == 0)
            {
                return new BaseDataResult(false, "Не задан период");
            }

            var data = DefaultPlanCollectionInfoDomain.GetAll()
                .Where(x => x.Year >= periodStart && periodEnd > x.Year)
                .GroupBy(x => x.Year)
                .ToDictionary(x => x.Key, y => y.First());

            var destinationIds = baseParams.Params.GetAs<long[]>("destinationIds");

            var subsidies = SubsidyRecordVersionDomain.GetAll()
                .Where(x => destinationIds.Contains(x.Version.Municipality.Id) && x.Version.IsMain)
                .Where(x => x.SubsidyYear >= periodStart)
                .Where(x => x.SubsidyYear < periodEnd)
                .ToList();

              // Получаем существующие записи субсидирования по МО
            var subsidyRecordsDicts =
                SubsidyRecordDomain.GetAll()
                    .GroupBy(x => x.Municiaplity.Id)
                    .ToDictionary(x => x.Key, x => x.GroupBy(y => y.SubsidyYear).ToDictionary(y => y.Key, y => y.First()));

            var municipalities = Container.ResolveRepository<Municipality>().GetAll().ToDictionary(x => x.Id, x => x);

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var destinationId in destinationIds)
                    {
                        var municipality = municipalities.ContainsKey(destinationId) ? municipalities[destinationId] : null;
                        if (municipality == null)
                        {
                            continue;
                        }

                        var version = VersionDomain.GetAll().FirstOrDefault(x => x.IsMain && x.Municipality.Id == destinationId);
                        if (version == null)
                        {
                            version = new ProgramVersion
                            {
                                IsMain = true,
                                Municipality = municipality,
                                VersionDate = DateTime.Now,
                                ActualizeDate = DateTime.Now,
                                Name = municipality.Name + " " + DateTime.Now.ToString("dd.MM.yyyy")
                            };
                            VersionDomain.Save(version);
                        }
                        var destinationSubsidies = subsidies.Where(x => x.Version.Municipality.Id == destinationId).ToList();
                        var subsidyRecords = subsidyRecordsDicts.ContainsKey(destinationId) ? subsidyRecordsDicts[destinationId] : new Dictionary<int, SubsidyRecord>();
                        for (var year = periodStart; year < periodEnd; year++)
                        {
                            if (!data.ContainsKey(year))
                            {
                                continue;
                            }

                            SubsidyRecord subsidyRecord;
                            if (subsidyRecords.ContainsKey(year))
                            {
                                subsidyRecord = subsidyRecords[year];
                            }
                            else
                            {
                                subsidyRecord = new SubsidyRecord
                                {
                                    SubsidyYear = year,
                                    PlanOwnerPercent = 0,
                                    NotReduceSizePercent = 0,
                                    Municiaplity = municipality,
                                    DateCalcOwnerCollection = DateTime.Now
                                };
                                SubsidyRecordDomain.Save(subsidyRecord);
                            }

                            var subsidy = destinationSubsidies.FirstOrDefault(x => x.SubsidyYear == year);
                            if (subsidy == null)
                            {
                                subsidy = new SubsidyRecordVersion
                                {
                                    Version = version,
                                    SubsidyRecord = subsidyRecord,
                                    SubsidyYear = year
                                };

                                SubsidyRecordVersionDomain.Save(subsidy);
                            }

                            subsidy.PlanOwnerPercent = data[year].PlanOwnerPercent;
                            subsidy.NotReduceSizePercent = data[year].NotReduceSizePercent;
                            SubsidyRecordVersionDomain.Update(subsidy);
                        }
                    }

                    tr.Commit();
                }
                catch (ValidationException e)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            return new BaseDataResult();
        }
    }
}