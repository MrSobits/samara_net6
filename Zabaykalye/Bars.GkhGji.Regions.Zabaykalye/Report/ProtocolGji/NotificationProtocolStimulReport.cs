﻿namespace Bars.GkhGji.Regions.Zabaykalye.Report.ProtocolGji
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;
    using GkhGji.Report;
    using Slepov.Russian.Morpher;

    /// <summary> Уведомление о составлении протокола </summary>
    public class NotificationProtocolStimulReport : GjiBaseStimulReport
    {
        #region .ctor

        public NotificationProtocolStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.NotificationProtocolStimulReport))
        {
        }

        #endregion .ctor

        #region Properties

        public override string Id
        {
            get { return "ProtocolNotification"; }
        }

        public override string Name
        {
            get { return "Уведомление о рассмотрении дела"; }
        }

        public override string Description
        {
            get { return "Уведомление о рассмотрении дела"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string CodeForm
        {
            get { return "Protocol"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        #endregion Properties

        #region Injections
        public IDomainService<ProtocolDefinition> protocolDefinitionDomain { get; set; }
        public IDomainService<ProtocolArticleLaw> protArtLawDomain { get; set; }
        public IDomainService<DocumentGjiInspector> docInspectorDomain { get; set; }
        public IDomainService<DocumentGJIPhysPersonInfo> physPersonInfoDomain { get; set; }
        public IDomainService<ProtocolViolation> protViolDomain { get; set; }
        public IDomainService<ContragentContact> contragentContactDomain { get; set; }
        public IProtocolService protocolService { get; set; }
        #endregion

        protected long DocumentId;

        protected Protocol Protocol;

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            var protocolDomain = Container.ResolveDomain<Protocol>();

            using (Container.Using(protocolDomain))
            {
                Protocol = protocolDomain.FirstOrDefault(x => x.Id == DocumentId);
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "NotificationProtocolStimulReport",
                    Name = "NotificationProtocolStimulReport",
                    Description = "Уведомление о рассмотрении дела",
                    Template = Properties.Resources.NotificationProtocolStimulReport
                }
            };
        }

        private void GetCodeTemplate()
        {
            CodeTemplate = "NotificationProtocolStimulReport";
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            if (Protocol == null)
            {
                return;
            }
            FillCommonFields(Protocol);

            var inspector = docInspectorDomain.GetAll()
                         .Where(x => x.DocumentGji.Id == Protocol.Id)
                         .OrderBy(x => x.Id)
                         .Select(x => new { x.Inspector.Fio, x.Inspector.Position })
                         .FirstOrDefault();

            if (inspector != null)
            {
                this.ReportParams["РуководительФИОСокр"] = inspector.Fio;
                this.ReportParams["ДолжностьРуководителя"] = inspector.Position;
            }

            var articles = protArtLawDomain.GetAll()
                    .Where(x => x.Protocol.Id == Protocol.Id)
                    .Select(x => new { x.ArticleLaw.Name, x.Description, ArtDescription = x.ArticleLaw.Description })
                    .ToList();

            if (articles.Any())
            {
                var artStr = articles.Aggregate(string.Empty, (x, y) => x + (y.Name + ", "));

                this.ReportParams["СтатьяЗакона"] = !string.IsNullOrEmpty(artStr)
                                            ? artStr.TrimEnd(new[] { ',', ' ' })
                                            : string.Empty;

                var descriptions = articles.Select(x => !string.IsNullOrEmpty(x.Description) ? x.Description : x.ArtDescription).Distinct().Aggregate(string.Empty, (x, y) => x + (y + ", "));

                this.ReportParams["ОписаниеСтатьи"] = !string.IsNullOrEmpty(descriptions)
                                            ? descriptions.TrimEnd(new[] { ',', ' ' })
                                            : string.Empty;
            }

            var roAddress =
                protViolDomain.GetAll()
                              .Where(x => x.Document.Id == Protocol.Id && x.InspectionViolation.RealityObject != null)
                              .Select(x => x.InspectionViolation.RealityObject.Address)
                              .Distinct()
                              .ToList();

            if (roAddress.Count > 0)
            {
                var str = roAddress.Aggregate(string.Empty, (x, y) => x + (y + ", "));
                this.ReportParams["АдресПравонарушения"] = !string.IsNullOrEmpty(str)
                                            ? str.TrimEnd(new[] { ',', ' ' })
                                            : string.Empty;
            }

            var physPerson = physPersonInfoDomain.GetAll().FirstOrDefault(x => x.Document.Id == Protocol.Id);
            
            if (Protocol.Executant != null)
            {
                this.ReportParams["ТипИсполнителя"] = Protocol.Executant.Code;

                var contrName = string.Empty;
                var shortName = string.Empty;
                var inn = string.Empty;
                var kpp = string.Empty;
                var jurAddress = string.Empty;
                var physPersonFio = Protocol.PhysicalPerson;
                var physPersonAddress = string.Empty;
                var physPersonPosition = string.Empty;
                var physPersonDoc = string.Empty;

                if (physPerson != null)
                {
                    physPersonAddress = physPerson.PhysPersonAddress;
                    physPersonPosition = physPerson.PhysPersonPosition;
                    physPersonDoc = physPerson.PhysPersonDocument;
                }

                if (!string.IsNullOrEmpty(physPersonFio))
                {
                    var fio = склонятель.Проанализировать(physPersonFio);
                    this.ReportParams["ФизЛицоФИО"] = physPersonFio;
                    this.ReportParams["ФизЛицоФИОРП"] = fio.Родительный;
                    this.ReportParams["ФизЛицоФИОДП"] = fio.Дательный;
                }

                if (!string.IsNullOrEmpty(physPersonPosition))
                {
                    var position = склонятель.Проанализировать(physPersonPosition);
                    this.ReportParams["ФизЛицоДолжность"] = physPersonPosition;
                    this.ReportParams["ФизЛицоДолжностьРП"] = position.Родительный;
                    this.ReportParams["ФизЛицоДолжностьДП"] = position.Дательный;
                }

                this.ReportParams["ФизЛицоАдрес"] = physPersonAddress;
                this.ReportParams["ФизЛицоДУЛ"] = physPersonDoc;

                var contragent = Protocol.Contragent;
                if (contragent != null)
                {
                    contrName = contragent.Return(x => x.Name);
                    shortName = contragent.Return(x => x.ShortName);
                    inn = contragent.Return(x => x.Inn);
                    kpp = contragent.Return(x => x.Kpp);
                    jurAddress = contragent.FiasJuridicalAddress != null ?
                        contragent.FiasJuridicalAddress.AddressName : contragent.JuridicalAddress;

                    this.ReportParams["Контрагент"] = contrName;
                    this.ReportParams["КонтрагентСокр"] = shortName;
                    this.ReportParams["КонтрагентИНН"] = inn;
                    this.ReportParams["КонтрагентКПП"] = kpp;
                    this.ReportParams["КонтрагентЮрАдрес"] = jurAddress;

                    var contact = contragentContactDomain.GetAll()
                                               .Where(x => x.Contragent.Id == contragent.Id && x.Position != null)
                                               .FirstOrDefault();

                    if (contact != null && contact.Position != null && !string.IsNullOrEmpty(contact.Position.Name))
                    {
                        var contactPosition = склонятель.Проанализировать(contact.Position.Name);
                        this.ReportParams["КонтрагентКонтактДолжность"] = contact.Position.Name;
                        this.ReportParams["КонтрагентКонтактДолжностьРП"] = !string.IsNullOrEmpty(contact.Position.NameGenitive) ? contact.Position.NameGenitive : contactPosition.Родительный;
                        this.ReportParams["КонтрагентКонтактДолжностьДП"] = !string.IsNullOrEmpty(contact.Position.NameDative) ? contact.Position.NameDative : contactPosition.Дательный;
                    }
                }
            }

            var getInfo = protocolService.GetInfo(Protocol.Id);

            if (getInfo != null && getInfo.Data is ProtocolGetInfoProxy)
            {
                this.ReportParams["Основание"] = (getInfo.Data as ProtocolGetInfoProxy).BaseName;
            }

            var parentPrescription = this.GetParentDocument(Protocol, TypeDocumentGji.Prescription);

            if (parentPrescription != null)
            {
                this.ReportParams["НомерПредписания"] = parentPrescription.DocumentNumber;
                this.ReportParams["ДатаПредписания"] = parentPrescription.DocumentDate.HasValue ? parentPrescription.DocumentDate.Value.ToShortDateString(): string.Empty;
            }
         
            this.ReportParams["ДатаПротокола"] = Protocol.DocumentDate.ToString();
            this.ReportParams["НомерПротокола"] = Protocol.DocumentNumber;
            this.ReportParams["ОснованиеПроверки"] = Protocol.Inspection != null
                                             ? Convert.ChangeType(
                                                 Protocol.Inspection.TypeBase,
                                                 Protocol.Inspection.TypeBase.GetTypeCode()).ToStr()
                                             : "";
        }

        public DocumentGji GetParentDocument(DocumentGji document, TypeDocumentGji type)
        {
            var docChildrenDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            try
            {
                var result = document;

                if (document.TypeDocumentGji != type)
                {
                    var docs = docChildrenDomain.GetAll()
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
            finally 
            {
                Container.Release(docChildrenDomain);
            }
        }
    }
}
