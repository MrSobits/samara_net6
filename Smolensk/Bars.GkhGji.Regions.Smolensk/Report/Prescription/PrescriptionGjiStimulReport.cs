namespace Bars.GkhGji.Regions.Smolensk.Report.Prescription
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    using Gkh.Report;
    using Slepov.Russian.Morpher;

    public class PrescriptionGjiStimulReport : GjiBaseStimulReport
    {
        #region .ctor
        public PrescriptionGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.Prescription))
        {
        }
        #endregion

        #region Properties
        public override string Name
        {
            get { return "Предписание"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override string Description
        {
            get { return "Предписание"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "Prescription"; }
        }

        public override string CodeForm
        {
            get { return "Prescription"; }
        }
        #endregion

        #region Fields
        private long DocumentId { get; set; }
        private Prescription prescription;
        private DocumentGji parentDisposal;
        private const string typeSurveyCodeGku = "23"; // Корд типа обследования для ЖКУ
        private const string typeSurveyCodeDi = "22";  // Код типа обследвоания для Раскрытия информации
        #endregion Fields

        #region Injections
        public IDomainService<Prescription> PrescriptionService { get; set; }
        public IDomainService<PrescriptionViol> PrescriptionViolService { get; set; }
        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorService { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenService { get; set; }
        public IDomainService<PrescriptionViolDescription> PrescrViolDescrDomain { get; set; }
        public IDomainService<DisposalTypeSurvey> DisposalTypeSurveyDomain { get; set; }
        public IDomainService<ContragentContact> ContragentContactDomain { get; set; }
        #endregion

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Name = "PrescriptionGJI",
                                   Code = "PrescriptionGJI",
                                   Description = "Предписание за ЖКУ (типа обследования с кодом 23)",
                                   Template = Properties.Resources.Prescription
                               },
                            new TemplateInfo
                               {
                                   Name = "PrescriptionGJIDisclusionInfo",
                                   Code = "PrescriptionGJIDisclusionInfo",
                                   Description = "Предписание раскрытие информации (типа обследования с кодом 22)",
                                   Template = Properties.Resources.Prescription
                               },
                            new TemplateInfo
                               {
                                   Name = "PrescriptionGJIOther",
                                   Code = "PrescriptionGJIOther",
                                   Description = "Предписание все остальные случаи",
                                   Template = Properties.Resources.Prescription
                               }
                       };
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            prescription = PrescriptionService.GetAll().FirstOrDefault(x => x.Id == DocumentId);

            if (prescription == null)
            {
                throw new ReportProviderException("Не удалось получить предписание");
            }

            //оулчаем родительский приказ
            parentDisposal = this.GetParentDocument(prescription, TypeDocumentGji.Disposal);
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        private void GetCodeTemplate()
        {
            // по умолчанию будет код для шаблона "Предписания для всех остальных"
            CodeTemplate = "PrescriptionGJIOther";
            var typeSurveyCodes = new List<string>();

            if (this.parentDisposal != null)
            {
                // получаем только коды котоыре нам необходимы
                // при этом необходим порядок добавления этих кодов потмоучто кто из них раньше добавился 
                // тот шаблон и будет распечатан
                typeSurveyCodes = DisposalTypeSurveyDomain.GetAll()
                                            .Where(x => x.Disposal.Id == parentDisposal.Id)
                                            .Where(
                                                x =>
                                                x.TypeSurvey.Code == typeSurveyCodeGku
                                                || x.TypeSurvey.Code == typeSurveyCodeDi)
                                            .OrderBy(x => x.ObjectCreateDate)
                                            .Select(x => x.TypeSurvey.Code)
                                            .ToList();
            }

            if (typeSurveyCodes.Any())
            {
                var firstCode = typeSurveyCodes.First();

                switch (firstCode)
                {
                    case typeSurveyCodeGku:
                        {
                            CodeTemplate = "PrescriptionGJI";
                        }
                        break;
                    case typeSurveyCodeDi:
                        {
                            CodeTemplate = "PrescriptionGJIDisclusionInfo";
                        }
                        break;
                }
            }

        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            FillCommonFields(prescription);

            this.ReportParams["ДатаПредписания"] = prescription.DocumentDate.HasValue
                                                                     ? prescription.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                                                                     : string.Empty;
            this.ReportParams["НомерПредписания"] = prescription.DocumentNumber ?? string.Empty;
            var realObj =
                PrescriptionViolService.GetAll()
                    .Where(x => x.Document.Id == DocumentId
                        && x.InspectionViolation.RealityObject != null)
                    .Select(x => x.InspectionViolation.RealityObject)
                    .FirstOrDefault();
            this.ReportParams["МестоСоставления"] = realObj != null ? realObj.FiasAddress.PlaceName : string.Empty;

            this.ReportParams["НомерАктаПроверки"] = string.Empty;
            this.ReportParams["ИнспекторФамИО"] = string.Empty;
            this.ReportParams["ДолжностьИнспектора"] = string.Empty;
            this.ReportParams["ИнспекторИДолжность"] = string.Empty;
            this.ReportParams["ИнспекторИДолжность"] = DocumentGjiInspectorService.GetAll()
                                         .Where(x => x.DocumentGji.Id == DocumentId)
                                         .Select(x => new
                                         {
                                             InspAndPosit = x.Inspector.ShortFio + " - " + x.Inspector.Position
                                         })
                                         .ToList()
                                         .AggregateWithSeparator(x => x.InspAndPosit, ", ");

            var actDoc = this.GetParentDocument(prescription, TypeDocumentGji.ActCheck);
            if (actDoc != null)
            {
                this.ReportParams["НомерАктаПроверки"] = string.Format(
                    "№{0} от {1}", actDoc.DocumentNumber,
                    actDoc.DocumentDate.HasValue ? actDoc.DocumentDate.Value.ToShortDateString() : string.Empty);

                this.ReportParams["ДатаАктаПроверки"] = actDoc.DocumentDate.HasValue
                                                                     ? actDoc.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                                                                     : string.Empty;
            }

            if (prescription.Executant != null)
            {
                this.ReportParams["АдресКонтрагента"] = string.Empty;
                var contragent = prescription.Contragent;
                var contragentName = string.Empty;
                if (contragent != null)
                {
                    var contragentNameAll = склонятель.Проанализировать(prescription.Contragent.Name);
                    contragentName = contragentNameAll.Дательный;
                }
                this.ReportParams["ТипИсполнителя"] = prescription.Executant.Code;

                switch (prescription.Executant.Code)
                {
                    case "0":
                    case "2":
                    case "9":
                    case "11":
                    case "8":
                    case "15":
                    case "18":
                    case "4":
                    case "17":
                        this.ReportParams["КомуВыдано"] = contragent != null
                            ? string.Format("{0} ({1})", contragentName, contragent.ShortName)
                            : string.Empty;

                        if (contragent != null)
                        {
                            if (contragent.FiasJuridicalAddress != null)
                            {
                                var subStr = contragent.FiasJuridicalAddress.AddressName.Split(',');

                                var newAddr = new StringBuilder();

                                foreach (var rec in subStr)
                                {
                                    if (newAddr.Length > 0)
                                    {
                                        newAddr.Append(',');
                                    }

                                    if (rec.Contains("р-н."))
                                    {
                                        var mu = rec.Replace("р-н.", string.Empty) + " район";
                                        newAddr.Append(mu);
                                        continue;
                                    }

                                    newAddr.Append(rec);
                                }

                                this.ReportParams["АдресКонтрагента"] = newAddr.ToStr();
                            }
                            else
                            {
                                this.ReportParams["АдресКонтрагента"] = contragent.JuridicalAddress;
                            }
                        }
                        break;
                    case "1":
                    case "3":
                    case "10":
                    case "12":
                    case "13":
                    case "16":
                    case "19":
                    case "5":
                        {
                            var result = contragent != null
                                ? string.Format("{0} ({1})", contragentName, contragent.ShortName)
                                : string.Empty;

                            if (!string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(prescription.PhysicalPerson))
                                result += ", ";

                            result += склонятель.Проанализировать(prescription.PhysicalPerson).Дательный;

                            this.ReportParams["КомуВыдано"] = result;

                            if (contragent != null)
                            {
                                if (contragent.FiasJuridicalAddress != null)
                                {
                                    var subStr = contragent.FiasJuridicalAddress.AddressName.Split(',');

                                    var newAddr = new StringBuilder();

                                    foreach (var rec in subStr)
                                    {
                                        if (newAddr.Length > 0)
                                        {
                                            newAddr.Append(',');
                                        }

                                        if (rec.Contains("р-н."))
                                        {
                                            var mu = rec.Replace("р-н.", string.Empty) + " район";
                                            newAddr.Append(mu);
                                            continue;
                                        }

                                        newAddr.Append(rec);
                                    }

                                    this.ReportParams["АдресКонтрагента"] = newAddr.ToStr();
                                }
                                else
                                {
                                    this.ReportParams["АдресКонтрагента"] = contragent.JuridicalAddress;
                                }
                            }
                        }

                        break;
                    case "6":
                    case "7":
                    case "14":
                    case "20":
                        this.ReportParams["КомуВыдано"] = склонятель.Проанализировать(prescription.PhysicalPerson).Дательный;
                        this.ReportParams["АдресКонтрагента"] = prescription.PhysicalPersonInfo;
                        break;
                }
            }

            if (prescription.Contragent != null)
            {
                var mainContact =
                    this.ContragentContactDomain.GetAll()
                                           .Where(x => x.Contragent.Id == this.prescription.Contragent.Id)
                                           .FirstOrDefault(x => x.Position != null && x.Position.Code == "1");

                if (mainContact != null)
                {
                    this.ReportParams["ФиоРуОрг"] =
                        string.Format("{0} {1} {2}", mainContact.Surname, mainContact.Name, mainContact.Patronymic).Trim();
                }

                this.ReportParams["ИНН"] = prescription.Contragent.Inn;
            }

            this.ReportParams["РеквезитыФизЛица"] = prescription.PhysicalPersonInfo;


            var violations = PrescriptionViolService.GetAll()
                .Where(x => x.Document.Id == prescription.Id)
                .Select(x => new
                    {
                        x.Id,
                        ViolationId = x.InspectionViolation.Violation.Id,
                        ViolationCodePin = x.InspectionViolation.Violation.CodePin,
                        ViolationName = x.InspectionViolation.Violation.Name,
                        x.Action,
                        x.Description,
                        x.InspectionViolation.DatePlanRemoval,
                        x.InspectionViolation.Violation.PpRf170,
                        x.InspectionViolation.Violation.PpRf25,
                        x.InspectionViolation.Violation.PpRf307,
                        x.InspectionViolation.Violation.PpRf491,
                        x.InspectionViolation.Violation.OtherNormativeDocs,
                        Address = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Address : null
                    })
                .AsEnumerable()
                .Distinct()
                .ToList();

            this.ReportParams["АдресПравонарушения"] = violations.Where(x => x.Address != null).Select(x => x.Address).FirstOrDefault();
            RegViolationsDataSource();
        }

        private void RegViolationsDataSource()
        {
            var violPiontDomain = Container.Resolve<IDomainService<DocumentViolGroupPoint>>();
            var domainService = Container.Resolve<IDomainService<DocumentViolGroup>>();
            var longTextDomain = Container.Resolve<IDomainService<DocumentViolGroupLongText>>();

            using (Container.Using(violPiontDomain, domainService, longTextDomain))
            {

                var query = domainService.GetAll()
                    .Where(x => x.Document.Id == DocumentId);

                var longTexts = longTextDomain.GetAll()
                    .Where(x => x.ViolGroup.Document.Id == DocumentId).ToArray()
                    .GroupBy(x => x.ViolGroup.Id).ToDictionary(x => x.Key, x => x.Select(y => new
                    {
                        y.Description,
                        y.Action
                    }).First());

                var violPoints =
                    violPiontDomain.GetAll()
                        .Where(x => query.Any(y => y.Id == x.ViolGroup.Id))
                        .Select(
                            x =>
                                new
                                {
                                    x.ViolStage.InspectionViolation.Violation.CodePin,
                                    ViolStageId = x.ViolStage.Id,
                                    violGroupId = x.ViolGroup.Id
                                })
                        .AsEnumerable()
                        .GroupBy(x => x.violGroupId)
                        .ToDictionary(
                            x => x.Key,
                            y => new
                            {
                                PointCodes = y.Select(z => z.CodePin)
                                    .Aggregate((str, result) => !string.IsNullOrEmpty(result) ? result + "," + str : str)
                            });

                var data = query
                    .Select(x => new
                    {
                        x.Id,
                        x.DatePlanRemoval,
                        x.Description,
                        x.Action
                    })
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        x.DatePlanRemoval,
                        x.Description,
                        x.Action,
                        PointCodes = violPoints.ContainsKey(x.Id) ? violPoints[x.Id].PointCodes : null
                    });

                var viols = data.Select(viol => new PrescriptionViolRecord
                {
                    Наименование =
                        longTexts.ContainsKey(viol.Id) && longTexts[viol.Id].Description != null
                            ? Encoding.UTF8.GetString(longTexts[viol.Id].Description).ToString(CultureInfo.GetCultureInfo("ru-RU"))
                            : viol.Description,
                    Мероприятия =
                        longTexts.ContainsKey(viol.Id) && longTexts[viol.Id].Action != null
                            ? Encoding.UTF8.GetString(longTexts[viol.Id].Action).ToString(CultureInfo.GetCultureInfo("ru-RU"))
                            : viol.Action,
                    СрокИсполнения =
                        viol.DatePlanRemoval.HasValue ? viol.DatePlanRemoval.Value.ToShortDateString() : string.Empty,
                    Пункт = viol.PointCodes
                });
                this.DataSources.Add(new MetaData
                {
                    SourceName = "Нарушения",
                    MetaType = nameof(PrescriptionViolRecord),
                    Data = viols
                });
            }
        }

        protected class PrescriptionViolRecord
        {
            public int НомерПп { get; set; }
            public string Наименование { get; set; }
            public string Подробнее { get; set; }
            public string Мероприятия { get; set; }
            public string СрокИсполнения { get; set; }
            public string Пункт { get; set; }
        }

        public DocumentGji GetParentDocument(DocumentGji document, TypeDocumentGji type)
        {
            var result = document;

            if (document.TypeDocumentGji != type)
            {
                var docs = DocumentGjiChildrenService.GetAll()
                                    .Where(x => x.Children.Id == document.Id)
                                    .Select(x => x.Parent)
                                    .ToList();

                foreach (var doc in docs)
                {
                    result = GetParentDocument(doc, type);
                }
            }

            if (result != null)
            {
                return result.TypeDocumentGji == type ? result : null;
            }

            return null;
        }
    }
}