namespace Bars.GkhGji.Regions.Samara.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class ProtocolResponsibility_2 : BasePrintForm
    {
        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;

        private long[] municipalityIds;
        private bool canceled;
        private bool returned;
        private bool remarked;

        public ProtocolResponsibility_2()
            : base(new ReportTemplateBinary(Properties.Resources.ProtocolResponsibility_2))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.GJI.ProtocolResponsibility_2"; }
        }

        public override string Name
        {
            get { return "Отчет по протоколам-2"; }
        }

        public override string Desciption
        {
            get { return "Отчет по протоколам-2"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ProtocolResponsibility_2"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.dateStart = baseParams.Params["dateStart"].ToDateTime();
            this.dateEnd = baseParams.Params["dateEnd"].ToDateTime();

            canceled = baseParams.Params["canceled"].ToBool();
            returned = baseParams.Params["returned"].ToBool();
            remarked = baseParams.Params["remarked"].ToBool();

            var m = baseParams.Params["municipalityIds"].ToString();
            municipalityIds = m.Length > 0 ? m.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
        }

        public Dictionary<long, string> GetArticleLaws()
        {
            var servDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var servProtocolArticleLaw = Container.Resolve<IDomainService<ProtocolArticleLaw>>();
            var servResolProsArticleLaw = Container.Resolve<IDomainService<ResolProsArticleLaw>>();

            var articleLaws = servDocumentGjiChildren.GetAll()
                .Join(
                    servProtocolArticleLaw.GetAll(),
                    a => a.Parent.Id,
                    b => b.Protocol.Id,
                    (a, b) => new
                    {
                        resolutionId = a.Children.Id,
                        articleLawName = b.ArticleLaw.Name,
                        childTypeDocument = a.Children.TypeDocumentGji,
                        childDocumentDate = a.Children.DocumentDate,
                        parentTypeDocument = a.Parent.TypeDocumentGji
                    })
               .Where(x => x.childTypeDocument == TypeDocumentGji.Resolution)
               .Where(x => x.childDocumentDate >= this.dateStart && x.childDocumentDate <= this.dateEnd)
               .Where(x => x.parentTypeDocument == TypeDocumentGji.Protocol)
               .ToList();

            articleLaws.AddRange(servDocumentGjiChildren.GetAll()
                .Join(
                    servResolProsArticleLaw.GetAll(),
                    a => a.Parent.Id,
                    b => b.ResolPros.Id,
                    (a, b) => new
                    {
                        resolutionId = a.Children.Id,
                        articleLawName = b.ArticleLaw.Name,
                        childTypeDocument = a.Children.TypeDocumentGji,
                        childDocumentDate = a.Children.DocumentDate,
                        parentTypeDocument = a.Parent.TypeDocumentGji
                    })
               .Where(x => x.childTypeDocument == TypeDocumentGji.Resolution)
               .Where(x => x.childDocumentDate >= this.dateStart && x.childDocumentDate <= this.dateEnd)
               .Where(x => x.parentTypeDocument == TypeDocumentGji.ResolutionProsecutor));

            return articleLaws
                .GroupBy(x => x.resolutionId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => y.articleLawName).Aggregate((a, b) => a + ", " + b));
        }

        public Dictionary<long, string> GetInspectors()
        {
            var servDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var servDocGjiInspector = Container.Resolve<IDomainService<DocumentGjiInspector>>();

            var inspectors = servDocumentGjiChildren.GetAll()
                .Join(
                    servDocGjiInspector.GetAll(),
                    a => a.Parent.Id,
                    b => b.DocumentGji.Id,
                    (a, b) => new
                    {
                        resolutionId = a.Children.Id,
                        inspector = b.Inspector.Fio ?? string.Empty,
                        childTypeDocument = a.Children.TypeDocumentGji,
                        childDocumentDate = a.Children.DocumentDate,
                        parentTypeDocument = a.Parent.TypeDocumentGji
                    })
               .Where(x => x.childTypeDocument == TypeDocumentGji.Resolution)
               .Where(x => x.childDocumentDate >= this.dateStart && x.childDocumentDate <= this.dateEnd)
               .Where(x => x.parentTypeDocument == TypeDocumentGji.Protocol)
               .ToList();

            return inspectors
                .GroupBy(x => x.resolutionId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Where(y => !string.IsNullOrEmpty(y.inspector)).Select(y => y.inspector).Aggregate((a, b) => a + ", " + b));

        }

        private Dictionary<long, PayFineProxy> GetPayFineData()
        {
            var payFineDict = Container.Resolve<IDomainService<ResolutionPayFine>>().GetAll()
              .Where(x => x.Resolution.DocumentDate >= this.dateStart && x.Resolution.DocumentDate <= this.dateEnd)
              .Select(x => new
              {
                  x.Resolution.Id,
                  x.Amount,
                  x.DocumentNum,
                  x.DocumentDate,
                  x.TypeDocumentPaid
              })
              .AsEnumerable()
              .GroupBy(x => x.Id)
              .Select(x => new PayFineProxy
              {
                  Id = x.Key,
                  Amount = x.Sum(y => y.Amount),
                  Documents = x.Select(y => string.Format("{0} №{1}", y.DocumentDate.HasValue ? y.DocumentDate.Value.ToShortDateString() : string.Empty, y.DocumentNum)).Aggregate((curr, next) => string.Format("{0}, {1}", curr, next)),
                  docDate = x.Select(y => string.Format("{0}", y.DocumentDate.HasValue ? y.DocumentDate.Value.ToShortDateString() : string.Empty)).Aggregate((curr, next) => string.Format("{0}, {1}", curr, next)),
                  typeDoc = x.Select(y => string.Format("{0}", y.TypeDocumentPaid.GetEnumMeta().Display)).Aggregate((curr, next) => string.Format("{0}, {1}", curr, next)),
              })
              .ToDictionary(x => x.Id);

            return payFineDict;
        }

        private Dictionary<long, DisputeProxy> GetDisputeData()
        {
            var disputeDict = Container.Resolve<IDomainService<ResolutionDispute>>().GetAll()
                .Where(x => x.Resolution.DocumentDate >= this.dateStart && x.Resolution.DocumentDate <= this.dateEnd)
                .Select(x => new
                {
                    x.Resolution.Id,
                    x.DocumentDate,
                    CourtVerdictName = x.CourtVerdict.Name,
                    CourtVerdictCode = x.CourtVerdict.Code
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .Select(x => new DisputeProxy
                {
                    Id = x.Key,
                    verdictName = x.OrderBy(z => z.DocumentDate).Select(z => z.CourtVerdictName).LastOrDefault(),
                    verdictsCode2Count = x.Count(z => z.CourtVerdictCode == "2"),
                    verdictsCode3Count = x.Count(z => z.CourtVerdictCode == "3")
                })
                .ToDictionary(x => x.Id);

            return disputeDict;
        }

        private Dictionary<long, DocumentGji> GetResolutionParentData()
        {
            var resolutionData = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor || x.Parent.TypeDocumentGji == TypeDocumentGji.Protocol)
                .Where(x => x.Children.DocumentDate >= this.dateStart && x.Children.DocumentDate <= this.dateEnd)
                .Select(x => new
                {
                    resolutionId = x.Children.Id,
                    x.Parent.TypeDocumentGji,
                    x.Parent.DocumentNumber,
                    x.Parent.DocumentDate
                })
                .AsEnumerable()
                .GroupBy(x => x.resolutionId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var doc = x.First();

                        return new DocumentGji
                        {
                            TypeDocumentGji = doc.TypeDocumentGji,
                            DocumentNumber = doc.DocumentNumber,
                            DocumentDate = doc.DocumentDate
                        };
                    });

            return resolutionData;
        }

        private List<ResolutionDataProxy> GetResolutions()
        {
            var servDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var servProtocolViolation = Container.Resolve<IDomainService<ProtocolViolation>>();
            var servResolProsRo = Container.Resolve<IDomainService<ResolProsRealityObject>>();
            var servResolution = Container.Resolve<IDomainService<Resolution>>();

            var addresses = servDocumentGjiChildren.GetAll()
                .Join(
                    servProtocolViolation.GetAll(),
                    a => a.Parent.Id,
                    b => b.Document.Id,
                    (a, b) => new
                    {
                        resolutionId = a.Children.Id,
                        municipalityName = b.InspectionViolation.RealityObject.Municipality.Name,
                        municipalityId = b.InspectionViolation.RealityObject.Municipality.Id,
                        b.InspectionViolation.RealityObject.Address,
                        childTypeDocument = a.Children.TypeDocumentGji,
                        childDocumentDate = a.Children.DocumentDate,
                        parentTypeDocument = a.Parent.TypeDocumentGji
                    })
               .Where(x => x.childTypeDocument == TypeDocumentGji.Resolution)
               .Where(x => x.childDocumentDate >= this.dateStart && x.childDocumentDate <= this.dateEnd)
               .Where(x => x.parentTypeDocument == TypeDocumentGji.Protocol)
               .ToList();

            addresses.AddRange(servDocumentGjiChildren.GetAll()
                .Join(
                    servResolProsRo.GetAll(),
                    a => a.Parent.Id,
                    b => b.ResolPros.Id,
                    (a, b) => new
                    {
                        resolutionId = a.Children.Id,
                        municipalityName = b.RealityObject.Municipality.Name,
                        municipalityId = b.RealityObject.Municipality.Id,
                        b.RealityObject.Address,
                        childTypeDocument = a.Children.TypeDocumentGji,
                        childDocumentDate = a.Children.DocumentDate,
                        parentTypeDocument = a.Parent.TypeDocumentGji
                    })
               .Where(x => x.childTypeDocument == TypeDocumentGji.Resolution)
               .Where(x => x.childDocumentDate >= this.dateStart && x.childDocumentDate <= this.dateEnd)
               .Where(x => x.parentTypeDocument == TypeDocumentGji.ResolutionProsecutor));

            var addressesDict = addresses
                .GroupBy(x => x.resolutionId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => string.Format("{0}, {1}", y.municipalityName, y.Address)).Distinct().Aggregate((a, b) => a + ", " + b));

            var resolutions = servResolution.GetAll()
               .Where(x => x.DocumentDate >= this.dateStart && x.DocumentDate <= this.dateEnd)
               .Select(x => new ResolutionDataProxy
               {
                   id = x.Id,
                   docDate = x.DocumentDate.Value,
                   initiator = x.TypeInitiativeOrg,
                   penaltyValue = x.PenaltyAmount,
                   physicalPerson = x.PhysicalPerson,
                   contragentName = x.Contragent.Name,
                   executantCode = x.Executant.Code,
                   sanctionCode = x.Sanction.Code,
                   sanctionName = x.Sanction.Name,
                   sspDate = x.DateTransferSsp,
                   sspDoc = x.DocumentNumSsp,
                   paided = x.Paided,
                   Address = addressesDict.ContainsKey(x.Id) ? addressesDict[x.Id] : string.Empty
               })
               .ToList();

            if (municipalityIds.Length > 0)
            {
                var resolutionsOfMunicipality = addresses
                    .GroupBy(x => x.resolutionId)
                    .Select(x => new { resolutionId = x.Key, municipalityId = x.Min(y => y.municipalityId) }) // x.Min(y => y.municipalityId) => имитация работы вьюшки
                    .Where(x => municipalityIds.Contains(x.municipalityId))
                    .Select(x => x.resolutionId)
                    .ToList();

                resolutions = resolutions.Where(x => resolutionsOfMunicipality.Contains(x.id)).ToList();
            }

            return resolutions;
        }

        private List<ProtocolResponsibilityRow> GetData()
        {
            var resolutions = this.GetResolutions();
            var articleLawsDict = this.GetArticleLaws();
            var inspectorsDict = this.GetInspectors();
            var payFineDict = this.GetPayFineData();
            var disputeDict = this.GetDisputeData();
            var resolutionParentDataDict = this.GetResolutionParentData();

            var sanctionCodes = new[] { "2", "3", "4" };
            var physicCodes = new[] { "6", "7", "14" };
            var juridicCodes = new[] { "0", "2", "4", "8", "9", "11", "15", "18" };
            var officialPerson = new[] { "1", "3", "5", "10", "12", "13", "16", "19" };


            var result = resolutions
                .Select(x =>
                {
                    var data = new ProtocolResponsibilityRow
                    {
                        Address = x.Address,
                        ReviewDate = x.docDate
                    };

                    if (resolutionParentDataDict.ContainsKey(x.id))
                    {
                        var resolutionParentData = resolutionParentDataDict[x.id];

                        if (resolutionParentData.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                        {
                            data.Inspector = "Прокуратура";
                        }
                        else if (inspectorsDict.ContainsKey(x.id))
                        {
                            data.Inspector = inspectorsDict[x.id];
                        }

                        data.ParentDocumentNumber = resolutionParentData.DocumentNumber;
                        data.PrepareDate = resolutionParentData.DocumentDate;
                    }

                    if (x.initiator == TypeInitiativeOrgGji.HousingInspection)
                    {
                        data.Authority = "Жилищная инспекция";
                        data.PenaltyZji = x.sanctionCode == "1" ? x.penaltyValue : 0;
                    }
                    else
                    {
                        data.Authority = "Мировой суд";
                        data.PenaltyCourt = x.sanctionCode == "1" ? x.penaltyValue : 0;
                    }

                    if (!string.IsNullOrEmpty(x.executantCode))
                    {
                        if (juridicCodes.Contains(x.executantCode))
                        {
                            data.StatusJur = true;
                            data.Contractor = x.contragentName;
                        }
                        else if (physicCodes.Contains(x.executantCode))
                        {
                            data.StatusFl = true;
                            data.Contractor = x.physicalPerson;
                        }
                        else if (officialPerson.Contains(x.executantCode))
                        {
                            var strList = new List<string> { x.contragentName, x.physicalPerson };
                            data.StatusDl = true;
                            data.Contractor = string.Join(", ", strList.Where(y => !string.IsNullOrEmpty(y)));
                        }
                    }

                    data.Article = articleLawsDict.ContainsKey(x.id) ? articleLawsDict[x.id] : string.Empty;
                    data.Sanction = sanctionCodes.Contains(x.sanctionCode) ? x.sanctionName : string.Empty;
                    data.SanctionCode = x.sanctionCode;

                    if (disputeDict.ContainsKey(x.id))
                    {
                        data.Dispute = disputeDict[x.id];
                    }

                    if (x.sspDate != null)
                    {
                        var sb = new StringBuilder(string.Format("{0:MM/dd/yyyy}", x.sspDate));
                        sb.Append(string.IsNullOrEmpty(x.sspDoc) ? string.Empty : string.Format(", {0}", x.sspDoc));
                        data.SspTransferInfo = sb.ToString();
                    }

                    if (payFineDict.ContainsKey(x.id))
                    {
                        var payFine = payFineDict[x.id];
                        data.docDate = payFine.docDate;
                        data.typeDoc = payFine.typeDoc;
                        if (x.initiator == TypeInitiativeOrgGji.HousingInspection)
                        {
                            data.PayFineZji = payFine.Amount;
                            data.PayFineZjiDetails = payFine.Documents;
                        }
                        else if (x.initiator == TypeInitiativeOrgGji.Court)
                        {
                            data.PayFineCourt = payFine.Amount;
                            data.PayFineCourtDetails = payFine.Documents;
                        }
                    }

                    return data;
                })
                .ToList();

            result = result
                .Where(x => !(remarked && (x.SanctionCode == "2" || x.SanctionCode == "3")))
                .Where(x => !(canceled && x.Dispute.verdictsCode2Count != 0))
                .Where(x => !(returned && x.Dispute.verdictsCode3Count != 0))
                .ToList();

            return result;
        }

        private void FillRow(Section section, ProtocolResponsibilityRow row)
        {
            section.ДобавитьСтроку();
            section["Инспектор"] = row.Inspector;
            section["НомерБланка"] = string.Empty;
            section["НомерПротокола"] = row.ParentDocumentNumber;
            section["АдресПравонарушения"] = row.Address;
            section["ДатаСоставления"] = row.PrepareDate;
            section["ДатаРассмотрения"] = row.ReviewDate;
            section["Орган"] = row.Authority;
            section["ШтрафСуд"] = row.PenaltyCourt > 0 ? row.PenaltyCourt.ToStr() : string.Empty;
            section["ШтрафГЖИ"] = row.PenaltyZji > 0 ? row.PenaltyZji.ToStr() : string.Empty;
            section["НаименованиеЮрЛица"] = row.Contractor;
            section["СтатусЮр"] = row.StatusJur ? "1" : string.Empty;
            section["СтатусФл"] = row.StatusFl ? "1" : string.Empty;
            section["СтатусДл"] = row.StatusDl ? "1" : string.Empty;
            section["СтатьяКоАП"] = row.Article;
            section["Санкция"] = row.Sanction;
            section["ДатаПостановления"] = row.docDate;
            section["Статус"] = row.typeDoc;
            section["Обжаловано"] = row.Dispute.verdictName;
            section["ДатаНаправления"] = row.SspTransferInfo;
            section["ОплатаСуд"] = row.PayFineCourt > 0 ? row.PayFineCourt.ToStr() : string.Empty;
            section["ДатаКвитанцииСуд"] = row.PayFineCourtDetails;
            section["ОплатаГЖИ"] = row.PayFineZji > 0 ? row.PayFineZji.ToStr() : string.Empty;
            section["ДатаКвитанцииГЖИ"] = row.PayFineZjiDetails;
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var innersection = section.ДобавитьСекцию("innersection");

            var sumList = new[] { "ШтрафСуд", "ШтрафГЖИ", "СтатусЮр", "СтатусДл", "СтатусФл", "ОплатаСуд", "ОплатаГЖИ" };
            var yearSum = sumList.ToDictionary(x => x, x => 0M);

            var data = this.GetData();

            foreach (var monthData in data.GroupBy(x => x.ReviewDate.Month).OrderBy(x => x.Key))
            {
                section.ДобавитьСтроку();
                section["Месяц"] = new DateTime(1, monthData.Key, 1).ToString("MMMM");
                var monthSum = sumList.ToDictionary(x => x, x => 0.ToDecimal());

                foreach (var protocolResponsibilityRow in monthData.OrderBy(x => x.ReviewDate))
                {
                    FillRow(innersection, protocolResponsibilityRow);

                    monthSum["ШтрафСуд"] += protocolResponsibilityRow.PenaltyCourt ?? 0;
                    monthSum["ШтрафГЖИ"] += protocolResponsibilityRow.PenaltyZji ?? 0;
                    monthSum["СтатусЮр"] += protocolResponsibilityRow.StatusJur ? 1 : 0;
                    monthSum["СтатусДл"] += protocolResponsibilityRow.StatusDl ? 1 : 0;
                    monthSum["СтатусФл"] += protocolResponsibilityRow.StatusFl ? 1 : 0;
                    monthSum["ОплатаСуд"] += protocolResponsibilityRow.PayFineCourt ?? 0;
                    monthSum["ОплатаГЖИ"] += protocolResponsibilityRow.PayFineZji ?? 0;
                }

                // Месячный итог
                foreach (var pair in monthSum)
                {
                    section[string.Format("МесяцИтого{0}", pair.Key)] = pair.Value;
                    yearSum[pair.Key] += pair.Value;
                }
            }

            // Годовой итог
            yearSum.ForEach(pair => reportParams.SimpleReportParams[string.Format("ГодИтого{0}", pair.Key)] = pair.Value);

            reportParams.SimpleReportParams["МесяцОкончания"] = new DateTime(1, this.dateEnd.Month, 1).ToString("MMMM");
            reportParams.SimpleReportParams["ГодОкончания"] = this.dateEnd.Year;
        }
    }

    internal sealed class PayFineProxy
    {
        public long Id { get; set; }

        public decimal? Amount { get; set; }

        public string Documents { get; set; }

        public string docDate { get; set; }

        public string typeDoc { get; set; }
    }

    internal struct DisputeProxy
    {
        public long Id { get; set; }

        public string verdictName { get; set; }

        public int verdictsCode2Count { get; set; }

        public int verdictsCode3Count { get; set; }
    }

    internal sealed class ProtocolResponsibilityRow
    {
        public string Inspector { get; set; }
        public string ParentDocumentNumber { get; set; }
        public string Address { get; set; }
        public DateTime? PrepareDate { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Authority { get; set; }
        public decimal? PenaltyCourt { get; set; }
        public decimal? PenaltyZji { get; set; }
        public string Contractor { get; set; }
        public bool StatusJur { get; set; }
        public bool StatusDl { get; set; }
        public bool StatusFl { get; set; }
        public string Article { get; set; }
        public string Sanction { get; set; }
        public string SanctionCode { get; set; }
        public DisputeProxy Dispute { get; set; }
        public string docDate { get; set; }
        public string typeDoc { get; set; }
        public string SspTransferInfo { get; set; }
        public decimal? PayFineCourt { get; set; }
        public string PayFineCourtDetails { get; set; }
        public decimal? PayFineZji { get; set; }
        public string PayFineZjiDetails { get; set; }
    }

    internal sealed class ResolutionDataProxy
    {
        public long id { get; set; }
        public DateTime docDate { get; set; }
        public TypeInitiativeOrgGji initiator { get; set; }
        public decimal? penaltyValue { get; set; }
        public string physicalPerson { get; set; }
        public string contragentName { get; set; }
        public string executantCode { get; set; }
        public string sanctionCode { get; set; }
        public string sanctionName { get; set; }
        public DateTime? sspDate { get; set; }
        public string sspDoc { get; set; }
        public YesNoNotSet paided { get; set; }
        public string Address { get; set; }
    }
}