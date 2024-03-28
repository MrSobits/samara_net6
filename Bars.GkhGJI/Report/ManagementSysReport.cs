using System.Collections.Generic;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Report
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using B4.Modules.Reports;
    using Bars.B4.Modules.DataExport.Domain;
    
    using Castle.Windsor;
    using Bars.GkhGji.Entities;


    class ManagementSysReport : BasePrintForm
    {
        protected DateTime period;

        public ManagementSysReport()
            : base(new ReportTemplateBinary(Bars.GkhGji.Properties.Resources.ManagementSysExport))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var sectionRow = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRow");

                #region Запросы 

            var codes = new Dictionary<int, Tuple<string, string>>
            {
                {1 ,  new Tuple<string, string> ("390009997", "642")},
                {2 ,  new Tuple<string, string> ("90318108",  "642")},
                {3 ,  new Tuple<string, string> ("90318110",  "642")},
                {4 ,  new Tuple<string, string> ("90318107",  "642")},
                {5 ,  new Tuple<string, string> ("90318109",  "642")},
                {6 ,  new Tuple<string, string> ("90317962",  "642")},
                {7 ,  new Tuple<string, string> ("90317971",  "642")},
                {8 ,  new Tuple<string, string> ("90317970",  "642")},
                {9 ,  new Tuple<string, string> ("90317969",  "642")},
                {10 , new Tuple<string, string> ("390009998", "642")},
                {11 , new Tuple<string, string> ("390009999", "642")},
                {12 , new Tuple<string, string> ("390001001", "642")},
                {13 , new Tuple<string, string> ("390001002", "642")},
                {14 , new Tuple<string, string> ("90317976",  "383")},
                {15 , new Tuple<string, string> ("90317977",  "383")},
                {16 , new Tuple<string, string> ("390001003", "642")}
            };

                // Количество проведённых мероприятий по контролю
                var data1 = Container.Resolve<IDomainService<ActCheckPeriod>>().GetAll()
                    .Where(x =>
                        x.DateCheck.HasValue &&
                        x.DateCheck.Value.Year == period.Year &&
                        x.DateCheck.Value.Month == period.Month &&
                        x.ActCheck.Inspection.Contragent.Municipality.Okato != null)
                    .Select(x => new { x.ActCheck.Inspection.Contragent.Municipality.Okato, DocId = x.ActCheck.Id })
                    .AsEnumerable()
                    .GroupBy(x => x.Okato)
                    .ToDictionary(x => x.Key, y => y.Count().ToString());

                // Количество обследованной общей площади жилищного фонда смешанной формы собственности 
                var data2 = GetActSurveyAreaDependingOnForm(period, "Смешанная");

                // Количество обследованной общей площади жилищного фонда частной формы собственности 
                var data3 = GetActSurveyAreaDependingOnForm(period, "Частная");

                // Количество обследованной общей площади жилищного фонда муниципальной формы собственности 
                var data4 = GetActSurveyAreaDependingOnForm(period, "Муниципальная");

                // Количество обследованной общей площади жилищного фонда государственной формы собственности
                var data5 = GetActSurveyAreaDependingOnForm(period, "Государственная");

                // Количество выявленных нарушений
                var data6 = Container.Resolve<IDomainService<ActCheckViolation>>().GetAll()
                .Where(x =>
                    x.ActObject.ActCheck.DocumentDate.HasValue &&
                    x.ActObject.ActCheck.DocumentDate.Value.Year == period.Year &&
                    x.ActObject.ActCheck.DocumentDate.Value.Month == period.Month &&
                    x.ActObject.RealityObject.Municipality.Okato != null)
                .Select(x => new {x.ActObject.RealityObject.Municipality.Okato, DocId = x.Id})
                .AsEnumerable()
                .GroupBy(x => x.Okato)
                .ToDictionary(x => x.Key, y => y.Count().ToString());

                // Количество документов ГЖИ за период
                var data7 = GetDocuments<Protocol>(period);

                // Количество составленных Предписаний
                var data8 = GetDocuments<Prescription>(period);

                // Количество составленных прокуратурой исполнительных документов 
                var data9 = GetProsecutionDocs();

                // Количество составленных Постановлений
                var data10 = GetDocuments<Resolution>(period);

                // Количество рассмотренных Постановлений
                var data11 = Container.Resolve<IDomainService<Resolution>>().GetAll()
                .Where(x =>
                    x.DocumentDate.HasValue &&
                    x.DocumentDate.Value.Year == period.Year &&
                    x.DocumentDate.Value.Month == period.Month)
                .Where(x => x.Sanction == null || x.Sanction.Code != "0" && x.Contragent.Municipality.Okato != null)
                .Select(x => new {x.Contragent.Municipality.Okato, DocId = x.Id})
                .AsEnumerable()
                .GroupBy(x => x.Okato)
                .ToDictionary(x => x.Key, y => y.Count().ToString());
                
                //Количество прекращенных  Постановлений
                var data12 = Container.Resolve<IDomainService<Resolution>>().GetAll()
                .Where(x =>
                    x.DocumentDate.HasValue &&
                    x.DocumentDate.Value.Year == period.Year &&
                    x.DocumentDate.Value.Month == period.Month &&
                    x.Contragent.Municipality.Okato != null)
                .Where(x => x.Sanction.Code == "2")
                .Select(x => new { x.Contragent.Municipality.Okato, DocId = x.Id })
                .AsEnumerable()
                .GroupBy(x => x.Okato)
                .ToDictionary(x => x.Key, y => y.Count().ToString());

                // Количество Протоколов, документы по которым переданы в суд
                var data13 = Container.Resolve<IDomainService<Protocol>>().GetAll()
                    .Where(x =>
                        x.DocumentDate.HasValue &&
                        x.DocumentDate.Value.Year == period.Year &&
                        x.DocumentDate.Value.Month == period.Month &&
                        x.Contragent.Municipality.Okato != null)
                    .Where(x => x.ToCourt)
                    .Select(x => new { x.Contragent.Municipality.Okato, DocId = x.Id })
                    .AsEnumerable()
                    .GroupBy(x => x.Okato)
                    .ToDictionary(x => x.Key, y => y.Count().ToString());

                // Сумма предъявленных штрафных санкций
                var data14 = Container.Resolve<IDomainService<Resolution>>().GetAll()
                    .Where(x =>
                        x.DocumentDate.HasValue &&
                        x.DocumentDate.Value.Year == period.Year &&
                        x.DocumentDate.Value.Month == period.Month &&
                        x.Municipality.Okato != null)
                    .Select(x => new { x.Municipality.Okato, x.PenaltyAmount })
                    .AsEnumerable()
                    .GroupBy(x => x.Okato)
                    .ToDictionary(x => x.Key, y => y.Sum(z => z.PenaltyAmount).ToString());

                // cумма взысканных штрафных санкций 
                var data15 = Container.Resolve<IDomainService<ResolutionPayFine>>().GetAll()
                    .Where(x =>
                        x.DocumentDate.HasValue &&
                        x.DocumentDate.Value.Year == period.Year &&
                        x.DocumentDate.Value.Month == period.Month &&
                        x.Resolution.Municipality.Okato !=null)
                    .Select(x => new { x.Resolution.Municipality.Okato, x.Amount })
                    .AsEnumerable()
                    .GroupBy(x => x.Okato)
                    .ToDictionary(x => x.Key, x => x.Sum(y => y.Amount).ToString());

                // Количество нарушений правил предоставления коммунальных услуг гражданам

                var featDict = Container.Resolve<IDomainService<ViolationFeatureGji>>().GetAll()
                .Select(x => new
                {
                    x.ViolationGji.Id,
                    x.FeatureViolGji.Code
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Code.ToInt()).ToArray());

            var data16 = Container.Resolve<IDomainService<ActCheckViolation>>().GetAll()
                .Where(x =>
                    x.ActObject.ActCheck.DocumentDate.HasValue &&
                    x.ActObject.ActCheck.DocumentDate.Value.Year == period.Year &&
                    x.ActObject.ActCheck.DocumentDate.Value.Month == period.Month).ToList()
                .Where(x => IsSpecificCommunalViolation(x.InspectionViolation.Violation.Id, featDict))
                .AsEnumerable()
                .Select(x => new { x.ActObject.RealityObject.Municipality.Okato, DocId = x.Id })
                .GroupBy(x => x.Okato)
                .ToDictionary(x => x.Key, y => y.Count().ToString());

            #endregion

            var periodStr = period.ToShortDateString();
            var allData = new List<Dictionary<string, string>>();
            
            allData.Add(data1);
            allData.Add(data2);
            allData.Add(data3);
            allData.Add(data4);
            allData.Add(data5);
            allData.Add(data6);
            allData.Add(data7);
            allData.Add(data8);
            allData.Add(data9);
            allData.Add(data10);
            allData.Add(data11);
            allData.Add(data12);
            allData.Add(data13);
            allData.Add(data14);
            allData.Add(data15);
            allData.Add(data16);

            int curCode = 1;

            foreach (var data in allData)
            {
                foreach (var row in data)
                {
                    sectionRow.ДобавитьСтроку();
                    sectionRow["Уровень_календаря"] = "4";
                    sectionRow["Начало_периода"] = periodStr;
                    sectionRow["Вид_данных"] = "1";
                    sectionRow["Источник_данных"] = "4";
                    sectionRow["ОКАТО"] = row.Key;
                    sectionRow["Значение_показателя"] = row.Value;
                    sectionRow["Ключ_показателя"] = codes[curCode].Item1;
                    sectionRow["ОКЕИ"] = codes[curCode].Item2;
                }

                curCode++;
            }
        }

        private bool IsSpecificCommunalViolation(long id, Dictionary<long, int[]> featDict)
        {
            if (featDict.ContainsKey(id))
            {
                var feature = featDict[id];
                if (feature.Contains(6) ||
                    feature.Contains(7) ||
                    feature.Contains(8) ||
                    feature.Contains(9) ||
                    feature.Contains(10) ||
                    feature.Contains(11) ||
                    feature.Contains(12))
                {
                    return true;
                }
            }

            return false;
        }

        public Dictionary<string, string> GetProsecutionDocs()
        {

            var disposals = Container.Resolve<IDomainService<Disposal>>().GetAll()
                              .Select(x => new
                              {
                                  StageId = x.Stage.Id,
                              }).ToList();
            var disposalStageIds = disposals.Select(x => x.StageId).ToList();
            var docs = Container.Resolve<IDomainService<Prescription>>().GetAll()
                .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == period.Year && x.DocumentDate.Value.Month == period.Month)
                .Where(x => disposalStageIds.Contains(x.Stage.Parent.Id))
                .Where(x => x.Contragent.Municipality.Okato != null)
                .Select(x => new
                {
                    x.Contragent.Municipality.Okato,
                    x.Id
                })
                .ToList();

            var resolutionMonthTmp = Container.Resolve<IDomainService<Resolution>>().GetAll()
            .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == period.Year && x.DocumentDate.Value.Month == period.Month)
            .Where(x => x.Sanction.Code == "1" && x.Stage != null)
            .Where(x => disposalStageIds.Contains(x.Stage.Parent.Id))
            .Where(x => x.Contragent.Municipality.Okato != null)
            .Select(x => new
            {
                x.Id,
                ParentstageId = x.Stage.Parent.Id,
                x.Contragent.Municipality.Okato
            })
            .ToList();

            var resolutionCanceled = Container.Resolve<IDomainService<ResolutionDispute>>().GetAll()
                .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == period.Year && x.DocumentDate.Value.Month == period.Month)
                .Select(x => x.Id)
                .ToList();

            var resolutionMonth = resolutionMonthTmp.Where(x => !resolutionCanceled.Contains(x.Id)).ToList();
            docs.AddRange(resolutionMonth.Where(x => disposalStageIds.Contains(x.ParentstageId))
                .Select(x => new
                {
                    x.Okato,
                    x.Id
                }).ToList());

            var dict = docs.GroupBy(x => x.Okato).ToDictionary(x => x.Key, y => y.Count().ToString());

            return dict;
        }

        /// <summary>
        /// Количество документов ГЖИ за период
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetDocuments<T>(DateTime period) where T : DocumentGji
        {
            return Container.Resolve<IDomainService<T>>().GetAll()
                .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == period.Year && x.DocumentDate.Value.Month == period.Month && x.Inspection.Contragent.Municipality.Okato != null)
                .Select(x => new { x.Inspection.Contragent.Municipality.Okato, DocId = x.Id })
                .AsEnumerable()
                .GroupBy(x => x.Okato)
                .ToDictionary(x => x.Key, y => y.Count().ToString());
        }

        public Dictionary<string, string> GetActSurveyAreaDependingOnForm(DateTime period, string form)
        {
            return Container.Resolve<IDomainService<ActSurveyRealityObject>>().GetAll()
                .Where(
                    x => x.ActSurvey.DocumentDate.HasValue &&
                        x.ActSurvey.DocumentDate.Value.Year == period.Year &&
                        x.ActSurvey.DocumentDate.Value.Month == period.Month)
                .Where(
                    x =>
                        x.ActSurvey.FactSurveyed == SurveyResult.Surveyed &&
                        x.RealityObject.TypeOwnership.Name == form &&
                        x.RealityObject.Municipality.Okato != null)
                .Select(x => new { x.RealityObject.Municipality.Okato, x.ActSurvey.Area })
                .AsEnumerable()
                .GroupBy(x => x.Okato)
                .ToDictionary(x => x.Key, y => y.Sum(z => z.Area).ToString());
        }

        public override string Name
        {
            get
            {
                return "ManagementSysExport"; 
            }
        }

        public override string Desciption
        {
            get
            {
                return "Выгрузка для ГАС УПРАВЛЕНИЕ";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Экспорты";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.InformationOfManagOrg";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.InformationOfManagOrg";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            period = baseParams.Params["periodStart"].ToDateTime();
        }
    }
}
