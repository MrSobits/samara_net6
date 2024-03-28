using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.Gkh.Report
{
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Properties;

    using Castle.Windsor;

    class ReferenceWallMaterial : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long[] municipalityIds;

        private DateTime reportDate;

        private Dictionary<int, int> floorGroup = new Dictionary<int, int>()
                                                      {
                                                          { 0, -1 }, { 1, 0 }, { 2, 0 }, { 3, 1 }, { 4, 1 },
                                                          { 5, 1 }, { 6, 2 }, { 7, 2 }, { 8, 2 }, { 9, 2 }
                                                      };
        private Dictionary<int, string> monthDict = new Dictionary<int, string>()
                                                        {
                                                            { 1, "Января" }, { 2, "Февраля" }, { 3, "Марта" }, { 4, "Апреля" }, { 5, "Мая" }, { 6, "Июня" },
                                                            { 7, "Июля" }, { 8, "Августа" }, { 9, "Сентября" }, { 10, "Октября" }, { 11, "Ноября" }, { 12, "Декабря" }
                                                        };

        public ReferenceWallMaterial()
            : base(new ReportTemplateBinary(Resources.ReferenceWallMaterial))
        {
        }

        public override string Name
        {
            get
            {
                return "Справка по материалу стен (Приложение 6)";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Справка по материалу стен (Приложение 6)";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Аварийность";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.ReferenceWallMaterial";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GKH.ReferenceWallMaterial";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            reportDate = baseParams.Params["reportDate"].ToDateTime();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceEmergencyObject = Container.Resolve<IDomainService<EmergencyObject>>();
            var serviceEmerObjResettlementProgram = Container.Resolve<IDomainService<EmerObjResettlementProgram>>();

            Func<string, int> getGroup = wall =>
            {
                var wallMaterial = wall;

                if (wallMaterial == "Кирпичные")
                {
                    return 1;
                }
                else if (wallMaterial == "Панельные")
                {
                    return 2;
                }
                else if (wallMaterial == "Блочные")
                {
                    return 3;
                }
                else if (wallMaterial == "Смешанные")
                {
                    return 4;
                }
                else if (wallMaterial == "Деревянные")
                {
                    return 5;
                }
                else
                {
                    return 6;
                }

            };

            #region Запросы
            var municipalityList = Container.Resolve<IDomainService<Municipality>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Id))
                .Select(x => x.Name)
                .ToList();

            var infoEmergRealObjByCount = serviceEmergencyObject.GetAll()
                          .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                          .Select(x => new
                          {
                              WallMaterial = x.RealityObject.WallMaterial.Name,
                              countRealObjId = x.RealityObject.Id,
                              areaMkd = x.RealityObject.AreaMkd,
                              areaMkdLiv = x.RealityObject.AreaLiving,
                              countPerson = x.RealityObject.NumberLiving,
                              countResettlementFlatAmount = x.ResettlementFlatAmount,
                              resettlementFlatArea = x.ResettlementFlatArea
                          })
                          .AsEnumerable()
                          .GroupBy(x => getGroup(x.WallMaterial))
                          .ToDictionary(x => x.Key, y => new
                          {
                              countRealObj = y.Count(),
                              sumArea = y.Sum(z => z.areaMkd),
                              sumAreaLiv = y.Sum(z => z.areaMkdLiv),
                              numPerson = y.Sum(z => z.countPerson),
                              countResElemFlatAm = y.Sum(z => z.countResettlementFlatAmount),
                              resElemFlatArea = y.Sum(z => z.resettlementFlatArea)
                          });

            var infoEmergRealObjByFinPrCount = serviceEmerObjResettlementProgram.GetAll()
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.EmergencyObject.RealityObject.Municipality.Id))
                         .Select(x => new
                            {
                                resettlementProgramSource = x.ResettlementProgramSource.Code,
                                wallMaterial = x.EmergencyObject.RealityObject.WallMaterial.Name,
                                residents = x.CountResidents,
                                area = x.Area,
                                cost = x.Cost
                            })
                            .AsEnumerable()
                            .GroupBy(x => x.resettlementProgramSource)
                            .ToDictionary(x => x.Key, x => x
                                .GroupBy(y => getGroup(y.wallMaterial))
                                .ToDictionary(y => y.Key, y => new
                                                                   {
                                                                       countResidents = y.Sum(z => z.residents),
                                                                       sumArea = y.Sum(z => z.area),
                                                                       sumCost = y.Sum(z => z.cost)
                                                                   })
                                );

            #endregion

            #region Вывод данных
            reportParams.SimpleReportParams["municipalities"] = municipalityList.Aggregate((a, b) => a + ", " + b);
            reportParams.SimpleReportParams["Day"] = reportDate.Date.Day;
            reportParams.SimpleReportParams["Month"] = monthDict.ContainsKey(reportDate.Date.Month) ? monthDict[reportDate.Date.Month] : string.Empty;
            reportParams.SimpleReportParams["Year"] = reportDate.Date.Year;

            var roByFlDict = infoEmergRealObjByCount;
            foreach (var item in roByFlDict.Where(item => item.Key != -1))
            {
                reportParams.SimpleReportParams["totCountHouse" + item.Key] = item.Value.countRealObj;
                reportParams.SimpleReportParams["totArea" + item.Key] = item.Value.sumArea;
                reportParams.SimpleReportParams["totAreaLiv" + item.Key] = item.Value.sumAreaLiv;
                reportParams.SimpleReportParams["totPerson" + item.Key] = item.Value.numPerson;
                reportParams.SimpleReportParams["totCountRes" + item.Key] = item.Value.countResElemFlatAm;
                reportParams.SimpleReportParams["totAreaRes" + item.Key] = item.Value.resElemFlatArea;
            }

            var roByFinPr = infoEmergRealObjByFinPrCount;
            foreach (var finPr in roByFinPr)
            {
                foreach (var houses in finPr.Value.Where(floors => floors.Key != -1))
                {
                    reportParams.SimpleReportParams["countPerson" + finPr.Key + houses.Key] = houses.Value.countResidents;
                    reportParams.SimpleReportParams["sumAreaRes" + finPr.Key + houses.Key] = houses.Value.sumArea;
                    reportParams.SimpleReportParams["sumCost" + finPr.Key + houses.Key] = houses.Value.sumCost;
                }
            }
            #endregion
        }
    }
}
