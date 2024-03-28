namespace Bars.Gkh.Gasu.Export
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class InspGasAgregations: BaseDataExportService
    {
        protected DateTime Period;

        Dictionary<int, Tuple<string, string>> codes = new Dictionary<int, Tuple<string, string>>
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

        public override ReportStreamResult ExportData(BaseParams baseParams)
        {

            var dateStart = baseParams.Params["periodStart"].ToDateTime();
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(GetData(dateStart));
            writer.Flush();
            stream.Position = 0;

            return new ReportStreamResult(stream, "export.xml");
        }

        private string GetData(DateTime periodStart)
        {
            Period = periodStart;

            var result = new StringBuilder();
            var periodStr = Period.ToShortDateString();
            var allData = new List<Dictionary<string, string>>
            {
                GetControlActions(),
                GetActSurveyAreaDependingOnForm("Смешанная"),
                GetActSurveyAreaDependingOnForm("Частная"),
                GetActSurveyAreaDependingOnForm("Муниципальная"),
                GetActSurveyAreaDependingOnForm("Государственная"),
                GetViolations(),
                GetDocuments<Protocol>(Period),
                GetDocuments<Prescription>(Period),
                GetProsecutionDocs(),
                GetDocuments<Resolution>(Period),
                GetConsideredResolutions(),
                GetClosedResolutions(),
                GetProtocolsToCourt(),
                GetResolutionPenaltyAmount(),
                GetResolution(),
                GetCommunalViolations()
            };

            int curCode = 1;

            result.Append(@"<?xml version=""1.0"" encoding=""windows-1251""?>");

            foreach (var data in allData)
            {
                foreach (var row in data)
                {
                    result.Append(String.Format(@"
         <ROW>
           <N_CALLVL>4</N_CALLVL>
           <D_CALEN>{0}</D_CALEN>
           <ID_INFO>1</ID_INFO>
           <ID_SINFO>4</ID_SINFO> 
           <ID_TER>{1}</ID_TER>
           <N_VAL>{2}</N_VAL>
           <ID_POK>{3}</ID_POK>
           <ID_UNITS>{4}</ID_UNITS>
        </ROW>", 
                   periodStr, row.Key, row.Value, codes[curCode].Item1, codes[curCode].Item2));

                }

                curCode++;
            }

            return result.ToString();
        }

        #region Запросы
        public Dictionary<string, string> GetProsecutionDocs()
        {

            var s = new Stopwatch();

            var disposals =
                Container.Resolve<IDomainService<DocumentGji>>().GetAll()
                    .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal)
                    .Select(x => x.Stage.Id);
         
            var docs = Container.Resolve<IDomainService<Prescription>>().GetAll()
                .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == Period.Year && x.DocumentDate.Value.Month == Period.Month)
                .Where(x => disposals.Any(y => y == x.Stage.Parent.Id))
                .Where(x => x.Contragent.Municipality.Okato != null)
                .Select(x => new
                {
                    x.Id,
                    x.Contragent.Municipality.Okato
                })
                .ToList();

            var resolutionCanceled = Container.Resolve<IDomainService<ResolutionDispute>>().GetAll()
                .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == Period.Year && x.DocumentDate.Value.Month == Period.Month)
                .Select(x => x.Id);

            var resolutionMonth = Container.Resolve<IDomainService<Resolution>>().GetAll()
                .Where(x => x.DocumentDate.HasValue &&
                            x.DocumentDate.Value.Year == Period.Year &&
                            x.DocumentDate.Value.Month == Period.Month)
                .Where(x => x.Sanction.Code == "1" && x.Stage != null)
                .Where(x => disposals.Any(y => y == x.Stage.Parent.Id))
                .Where(x => x.Contragent.Municipality.Okato != null)
                .Where(x => !resolutionCanceled.Any(y => y == x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Contragent.Municipality.Okato
                });

            docs.AddRange(resolutionMonth.ToList());

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

        public Dictionary<string, string> GetActSurveyAreaDependingOnForm(string form)
        {
            return Container.Resolve<IDomainService<ActSurveyRealityObject>>().GetAll()
                .Where(
                    x => x.ActSurvey.DocumentDate.HasValue &&
                        x.ActSurvey.DocumentDate.Value.Year == Period.Year &&
                        x.ActSurvey.DocumentDate.Value.Month == Period.Month)
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


        /// <summary>
        /// 1)	Количество проведённых мероприятий по контролю
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetControlActions()
        {
            return Container.Resolve<IDomainService<ActCheckPeriod>>().GetAll()
                .Where(x =>
                    x.DateCheck.HasValue &&
                    x.DateCheck.Value.Year == Period.Year &&
                    x.DateCheck.Value.Month == Period.Month &&
                    x.ActCheck.Inspection.Contragent.Municipality.Okato != null)
                .Select(x => new { x.ActCheck.Inspection.Contragent.Municipality.Okato, DocId = x.ActCheck.Id })
                .AsEnumerable()
                .GroupBy(x => x.Okato)
                .ToDictionary(x => x.Key, y => y.Count().ToString());
        }

        /// <summary>
        /// 2)	Количество обследованной общей площади жилищного фонда смешанной формы собственности 
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetMixedFormArea(DateTime period)
        {
            return GetActSurveyAreaDependingOnForm("Смешанная");
        }

        /// <summary>
        /// 3)	Количество обследованной общей площади жилищного фонда частной формы собственности 
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetPrivateFormArea(DateTime period)
        {
            return GetActSurveyAreaDependingOnForm("Частная");
        }

        /// <summary>
        /// 4)	Количество обследованной общей площади жилищного фонда муниципальной формы собственности 
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetMunicipalityFormArea(DateTime period)
        {
            return GetActSurveyAreaDependingOnForm("Муниципальная");
        }

        /// <summary>
        /// 5)	Количество обследованной общей площади жилищного фонда государственной формы собственности 
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetStateFormArea(DateTime period)
        {
            return GetActSurveyAreaDependingOnForm("Государственная");
        }

        /// <summary>
        /// 6)	Количество выявленных нарушений
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetViolations()
        {
            return Container.Resolve<IDomainService<ActCheckViolation>>().GetAll()
            .Where(x =>
                x.ActObject.ActCheck.DocumentDate.HasValue &&
                x.ActObject.ActCheck.DocumentDate.Value.Year == Period.Year &&
                x.ActObject.ActCheck.DocumentDate.Value.Month == Period.Month &&
                x.ActObject.RealityObject.Municipality.Okato != null)
            .Select(x => new { x.ActObject.RealityObject.Municipality.Okato, DocId = x.Id })
            .AsEnumerable()
            .GroupBy(x => x.Okato)
            .ToDictionary(x => x.Key, y => y.Count().ToString());
        }

        /// <summary>
        /// Количество документов ГЖИ за период
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<long, int> GetDocuments<T>() where T : DocumentGji
        {
            return Container.Resolve<IDomainService<T>>().GetAll()
                .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == Period.Year && x.DocumentDate.Value.Month == Period.Month)
                .Select(x => new { x.Inspection.Contragent.Municipality.Id, DocId = x.Id })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Count());
        }

        /// <summary>
        /// 7)	Количество составленных Протоколов
        /// </summary>
        /// <returns></returns>
        public Dictionary<long, int> GetProtocols()
        {
            return GetDocuments<Protocol>();
        }

        /// <summary>
        /// 8)	Количество составленных Предписаний
        /// </summary>
        /// <returns></returns>
        public Dictionary<long, int> GetPerscriptions()
        {
            return GetDocuments<Prescription>();
        }

        /// <summary>
        /// 9)	Количество составленных прокуратурой исполнительных документов 
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetProsecutionDocs(DateTime period)
        {

            var disposals =Container.Resolve<IDomainService<Disposal>>().GetAll()
                              .Select(x => new {
                                  StageId = x.Stage.Id,
                              }).ToList();
            var disposalStageIds = disposals.Select(x => x.StageId).ToList();
            var docs = Container.Resolve<IDomainService<Prescription>>().GetAll()
                .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == period.Year && x.DocumentDate.Value.Month == period.Month)
                .Where(x => disposalStageIds.Contains(x.Stage.Parent.Id))
                .Where(x => x.Contragent.Municipality.Okato != null)
                .Select(x => new {
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
                .Select(x => new {
                    x.Okato,
                    x.Id
                }).ToList());

            var dict = docs.GroupBy(x => x.Okato).ToDictionary(x => x.Key, y => y.Count());

            return dict;
        }

        /// <summary>
        /// 10)	Количество составленных Постановлений
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<long, int> GetResolutions()
        {
            return GetDocuments<Resolution>(); 
        }

        /// <summary>
        /// 11)	Количество рассмотренных Постановлений
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetConsideredResolutions()
        {
            return Container.Resolve<IDomainService<Resolution>>().GetAll()
            .Where(x =>
                x.DocumentDate.HasValue &&
                x.DocumentDate.Value.Year == Period.Year &&
                x.DocumentDate.Value.Month == Period.Month)
            .Where(x => x.Sanction == null || x.Sanction.Code != "0" && x.Contragent.Municipality.Okato != null)
            .Select(x => new { x.Contragent.Municipality.Okato, DocId = x.Id })
            .AsEnumerable()
            .GroupBy(x => x.Okato)
            .ToDictionary(x => x.Key, y => y.Count().ToString());
        }

        /// <summary>
        /// 12)	Количество прекращенных  Постановлений
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetClosedResolutions()
        {
            return Container.Resolve<IDomainService<Resolution>>().GetAll()
            .Where(x =>
                x.DocumentDate.HasValue &&
                x.DocumentDate.Value.Year == Period.Year &&
                x.DocumentDate.Value.Month == Period.Month &&
                x.Contragent.Municipality.Okato != null)
            .Where(x => x.Sanction.Code == "2")
            .Select(x => new { x.Contragent.Municipality.Okato, DocId = x.Id })
            .AsEnumerable()
            .GroupBy(x => x.Okato)
            .ToDictionary(x => x.Key, y => y.Count().ToString());
        }

        /// <summary>
        /// 13)	Количество Протоколов, документы по которым переданы в суд
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetProtocolsToCourt()
        {
            return Container.Resolve<IDomainService<Protocol>>().GetAll()
                .Where(x =>
                    x.DocumentDate.HasValue &&
                    x.DocumentDate.Value.Year == Period.Year &&
                    x.DocumentDate.Value.Month == Period.Month &&
                    x.Contragent.Municipality.Okato != null)
                .Where(x => x.ToCourt)
                .Select(x => new { x.Contragent.Municipality.Okato, DocId = x.Id })
                .AsEnumerable()
                .GroupBy(x => x.Okato)
                .ToDictionary(x => x.Key, y => y.Count().ToString());
        }

        /// <summary>
        /// 14)	Сумма предъявленных штрафных санкций
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetResolutionPenaltyAmount()
        {
            return Container.Resolve<IDomainService<Resolution>>().GetAll()
                .Where(x =>
                    x.DocumentDate.HasValue &&
                    x.DocumentDate.Value.Year == Period.Year &&
                    x.DocumentDate.Value.Month == Period.Month &&
                    x.Municipality.Okato != null)
                .Select(x => new { x.Municipality.Okato, x.PenaltyAmount })
                .AsEnumerable()
                .GroupBy(x => x.Okato)
                .ToDictionary(x => x.Key, y => y.Sum(z => z.PenaltyAmount).ToString());
        }

        /// <summary>
        /// 15)	Сумма взысканных штрафных санкций 
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetResolution()
        {
            return Container.Resolve<IDomainService<ResolutionPayFine>>().GetAll()
                .Where(x =>
                    x.DocumentDate.HasValue &&
                    x.DocumentDate.Value.Year == Period.Year &&
                    x.DocumentDate.Value.Month == Period.Month &&
                    x.Resolution.Municipality.Okato != null)
                .Select(x => new { x.Resolution.Municipality.Okato, x.Amount })
                .AsEnumerable()
                .GroupBy(x => x.Okato)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.Amount).ToString());
        }

        /// <summary>
        /// 16)	Количество нарушений правил предоставления коммунальных услуг гражданам
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetCommunalViolations()
        {
            var featCodes = new List<string>
            {
                "6", "7", "8",
                "9", "10", "11", "12"
            };

            var featDict = Container.Resolve<IDomainService<ViolationFeatureGji>>().GetAll()
                .Where(x => x.FeatureViolGji.Code != null)
                .Where(x => featCodes.Contains(x.FeatureViolGji.Code.Trim()))
                .Select(x => new
                {
                    x.ViolationGji.Id,
                    x.FeatureViolGji.Code
                });

            return Container.Resolve<IDomainService<ActCheckViolation>>().GetAll()
                .Where(x =>
                    x.ActObject.ActCheck.DocumentDate.HasValue &&
                    x.ActObject.ActCheck.DocumentDate.Value.Year == Period.Year &&
                    x.ActObject.ActCheck.DocumentDate.Value.Month == Period.Month)
                .Where(x => featDict.Any(y => y.Id == x.InspectionViolation.Violation.Id))
                .Select(x => new
                {
                    x.ActObject.RealityObject.Municipality.Okato,
                    DocId = x.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Okato)
                .ToDictionary(x => x.Key, y => y.Count().ToString());
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
                    feature.Contains(12)){
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}