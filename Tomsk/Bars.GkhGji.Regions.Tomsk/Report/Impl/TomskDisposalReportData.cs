namespace Bars.GkhGji.Regions.Tomsk.Report
{
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class TomskDisposalReportData: ITomskDisposalReportData
    {
        public virtual IDomainService<InspectionAppealCits> inspectionAppealCitsDomain { get; set; }

        public virtual IDomainService<BaseJurPerson> baseJurPersonDomain { get; set; }

        public virtual IDomainService<AppealCitsSource> appealCitsSourceDomain { get; set; }

        public virtual IDomainService<DocumentGjiChildren> docChildrenDomain { get; set; }
        
        public virtual IDomainService<InspectionGjiRealityObject> inspRoDomain { get; set; }

        public DisposalReportData GetData(Bars.GkhGji.Entities.Disposal disposal)
        {
            var result = new DisposalReportData();

            if (disposal == null) return result;

            result.Disposal = disposal;

            // ТипОснованияПроверки
            result.InspectionTypeBase = disposal.Inspection.TypeBase;
            result.InspectionTypeBaseName = disposal.Inspection.TypeBase.GetEnumMeta().Display;

            // НомерОснованияПроверки
            result.InspectionNumber = disposal.Inspection.InspectionNumber;
            // ДатаОснованияПроверки
            result.InspectionDate = disposal.Inspection.ObjectCreateDate;
            result.InspectionDateStr = result.InspectionDate.Value.ToString("dd.MM.yyyy г.");

            var appealCitsData =
                    inspectionAppealCitsDomain.GetAll()
                    .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                                              .Select(
                                                  x =>
                                                  new
                                                  {
                                                      x.AppealCits.Id,
                                                      x.AppealCits.DocumentNumber,
                                                      x.AppealCits.DateFrom,
                                                      x.AppealCits.Correspondent,
                                                      x.AppealCits.TypeCorrespondent,
                                                      x.AppealCits.Description
                                                  })
                    .FirstOrDefault();

            result.AppealSourceName = string.Empty;
            result.AppealSourceNameGenetive = string.Empty;

            if (appealCitsData != null)
            {
                result.AppealCitsId = appealCitsData.Id;
                result.AppealCitsDate = appealCitsData.DateFrom;
                result.AppealCitsDateStr = result.AppealCitsDate.HasValue
                        ? result.AppealCitsDate.Value.ToString("dd.MM.yyyy г.")
                        : string.Empty;
                result.AppealCitsNumber = appealCitsData.DocumentNumber;
                result.AppealCitsCorrespondent = appealCitsData.Correspondent;
                result.AppealCitsTypeCorrespondent = appealCitsData.TypeCorrespondent.GetEnumMeta().Display;
                result.AppealCitsQuestion = appealCitsData.Description;

                var src = appealCitsSourceDomain.GetAll()
                                      .Where(x => x.AppealCits.Id == appealCitsData.Id)
                                      .Select(x => new { x.RevenueSource.Name, x.RevenueSource.NameGenitive })
                                      .FirstOrDefault();

                result.AppealSourceName = string.Empty;
                if (src != null)
                {
                    result.AppealSourceName = src.Name;
                    result.AppealSourceNameGenetive = src.NameGenitive;
                }

            }

            var jurPersonPlan = baseJurPersonDomain
                                .GetAll()
                                .Where(x => x.Id == disposal.Inspection.Id)
                                .Select(x => x.Plan)
                                .FirstOrDefault();

            if (jurPersonPlan != null && jurPersonPlan.DateStart.HasValue)
            {
                result.JurPersonPlanYear = jurPersonPlan.DateStart.Value.Year;
            }

            var realityObjs = inspRoDomain.GetAll()
                                .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                                .Select(x => x.RealityObject)
                                .ToList();

            var realObjs = new StringBuilder();
            if (realityObjs.Count > 0)
            {
                foreach (var realityObject in realityObjs)
                {
                    if (realObjs.Length > 0)
                        realObjs.Append("; ");

                    var housing = string.Empty;
                    if (!string.IsNullOrEmpty(realityObject.FiasAddress.Housing))
                    {
                        housing = string.Format(", корп. {0}", realityObject.FiasAddress.Housing);
                    }

                    realObjs.AppendFormat("{0}, д.{1}{2}", realityObject.FiasAddress.StreetName, realityObject.FiasAddress.House, housing);
                }

                result.HouseAndAdrress = string.Format("{0}, ", realityObjs.FirstOrDefault().FiasAddress.PlaceName) + realObjs.ToString();

            }

            return result;
        }

        public string GetBaseSurvey(DisposalReportData reportData)
        {
            string result = string.Empty;

            if (reportData.Disposal.TypeDisposal == TypeDisposalGji.DocumentGji)
            {
                var prescriptions = docChildrenDomain.GetAll()
                                     .Where(
                                         x =>
                                         x.Children.Id == reportData.Disposal.Id
                                         && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                                     .Select(x => new { x.Parent.DocumentNumber, x.Parent.DocumentDate })
                                     .ToList();

                if (prescriptions.Any())
                {
                    // проверка исполнения ранее выданного предписания № … от 20.01.2014 г.
                    if (prescriptions.Count == 1)
                    {
                        result = "проверка исполнения ранее выданного предписания ";
                    }
                    else
                    {
                        result = "проверка исполнений ранее выданных предписаний: ";
                    }

                    result +=
                        prescriptions.Select(
                            x =>
                            string.Format(
                                "№ {0} от {1} г.",
                                !string.IsNullOrEmpty(x.DocumentNumber) ? x.DocumentNumber : string.Empty,
                                x.DocumentDate.HasValue ? x.DocumentDate.Value.ToShortDateString() : string.Empty))
                                     .Aggregate((x, y) => !string.IsNullOrEmpty(y) ? y + ", " + x : x);

                }
            }
            else if (reportData.InspectionTypeBase == TypeBase.CitizenStatement)
            {
                result = string.Format(
                    "обращение граждан № {0} от {1}",
                    reportData.AppealCitsNumber,
                    reportData.AppealCitsDateStr);
            }
            else if (reportData.InspectionTypeBase == TypeBase.DisposalHead)
            {
                result =
                    string.Format(
                        "поручение руководителя (вх. № {0} от {1}) по вопросу {2} в доме, расположенном по адресу: {3}",
                                                  reportData.InspectionNumber,
                                                  reportData.InspectionDateStr,
                                                  reportData.AppealCitsQuestion,
                                                  reportData.HouseAndAdrress);
            }
            else if (reportData.InspectionTypeBase == TypeBase.ProsecutorsClaim)
            {
                result =
                    string.Format(
                        "требование {0} по вопросу {1} в доме, расположенном по адресу: {2}",
                                                  reportData.AppealSourceNameGenetive,
                                                  reportData.AppealCitsQuestion,
                                                  reportData.HouseAndAdrress);
            }
            else if (reportData.InspectionTypeBase == TypeBase.PlanJuridicalPerson)
            {
                result = string.Format(
                    "план проведения плановых проверок на {0} год", reportData.JurPersonPlanYear);
            }

            return result;
        }
    }
}
