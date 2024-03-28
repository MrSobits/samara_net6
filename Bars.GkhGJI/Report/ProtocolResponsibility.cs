namespace Bars.GkhGji.Report
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ProtocolResponsibility : BasePrintForm
    {
        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;

        private long[] municipalityIds;
        private bool canceled;
        private bool returned;
        private bool remarked;

        public ProtocolResponsibility()
            : base(new ReportTemplateBinary(Properties.Resources.ProtocolResponsibility))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.GJI.ProtocolResponsibility"; }
        }

        public override string Name
        {
            get { return "Отчет по протоколам"; }
        }

        public override string Desciption
        {
            get { return "Отчет по протоколам"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ProtocolResponsibility"; }
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

        public override string ReportGenerator { get; set; }

        public Dictionary<long, string> GetArticleLaws()
        {
            var servDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var servProtocolArticleLaw = Container.Resolve<IDomainService<ProtocolArticleLaw>>();
            var servProtocolMvdArticleLaw = Container.Resolve<IDomainService<ProtocolMvdArticleLaw>>();
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
               .Where(x => x.parentTypeDocument == TypeDocumentGji.Protocol
                   || x.parentTypeDocument == TypeDocumentGji.ProtocolMvd)
               .ToList();

            articleLaws.AddRange(servDocumentGjiChildren.GetAll()
                .Join(
                    servProtocolMvdArticleLaw.GetAll(),
                    a => a.Parent.Id,
                    b => b.ProtocolMvd.Id,
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
               .Where(x => x.parentTypeDocument == TypeDocumentGji.Protocol
                   || x.parentTypeDocument == TypeDocumentGji.ProtocolMvd)
               .ToList());

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
               .Where(x => x.parentTypeDocument == TypeDocumentGji.ResolutionProsecutor
                   || x.parentTypeDocument == TypeDocumentGji.ProtocolMvd));

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
               .Where(x => x.parentTypeDocument == TypeDocumentGji.Protocol
                   || x.parentTypeDocument == TypeDocumentGji.ProtocolMvd)
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
                  x.DocumentDate
              })
              .AsEnumerable()
              .GroupBy(x => x.Id)
              .Select(x => new PayFineProxy
              {
                  Id = x.Key,
                  Amount = x.Sum(y => y.Amount),
                  Documents = x.Select(y => string.Format("{0} №{1}", y.DocumentDate.HasValue ? y.DocumentDate.Value.ToShortDateString() : string.Empty, y.DocumentNum)).Aggregate((curr, next) => string.Format("{0}, {1}", curr, next))
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
                .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor
                    || x.Parent.TypeDocumentGji == TypeDocumentGji.Protocol
                    || x.Parent.TypeDocumentGji == TypeDocumentGji.ProtocolMvd)
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
            var servProtocolMvdRo = Container.Resolve<IDomainService<ProtocolMvdRealityObject>>();

            var addresses = servDocumentGjiChildren.GetAll()
                .Join(
                    servProtocolViolation.GetAll(),
                    a => a.Parent.Id,
                    b => b.Document.Id,
                    (a, b) => new
                    {
                        resolutionId = a.Children.Id,
                        realObj = b.InspectionViolation.RealityObject,
                        municipalityName = b.InspectionViolation.RealityObject.Municipality.Name,
                        municipalityId = b.InspectionViolation.RealityObject.Municipality.Id,
                        b.InspectionViolation.RealityObject.Address,
                        childTypeDocument = a.Children.TypeDocumentGji,
                        childDocumentDate = a.Children.DocumentDate,
                        parentTypeDocument = a.Parent.TypeDocumentGji
                    })
               .Where(x => x.realObj != null)
               .Where(x => x.childTypeDocument == TypeDocumentGji.Resolution)
               .Where(x => x.childDocumentDate >= this.dateStart && x.childDocumentDate <= this.dateEnd)
               .Where(x => x.parentTypeDocument == TypeDocumentGji.Protocol
                   || x.parentTypeDocument == TypeDocumentGji.ProtocolMvd)
               .ToList();

            addresses.AddRange(servDocumentGjiChildren.GetAll()
                .Join(
                    servResolProsRo.GetAll(),
                    a => a.Parent.Id,
                    b => b.ResolPros.Id,
                    (a, b) => new
                    {
                        resolutionId = a.Children.Id,
                        realObj = b.RealityObject,
                        municipalityName = b.RealityObject.Municipality.Name,
                        municipalityId = b.RealityObject.Municipality.Id,
                        b.RealityObject.Address,
                        childTypeDocument = a.Children.TypeDocumentGji,
                        childDocumentDate = a.Children.DocumentDate,
                        parentTypeDocument = a.Parent.TypeDocumentGji
                    })
               .Where(x => x.realObj != null)
               .Where(x => x.childTypeDocument == TypeDocumentGji.Resolution)
               .Where(x => x.childDocumentDate >= this.dateStart && x.childDocumentDate <= this.dateEnd)
               .Where(x => x.parentTypeDocument == TypeDocumentGji.ResolutionProsecutor
                   || x.parentTypeDocument == TypeDocumentGji.ProtocolMvd));

            addresses.AddRange(servDocumentGjiChildren.GetAll()
                .Join(
                    servProtocolMvdRo.GetAll(),
                    a => a.Parent.Id,
                    b => b.ProtocolMvd.Id,
                    (a, b) => new
                    {
                        resolutionId = a.Children.Id,
                        realObj = b.RealityObject,
                        municipalityName = b.RealityObject.Municipality.Name,
                        municipalityId = b.RealityObject.Municipality.Id,
                        b.RealityObject.Address,
                        childTypeDocument = a.Children.TypeDocumentGji,
                        childDocumentDate = a.Children.DocumentDate,
                        parentTypeDocument = a.Parent.TypeDocumentGji
                    })
               .Where(x => x.realObj != null)
               .Where(x => x.childTypeDocument == TypeDocumentGji.Resolution)
               .Where(x => x.childDocumentDate >= this.dateStart && x.childDocumentDate <= this.dateEnd)
               .Where(x => x.parentTypeDocument == TypeDocumentGji.ResolutionProsecutor
                   || x.parentTypeDocument == TypeDocumentGji.ProtocolMvd));

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
                   DocDate = x.DocumentDate.Value,
                   Initiator = x.TypeInitiativeOrg,
                   PenaltyValue = x.PenaltyAmount,
                   PhysicalPerson = x.PhysicalPerson,
                   ContragentName = x.Contragent.Name,
                   ExecutantCode = x.Executant.Code,
                   SanctionCode = x.Sanction.Code,
                   SanctionName = x.Sanction.Name,
                   SspDate = x.DateTransferSsp,
                   SspDoc = x.DocumentNumSsp,
                   Paided = x.Paided,
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
                            ReviewDate = x.DocDate
                        };

                        if (resolutionParentDataDict.ContainsKey(x.id))
                        {
                            var resolutionParentData = resolutionParentDataDict[x.id];

                            if (resolutionParentData.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                            {
                                data.Inspector = "Прокуратура";
                            }
                            else if (resolutionParentData.TypeDocumentGji == TypeDocumentGji.ProtocolMvd)
                            {
                                data.Inspector = "Протокол МВД";
                            }
                            else if (inspectorsDict.ContainsKey(x.id))
                            {
                                data.Inspector = inspectorsDict[x.id];
                            }

                            data.ParentDocumentNumber = resolutionParentData.DocumentNumber;
                            data.PrepareDate = resolutionParentData.DocumentDate;
                        }

                        if (x.Initiator == TypeInitiativeOrgGji.HousingInspection)
                        {
                            data.Authority = "Жилищная инспекция";
                            data.PenaltyZji = x.SanctionCode == "1" ? x.PenaltyValue : 0;
                        }
                        else
                        {
                            data.Authority = "Мировой суд";
                            data.PenaltyCourt = x.SanctionCode == "1" ? x.PenaltyValue : 0;
                        }

                        if (!string.IsNullOrEmpty(x.ExecutantCode))
                        {
                            if (juridicCodes.Contains(x.ExecutantCode))
                            {
                                data.StatusJur = true;
                                data.Contractor = x.ContragentName;
                            }
                            else if (physicCodes.Contains(x.ExecutantCode))
                            {
                                data.StatusFl = true;
                                data.Contractor = x.PhysicalPerson;
                            }
                            else if (officialPerson.Contains(x.ExecutantCode))
                            {
                                var strList = new List<string> { x.ContragentName, x.PhysicalPerson };
                                data.StatusDl = true;
                                data.Contractor = string.Join(", ", strList.Where(y => !string.IsNullOrEmpty(y)));
                            }
                        }

                        data.Article = articleLawsDict.ContainsKey(x.id) ? articleLawsDict[x.id] : string.Empty;
                        data.Sanction = sanctionCodes.Contains(x.SanctionCode) ? x.SanctionName : string.Empty;
                        data.SanctionCode = x.SanctionCode;

                        if (disputeDict.ContainsKey(x.id))
                        {
                            data.Dispute = disputeDict[x.id];
                        }

                        if (x.SspDate != null)
                        {
                            var sb = new StringBuilder(string.Format("{0:MM/dd/yyyy}", x.SspDate));
                            sb.Append(string.IsNullOrEmpty(x.SspDoc) ? string.Empty : string.Format(", {0}", x.SspDoc));
                            data.SspTransferInfo = sb.ToString();
                        }

                        if (payFineDict.ContainsKey(x.id))
                        {
                            var payFine = payFineDict[x.id];

                            if (x.Initiator == TypeInitiativeOrgGji.HousingInspection)
                            {
                                data.PayFineZji = payFine.Amount;
                                data.PayFineZjiDetails = payFine.Documents;
                            }
                            else if (x.Initiator == TypeInitiativeOrgGji.Court)
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
        public string SspTransferInfo { get; set; }
        public decimal? PayFineCourt { get; set; }
        public string PayFineCourtDetails { get; set; }
        public decimal? PayFineZji { get; set; }
        public string PayFineZjiDetails { get; set; }
    }

    internal sealed class ResolutionDataProxy
    {
        public long id { get; set; }
        public DateTime DocDate { get; set; }
        public TypeInitiativeOrgGji Initiator { get; set; }
        public decimal? PenaltyValue { get; set; }
        public string PhysicalPerson { get; set; }
        public string ContragentName { get; set; }
        public string ExecutantCode { get; set; }
        public string SanctionCode { get; set; }
        public string SanctionName { get; set; }
        public DateTime? SspDate { get; set; }
        public string SspDoc { get; set; }
        public YesNoNotSet Paided { get; set; }
        public string Address { get; set; }
        public string SanctionsDuration { get; set; }
    }
}