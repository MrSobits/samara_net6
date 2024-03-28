namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;

    public class DpkrDataAnalysisReport : BasePrintForm
    {
        private List<long> municipalityIds;
        private int version;

        #region DomainServices

        public IWindsorContainer Container { get; set; }
        public IDomainService<PublishedProgramRecord> PublishedProgramRecordDomain { get; set; }
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }
        public IDomainService<VersionRecordStage2> VersionRecordStage2Domain { get; set; }
        public IDomainService<CommonEstateObject> CommonEstateObjectDomain { get; set; }
        public IDomainService<VersionRecordStage1> VersionRecordStage1Domain { get; set; }
        public IDomainService<ProgramVersion> ProgramVersionDomain { get; set; }
        public IRepository<Municipality> MunicipalityRepo { get; set; }

        #endregion

        #region Properties && .ctor

        public DpkrDataAnalysisReport()
            : base(new ReportTemplateBinary(Properties.Resources.DpkrDataAnalysisReport))
        {
        }

        public override string Name { get { return "Анализ данных по ДПКР (по основной и опубликованной версии программы)"; } }

        public override string Desciption { get { return "Анализ данных по ДПКР (по основной и опубликованной версии программы)"; } }

        public override string GroupName { get { return "Региональная программа"; } }

        public override string ParamsController { get { return "B4.controller.report.DpkrDataAnalysisReport"; } }

        public override string RequiredPermission { get { return "Reports.GkhOverhaul.DpkrDataAnalysisReport"; } }

        public override string ReportGenerator { get; set; }

        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            var moIdsList = baseParams.Params.GetAs("muIds", string.Empty);
            var moIds = !string.IsNullOrEmpty(moIdsList)
                                  ? moIdsList.Split(',').Select(id => id.ToLong()).ToList()
                                  : new List<long>();

            var appParams = this.Container.Resolve<IGkhParams>().GetParams();
            var hasMoParam = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>());
            var moLevel = hasMoParam
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            municipalityIds = Container.Resolve<IRepository<Municipality>>().GetAll()
                .WhereIf(moIds.Any(), x => moIds.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Level
                })
                .AsEnumerable()
                .Where(x => x.Level.ToMoLevel(Container) == moLevel)
                .Select(x => x.Id)
                .ToList();

            version = baseParams.Params.GetAs<int>("programVersion");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            // получаю ООИ, у которых есть отметка в поле "Включен в программу субъекта"
            var ooiList = CommonEstateObjectDomain.GetAll()
                .Where(x => x.IncludedInSubjectProgramm)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .ToList();

            if (ooiList.Any())
            {
                var ooiSection = reportParams.ComplexReportParams.ДобавитьСекцию("ooiSection");

                foreach (var ooi in ooiList)
                {
                    ooiSection.ДобавитьСтроку();
                    ooiSection["OoiName"] = ooi.Name;
                    ooiSection["VolumeOoi"] = string.Format("$Volume{0}$", ooi.Id);
                    ooiSection["YearOoi"] = string.Format("$Year{0}$", ooi.Id);
                    ooiSection["CostOoi"] = string.Format("$Cost{0}$", ooi.Id);
                }
            }

            // ЕСЛИ СБОРКА ПО ОСНОВНОЙ ВЕРСИИ
            if (version == 0)
            {
                var versionIds =
                    ProgramVersionDomain.GetAll()
                        .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.Municipality.Id))
                        .Where(x => x.IsMain)
                        .Select(x => x.Id)
                        .ToArray();

                var records =
                    VersionRecordDomain.GetAll()
                        .Where(x => versionIds.Contains(x.ProgramVersion.Id))
                        .Select(x => new
                        {
                            RoId = x.RealityObject.Id,
                            Municipality = x.RealityObject.Municipality.Name,
                            x.RealityObject.FiasAddress.PlaceName,
                            x.RealityObject.FiasAddress.StreetName,
                            x.RealityObject.FiasAddress.House,
                            x.RealityObject.FiasAddress.Housing,
                            x.RealityObject.FiasAddress.Letter,
                            x.RealityObject.AreaMkd
                        })
                        .ToList()
                        .Distinct()
                        .OrderBy(x => x.Municipality)
                        .ThenBy(x => x.PlaceName)
                        .ThenBy(x => x.StreetName)
                        .ThenBy(x => x.House)
                        .ThenBy(x => x.Housing)
                        .ThenBy(x => x.Letter);

                var sumAndYearDict =
                    VersionRecordStage2Domain.GetAll()
                        .Where(x => versionIds.Contains(x.Stage3Version.ProgramVersion.Id))
                        .Select(x => new
                        {
                            RoId = x.Stage3Version.RealityObject.Id,
                            CeoId = x.CommonEstateObject.Id,
                            x.Stage3Version.Year,
                            x.Sum
                        })
                        .AsEnumerable()
                        .OrderBy(x => x.Year)
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, y => y.GroupBy(x => x.CeoId)
                            .ToDictionary(x => x.Key, z => new
                            {
                                Years = z.Select(x => x.Year.ToString("#.##")).AggregateWithSeparator(", "),
                                Sums = z.Select(x => x.Sum.ToString("#.##")).AggregateWithSeparator(", ")
                            }));

                var totalCostDict =
                    VersionRecordStage2Domain.GetAll()
                        .Where(x => versionIds.Contains(x.Stage3Version.ProgramVersion.Id))
                        .Select(x => new
                        {
                            RoId = x.Stage3Version.RealityObject.Id, 
                            x.Sum
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, y => new
                        {
                            TotalCost = y.SafeSum(x => x.Sum)
                        });

                var structElemVolumeDict =
                    VersionRecordStage1Domain.GetAll()
                        .Where(x => versionIds.Contains(x.Stage2Version.Stage3Version.ProgramVersion.Id))
                        .Select(x => new
                        {
                            RoId = x.RealityObject.Id,
                            CeoId = x.Stage2Version.CommonEstateObject.Id,
                            x.StructuralElement.Volume
                        })
                        .AsEnumerable()
                        .Distinct()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, y => y.GroupBy(x => x.CeoId)
                            .ToDictionary(x => x.Key, z => new
                            {
                                VolSum = z.SafeSum(x => x.Volume)
                            }));

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
                var num = 0;

                foreach (var record in records)
                {
                    section.ДобавитьСтроку();
                    section["Num"] = ++num;
                    section["Municipality"] = record.Municipality;
                    section["PlaceName"] = record.PlaceName;
                    section["Address"] = string.Format("{0}, д. {1}{2}{3}", 
                        record.StreetName,
                        record.House,
                        record.Letter,
                        !record.Housing.IsEmpty()
                            ? ", корп. " + record.Housing
                            : string.Empty);
                    section["AreaMkd"] = record.AreaMkd.HasValue 
                        ? record.AreaMkd.Value.ToString("#.##")
                        : string.Empty;

                    if (structElemVolumeDict.ContainsKey(record.RoId))
                    {
                        foreach (var ceoId in structElemVolumeDict[record.RoId].Keys)
                        {
                            section[string.Format("Volume{0}", ceoId)] = sumAndYearDict.ContainsKey(record.RoId) && sumAndYearDict[record.RoId].ContainsKey(ceoId)
                                ? structElemVolumeDict[record.RoId].ContainsKey(ceoId)
                                    ? structElemVolumeDict[record.RoId][ceoId].VolSum.ToString("#.##")
                                    : 0m.ToString("#.##")
                                : string.Empty;
                        }
                    }

                    if (sumAndYearDict.ContainsKey(record.RoId))
                    {
                        foreach (var ceoId in sumAndYearDict[record.RoId].Keys)
                        {
                            section[string.Format("Year{0}", ceoId)] = sumAndYearDict[record.RoId].ContainsKey(ceoId)
                                ? sumAndYearDict[record.RoId][ceoId].Years.ToStr()
                                : string.Empty;
                            section[string.Format("Cost{0}", ceoId)] = sumAndYearDict[record.RoId].ContainsKey(ceoId)
                                ? sumAndYearDict[record.RoId][ceoId].Sums.ToStr()
                                : string.Empty;
                        }
                    }

                    section["TotalCost"] = totalCostDict.ContainsKey(record.RoId)
                        ? totalCostDict[record.RoId].TotalCost.ToString("#.##")
                        : string.Empty;
                }
            }

            // ЕСЛИ СБОРКА ПО ОПУБЛИКОВАННОЙ ПРОГРАММЕ
            else
            {
                var versionIds =
                    ProgramVersionDomain.GetAll()
                        .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.Municipality.Id))
                        .Where(x => x.IsMain)
                        .Select(x => x.Id)
                        .ToArray();

                var records =
                    PublishedProgramRecordDomain.GetAll()
                        .Where(x => versionIds.Contains(x.PublishedProgram.ProgramVersion.Id))
                        .Select(x => new
                        {
                            RoId = x.RealityObject.Id,
                            Municipality = x.RealityObject.Municipality.Name,
                            x.RealityObject.FiasAddress.PlaceName,
                            x.RealityObject.FiasAddress.StreetName,
                            x.RealityObject.FiasAddress.House,
                            x.RealityObject.FiasAddress.Housing,
                            x.RealityObject.FiasAddress.Letter,
                            x.RealityObject.AreaMkd
                        })
                        .ToList()
                        .Distinct()
                        .OrderBy(x => x.Municipality)
                        .ThenBy(x => x.PlaceName)
                        .ThenBy(x => x.StreetName)
                        .ThenBy(x => x.House)
                        .ThenBy(x => x.Housing)
                        .ThenBy(x => x.Letter);

                var structElemVolumeDict =
                    VersionRecordStage1Domain.GetAll()
                        .Where(x => versionIds.Contains(x.Stage2Version.Stage3Version.ProgramVersion.Id))
                        .Select(x => new
                        {
                            RoId = x.RealityObject.Id,
                            CeoId = x.Stage2Version.CommonEstateObject.Id,
                            x.StructuralElement.Volume
                        })
                        .AsEnumerable()
                        .Distinct()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, y => y.GroupBy(x => x.CeoId)
                            .ToDictionary(x => x.Key, z => new
                            {
                                VolSum = z.SafeSum(x => x.Volume)
                            }));

                var sumAndYearDict =
                    PublishedProgramRecordDomain.GetAll()
                    .Where(x => x.Stage2 != null)
                        .Where(x => versionIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Id))
                        .Select(x => new
                        {
                            RoId = x.Stage2.Stage3Version.RealityObject.Id,
                            CeoId = x.Stage2.CommonEstateObject.Id,
                            x.PublishedYear,
                            x.Sum
                        })
                        .AsEnumerable()
                        .OrderBy(x => x.PublishedYear)
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, y => y.GroupBy(x => x.CeoId)
                            .ToDictionary(x => x.Key, z => new
                            {
                                Years = z.Select(x => x.PublishedYear.ToString("#.##")).AggregateWithSeparator(", "),
                                Sums = z.Select(x => x.Sum.ToString("#.##")).AggregateWithSeparator(", ")
                            }));

                var totalCostDict =
                    PublishedProgramRecordDomain.GetAll()
                        .Where(x => x.Stage2 != null)
                        .Where(x => versionIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Id))
                        .Select(
                            x => new
                            {
                                RoId = x.Stage2.Stage3Version.RealityObject.Id,
                                x.Sum
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(
                            x => x.Key,
                            y => new
                            {
                                TotalCost = y.SafeSum(x => x.Sum)
                            });

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
                var num = 0;

                foreach (var record in records)
                {
                    section.ДобавитьСтроку();
                    section["Num"] = ++num;
                    section["Municipality"] = record.Municipality;
                    section["PlaceName"] = record.PlaceName;
                    section["Address"] = string.Format("{0}, д. {1}{2}{3}",
                        record.StreetName,
                        record.House,
                        record.Letter,
                        !record.Housing.IsEmpty()
                            ? ", корп. " + record.Housing
                            : string.Empty);
                    section["AreaMkd"] = record.AreaMkd.HasValue
                        ? record.AreaMkd.Value.ToString("#.##")
                        : string.Empty;

                    if (structElemVolumeDict.ContainsKey(record.RoId))
                    {
                        foreach (var ceoId in structElemVolumeDict[record.RoId].Keys)
                        {
                            section[string.Format("Volume{0}", ceoId)] = sumAndYearDict.ContainsKey(record.RoId) && sumAndYearDict[record.RoId].ContainsKey(ceoId)
                                ? structElemVolumeDict[record.RoId].ContainsKey(ceoId)
                                    ? structElemVolumeDict[record.RoId][ceoId].VolSum.ToString("#.##")
                                    : 0m.ToString("#.##")
                                : string.Empty;
                        }
                    }

                    if (sumAndYearDict.ContainsKey(record.RoId))
                    {
                        foreach (var ceoId in sumAndYearDict[record.RoId].Keys)
                        {
                            section[string.Format("Year{0}", ceoId)] = sumAndYearDict[record.RoId].ContainsKey(ceoId)
                                ? sumAndYearDict[record.RoId][ceoId].Years.ToStr()
                                : string.Empty;
                            section[string.Format("Cost{0}", ceoId)] = sumAndYearDict[record.RoId].ContainsKey(ceoId)
                                ? sumAndYearDict[record.RoId][ceoId].Sums.ToStr()
                                : string.Empty;
                        }
                    }

                    section["TotalCost"] = totalCostDict.ContainsKey(record.RoId) 
                        ? totalCostDict[record.RoId].TotalCost.ToString("#.##")
                        : string.Empty;
                }
            }
        }
    }
}