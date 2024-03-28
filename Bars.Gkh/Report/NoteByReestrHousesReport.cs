namespace Bars.Gkh.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class NoteByReestrHousesReport : BasePrintForm
    {
        public NoteByReestrHousesReport(): base(new ReportTemplateBinary(Properties.Resources.NoteByReestrHouses))
        {
        }
        public IWindsorContainer Container { get; set; }

        #region Входящие параметры
        private long[] municipalityIds;
        private DateTime dateReport;
        #endregion

        public override string Name
        {
            get
            {
                return "Выписка из реестра домов (Приложение 2)";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Выписка из реестра домов (Приложение 2)";
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
                return "B4.controller.report.NoteByReestrHouses";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GKH.NoteByReestrHouses";
            }
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            dateReport = baseParams.Params.GetAs("reportDate", DateTime.MinValue);
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceEmergencyObject = Container.Resolve<IDomainService<EmergencyObject>>();
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();

            var dictMuName = serviceMunicipality.GetAll()
                                   .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Id))
                                   .Select(x => new { x.Id, x.Name })
                                   .OrderBy(x => Name)
                                   .ToDictionary(x => x.Id, v => v.Name);

            var dictEmergencyObject =
                serviceEmergencyObject.GetAll()
                                      .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                                      .Select(x => new
                                              {
                                                  MuId = x.RealityObject.Municipality.Id,
                                                  RoAddress = x.RealityObject.Address,
                                                  RoId = x.RealityObject.Id,
                                                  RoTotalArea = x.RealityObject.AreaMkd,
                                                  RoNumberLiving = x.RealityObject.NumberLiving,
                                                  x.ActualInfoDate,
                                                  x.DocumentDate,
                                                  x.DocumentNumber,
                                                  ReasonInexpedient = x.ReasonInexpedient.Name,
                                                  x.ResettlementFlatAmount,
                                                  x.ResettlementFlatArea,
                                                  FurtherUseName = x.FurtherUse.Name
                                              })
                                      .AsEnumerable()
                                      .GroupBy(x => x.MuId)
                                      .ToDictionary(k => k.Key, v => v.ToList());

            reportParams.SimpleReportParams["ДатаОтчета"] = string.Format("«{0}» месяц {1}  {2} года", dateReport.Day, dateReport.Month.ToString(), dateReport.Year);
            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("SectionMu");
            var sectionData = sectionMu.ДобавитьСекцию("SectionData");

            foreach (var mu in dictMuName)
            {
                var munId = mu.Key;
                
                sectionMu.ДобавитьСтроку();
                sectionMu["Мо"] = mu.Value;

                if (!dictEmergencyObject.ContainsKey(munId))
                {
                    continue;
                }

                var listRo = dictEmergencyObject[munId];
                var num = 0;
                foreach (var ro in listRo.OrderBy(x => x.RoAddress))
                {
                    sectionData.ДобавитьСтроку();

                    sectionData["НомерПП"] = ++num;
                    sectionData["Адрес"] = ro.RoAddress;
                    sectionData["ДатаВводаЭкспл"] = ro.ActualInfoDate != null
                                                        ? ro.ActualInfoDate.ToDateTime().ToShortDateString()
                                                        : string.Empty;
                    sectionData["ОбщПлощадь"] = ro.RoTotalArea;
                    sectionData["Дата"] = ro.DocumentDate;
                    sectionData["Номер"] = ro.DocumentNumber;
                    sectionData["ОснованиеАварийн"] = ro.ReasonInexpedient;
                    sectionData["ЧислЖильцов"] = ro.RoNumberLiving;
                    sectionData["КолВоЖилПом"] = ro.ResettlementFlatAmount;
                    sectionData["ПлощадьЖилПом"] = ro.ResettlementFlatArea;
                    sectionData["ДальнейДействия"] = ro.FurtherUseName;
                }

                //итоги Мо
                sectionMu["ИтогоМо1"] = listRo.Sum(x => x.RoTotalArea).ToDecimal();
                sectionMu["ИтогоМо2"] = listRo.Sum(x => x.RoNumberLiving).ToDecimal();
                sectionMu["ИтогоМо3"] = listRo.Sum(x => x.ResettlementFlatAmount).ToDecimal();
                sectionMu["ИтогоМо4"] = listRo.Sum(x => x.ResettlementFlatArea).ToDecimal();
            }

            //общие итоги
            var tmpValues = dictEmergencyObject.Values.SelectMany(x => x).ToList();
            reportParams.SimpleReportParams["Итого1"] = tmpValues.Sum(x => x.RoTotalArea).ToDecimal();
            reportParams.SimpleReportParams["Итого2"] = tmpValues.Sum(x => x.RoNumberLiving).ToDecimal();
            reportParams.SimpleReportParams["Итого3"] = tmpValues.Sum(x => x.ResettlementFlatAmount).ToDecimal();
            reportParams.SimpleReportParams["Итого4"] = tmpValues.Sum(x => x.ResettlementFlatArea).ToDecimal();

        }
    }
}