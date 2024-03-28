namespace Bars.Gkh.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Volgograd.Properties;

    using Castle.Windsor;

    public class RepairPlanning : BasePrintForm
    {
        #region Свойства
        public IWindsorContainer Container { get; set; }

        private long[] municipalityIds;

        public RepairPlanning()
            : base(new ReportTemplateBinary(Resources.RepairPlanning))
        {
        }

        public override string Name
        {
            get
            {
                return "Планирование ремонта";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Планирование ремонта";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Капитальный ремонт";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.RepairPlanning";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GKH.RepairPlanning";
            }
        }
        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var constElemColumnIndexes = new Dictionary<string, int>();

            constElemColumnIndexes["Подвал"] = 17;
            constElemColumnIndexes["Фасад"] = 20;
            constElemColumnIndexes["Кровля"] = 23;
            constElemColumnIndexes["Лифты"] = 26;
            constElemColumnIndexes["Фундамент"] = 29;
            constElemColumnIndexes["Система отопления"] = 42;
            constElemColumnIndexes["Система горячего водоснабжения"] = 45;
            constElemColumnIndexes["Система холодного водоснабжения"] = 48;
            constElemColumnIndexes["Система водоотведения (канализации)"] = 51;
            constElemColumnIndexes["Система электроснабжения"] = 54;
            constElemColumnIndexes["Вентиляция"] = 57;
            constElemColumnIndexes["Система газоснабжения"] = 60;

            var meteringDeviceColumnIndexes = new Dictionary<MeteringDeviceGroup, int>();

            meteringDeviceColumnIndexes[MeteringDeviceGroup.HotWater] = 32;
            meteringDeviceColumnIndexes[MeteringDeviceGroup.ColdWater] = 34;
            meteringDeviceColumnIndexes[MeteringDeviceGroup.Heating] = 36;
            meteringDeviceColumnIndexes[MeteringDeviceGroup.Electricity] = 38;
            meteringDeviceColumnIndexes[MeteringDeviceGroup.Gas] = 40;

            var groupNameList = constElemColumnIndexes.Keys;
            
            var realtyObjectsQuery = this.Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Municipality.Id));

            var nextYear = DateTime.Now.Year + 1;

            var realtyObjectIdsQuery = realtyObjectsQuery.Select(x => x.Id);

            var realtyObjectsInfo = realtyObjectsQuery
                .Select(x => new
                {
                    muName = x.Municipality.Name,
                    roId = x.Id,
                    x.Address,
                    x.DateCommissioning,
                    wallMaterialName = x.WallMaterial.Name,
                    x.MaximumFloors,
                    x.NumberEntrances,
                    x.AreaMkd,
                    x.AreaLiving,
                    x.AreaLivingNotLivingMkd,
                    x.NumberApartments,
                    x.NumberLiving,
                    x.PhysicalWear
                })
                .ToArray();

            var constructiveElements = this.Container.Resolve<IDomainService<RealityObjectConstructiveElement>>().GetAll()
                .Where(x => realtyObjectIdsQuery.Contains(x.RealityObject.Id))
                .Where(x => groupNameList.Contains(x.ConstructiveElement.Group.Name))
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.Volume,
                    x.LastYearOverhaul,
                    x.ConstructiveElement.Lifetime,
                    GroupName = x.ConstructiveElement.Group.Name,
                    x.ConstructiveElement.RepairCost
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.GroupName)
                    .ToDictionary(
                        y => y.Key,
                        y =>
                        {
                            var constuctiveElements = y.Select(z => new
                            {
                                year = z.Lifetime + z.LastYearOverhaul < nextYear ? nextYear : z.Lifetime + z.LastYearOverhaul,
                                z.Volume,
                                z.RepairCost
                            })
                            .ToList();

                            var minYear = constuctiveElements.Min(z => z.year);
                            var repairCost = 0M;

                            foreach (var constuctiveElement in constuctiveElements)
                            {
                                var currentVolume = constuctiveElement.Volume.HasValue ? constuctiveElement.Volume.ToDecimal() : 0M;
                                var currentRepairCost = constuctiveElement.RepairCost.HasValue ? constuctiveElement.RepairCost.ToDecimal() : 0M;
                                repairCost += currentVolume * currentRepairCost;
                            }

                            return new
                            {
                                year = minYear,
                                Volume = constuctiveElements.Sum(z => z.Volume),
                                RepairCost = repairCost,
                            };
                        }));

            var meteringDeviceInfo = this.Container.Resolve<IDomainService<RealityObjectMeteringDevice>>().GetAll()
                .Where(x => realtyObjectIdsQuery.Contains(x.RealityObject.Id))
                .Where(x => x.DateInstallation != null)
                .Where(x => x.MeteringDevice.Group != null)
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.MeteringDevice.Group,
                    MeteringDeviceRepairPlanDate = x.DateInstallation.Value + (x.MeteringDevice.LifeTime ?? 0),
                    x.MeteringDevice.ReplacementCost
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                x => x.Key,
                x => x.GroupBy(y => y.Group)
                      .ToDictionary(
                      y => y.Key,
                      y =>
                      {
                          var minDate = y.Min(z => z.MeteringDeviceRepairPlanDate);
                          var data = y.Where(z => z.MeteringDeviceRepairPlanDate == minDate).ToList();
                          return new { date = minDate < nextYear ? nextYear : minDate, sum = data.Sum(z => z.ReplacementCost) };
                      }));

            var sectionMain = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMain");
            var section = sectionMain.ДобавитьСекцию("sectionMunicipality");

            int i = 0;

            Func<int?, string> intToStringValue = x => x.HasValue ? x.Value.ToString() : string.Empty;
            Func<decimal?, string> decimalToStringValue = x => x.HasValue ? x.Value.ToString() : string.Empty;
           
            foreach (var municipalityData in realtyObjectsInfo.GroupBy(x => x.muName).OrderBy(x => x.Key))
            {
                sectionMain.ДобавитьСтроку();
                sectionMain["municipalityName"] = municipalityData.Key;

                var municipalitySumDict = new Dictionary<int, decimal>();

                var totalCost = 0m;
                var isComplexRepair = true;

                Action<int, decimal?> writeToReportAndSumNullable = (columnIndex, value) =>
                {
                    if (value.HasValue)
                    {
                        section["col" + columnIndex] = value.Value;

                        if (municipalitySumDict.ContainsKey(columnIndex))
                        {
                            municipalitySumDict[columnIndex] += value.Value;
                        }
                        else
                        {
                            municipalitySumDict[columnIndex] = value.Value;
                        }
                    }
                };

                Action<int, decimal?> writeToReportAndSumCost = (columnIndex, value) =>
                {
                    if (value.HasValue)
                    {
                        totalCost += value.Value;
                    }

                    isComplexRepair = isComplexRepair && value.HasValue && value.Value > 0;

                    writeToReportAndSumNullable(columnIndex, value);
                };

                foreach (var realtyObject in municipalityData.OrderBy(x => x.Address))
                {
                    var dateList = new List<int>();
                    totalCost = 0m;
                    isComplexRepair = true;

                    section.ДобавитьСтроку();

                    section["col1"] = ++i;
                    section["col2"] = municipalityData.Key;
                    section["col3"] = realtyObject.Address;
                    section["col4"] = realtyObject.DateCommissioning.HasValue ? realtyObject.DateCommissioning.Value.ToString("dd.MM.yyyy") : string.Empty;
                    section["col5"] = realtyObject.wallMaterialName;
                    section["col6"] = intToStringValue(realtyObject.MaximumFloors);
                    section["col7"] = intToStringValue(realtyObject.NumberEntrances);
                    section["col8"] = decimalToStringValue(realtyObject.AreaMkd);
                    section["col9"] = decimalToStringValue(realtyObject.AreaLiving);

                    if (realtyObject.AreaLivingNotLivingMkd.HasValue && realtyObject.AreaLiving.HasValue)
                    {
                        section["col10"] = decimalToStringValue(realtyObject.AreaLivingNotLivingMkd.Value - realtyObject.AreaLiving.Value);
                    }

                    section["col11"] = intToStringValue(realtyObject.NumberApartments);
                    section["col12"] = intToStringValue(realtyObject.NumberLiving);

                    section["col16"] = decimalToStringValue(realtyObject.PhysicalWear);

                    if (constructiveElements.ContainsKey(realtyObject.roId))
                    {
                        var roConstructiveElements = constructiveElements[realtyObject.roId];

                        constElemColumnIndexes.ForEach(x =>
                        {
                            if (roConstructiveElements.ContainsKey(x.Key))
                            {
                                var data = roConstructiveElements[x.Key];

                                writeToReportAndSumNullable(x.Value, data.Volume);
                                section["col" + (x.Value + 1)] = data.year;
                                writeToReportAndSumCost(x.Value + 2, data.RepairCost);

                                if (data.year >= nextYear)
                                {
                                    dateList.Add(decimal.ToInt32(data.year));
                                }
                            }
                            else
                            {
                                isComplexRepair = false;
                                section["col" + x.Value] = "-";
                                section["col" + (x.Value + 1)] = "-";
                                section["col" + (x.Value + 2)]  = "-";      
                            }
                        });
                    }
                    else
                    {
                        isComplexRepair = false;
                        constElemColumnIndexes.ForEach(x => {
                            section["col" + x.Value ] = "-";
                            section["col" + (x.Value + 1)] = "-";
                            section["col" + (x.Value + 2)] = "-";
                        });
                    }

                    if (meteringDeviceInfo.ContainsKey(realtyObject.roId))
                    {
                        var roMeteringDeviceInfo = meteringDeviceInfo[realtyObject.roId];

                        meteringDeviceColumnIndexes.ForEach(x =>
                        {
                            if (roMeteringDeviceInfo.ContainsKey(x.Key))
                            {
                                var data = roMeteringDeviceInfo[x.Key];

                                section["col" + x.Value] = data.date;
                                dateList.Add(data.date);
                                writeToReportAndSumCost(x.Value + 1, data.sum);
                            }
                            else
                            {
                                isComplexRepair = false;
                                section["col" + x.Value] = "-";
                                section["col" + (x.Value + 1)] = "-";
                            }
                        });
                    }
                    else
                    {
                        isComplexRepair = false;
                        meteringDeviceColumnIndexes.ForEach(x =>
                        {
                            section["col" + x.Value] = "-";
                            section["col" + (x.Value + 1)] = "-";
                        });
                    }

                    section["col13"] = isComplexRepair ? "комплексный" : "частичный";

                    if (dateList.Any())
                    {
                        section["col14"] = dateList.Min();
                    }

                    section["col15"] = totalCost;

                    municipalitySumDict[15] = (municipalitySumDict.ContainsKey(15) ? municipalitySumDict[15] : 0) + totalCost;
                }

                municipalitySumDict.ForEach(x => sectionMain["col" + x.Key + "Total"] = x.Value);
            }
        }
    }
}