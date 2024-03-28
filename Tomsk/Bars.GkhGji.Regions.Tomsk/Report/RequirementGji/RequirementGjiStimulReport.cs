using System.Globalization;
using Bars.B4.DataAccess;
using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;

namespace Bars.GkhGji.Regions.Tomsk.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using B4;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.Entities;
    using GkhGji.Enums;
    using Entities;
    using Enums;


    using TypeDocumentGji = GkhGji.Enums.TypeDocumentGji;

    /// <summary>
    /// Печатная форма требования
    /// </summary>
    public class RequirementGjiStimulReport : GkhBaseStimulReport
    {
        private long documentId { get; set; }

        private Requirement requirement;

        private IEnumerable<RequirementArticleLaw> articleLaws;

        /// <summary>
        /// Конструктор
        /// </summary>
        public RequirementGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.Requirement))
        {
        }

        /// <summary>
        /// Формат файла
        /// </summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        /// <summary>
        /// Идентификатор 
        /// </summary>
        public override string Id
        {
            get { return "RequirementGji"; }
        }
        /// <summary>
        /// Код формы
        /// </summary>
        public override string CodeForm
        {
            get { return "RequirementGji"; }
        }

        /// <summary>
        /// Название
        /// </summary>
        public override string Name
        {
            get { return "Требование"; }
        }

        /// <summary>
        /// Комментарий
        /// </summary>
        public override string Description
        {
            get { return "Требование"; }
        }

        /// <summary>
        /// Пользовательские параметры для формирование печатки 
        /// </summary>
        /// <param name="userParamsValues">Пользовательские параметры</param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.documentId = userParamsValues.GetValue<object>("documentId").ToLong();

            // Эти параметры заполняем здесь поскольку в разных методах они нужны
            var requirementDomain = this.Container.Resolve<IDomainService<Requirement>>();
            var requirementArticleLawDomain = this.Container.Resolve<IDomainService<RequirementArticleLaw>>();

            try
            {
                this.requirement = requirementDomain.GetAll()
                    .FirstOrDefault(x => x.Id == this.documentId);

                this.articleLaws = requirementArticleLawDomain.GetAll()
                                                   .Where(x => x.Requirement.Id == this.documentId)
                                                   .AsEnumerable();
            }
            finally
            {
                this.Container.Release(requirementDomain);
                this.Container.Release(requirementArticleLawDomain);
            }
        }

        /// <summary>
        /// Код шаблона
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Параметры для вывода описания о габлонах отчета
        /// </summary>
        /// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "TomskBlockGJI_Requirement",
                            Name = "RequirementGji",
                            Description = "Требование на предоставление информации",
                            Template = Properties.Resources.Requirement
                        }
                };
        }
        /// <summary>
        /// Заполнение печатной формы 
        /// </summary>
        /// <param name="reportParams">Данные для заполнение все параметров отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var requirementDomain = this.Container.Resolve<IDomainService<Requirement>>();
            var documentGjiInspectorDomain = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var requirementArticleLawDomain = this.Container.Resolve<IDomainService<RequirementArticleLaw>>();
            var inspectionGjiRealityObjectDomain = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var inspectionAppealCitsDomain = this.Container.Resolve<IDomainService<InspectionAppealCits>>();
            var disposalDomain = this.Container.Resolve<IDomainService<Disposal>>();
            var disposalTypeSurveyDomain = this.Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var documentGjiChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var protocolDomain = this.Container.Resolve<IDomainService<Protocol>>();
            var administrativeCaseDocDomain = this.Container.Resolve<IDomainService<AdministrativeCaseDoc>>();
            var disposalProvDocs = this.Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var primaryAppCitsDomain = this.Container.ResolveDomain<PrimaryBaseStatementAppealCits>();
            var appCitsRealObjDomain = this.Container.ResolveDomain<AppealCitsRealityObject>();
            var dbConfigProvider = this.Container.Resolve<IDbConfigProvider>();

            this.ReportParams["ИдентификаторДокументаГЖИ"] = this.requirement.Id.ToString();
            this.ReportParams["СтрокаПодключениякБД"] = dbConfigProvider.ConnectionString;

            try
            {
                if (this.requirement == null)
                {
                    return;
                }

                var inspector = documentGjiInspectorDomain.GetAll()
                                                           .FirstOrDefault(x => x.DocumentGji.Id == this.requirement.Document.Id)
                                                           .With(x => new
                                                           {
                                                               x.Inspector.Position,
                                                               x.Inspector.ShortFio,
                                                               x.Inspector.Fio
                                                           });
                if (inspector != null)
                {
                    this.ReportParams["ДолжностьИнспектора"] = inspector.Position;
                    this.ReportParams["Инспектор"] = inspector.Fio;
                    this.ReportParams["ИнспекторСокр"] = inspector.ShortFio;
                }

                this.ReportParams["СрокПредоставленияМатериалов"] = this.requirement.MaterialSubmitDate.HasValue
                    ? this.requirement.MaterialSubmitDate.GetValueOrDefault().ToString("dd.MM.yyyy")
                    : "____.____.________";

                this.ReportParams["ДатаДокумента"] = this.requirement.DocumentDate.HasValue
                                                                           ? this.requirement.DocumentDate.Value
                                                                                        .ToShortDateString()
                                                                           : string.Empty;
                this.ReportParams["НомерДокумента"] = this.requirement.DocumentNumber ?? string.Empty;
                this.ReportParams["Адресат"] = this.requirement.Destination ?? string.Empty;

                this.ReportParams["СтатьяЗакона"] = this.articleLaws.Any() ? this.articleLaws.Select(x => x.ArticleLaw.Name).Distinct().ToList().AggregateWithSeparator(", ") : string.Empty;

                var realityObjs = inspectionGjiRealityObjectDomain.GetAll()
                               .Where(x => x.Inspection.Id == this.requirement.Document.Inspection.Id)
                               .Select(x => x.RealityObject)
                               .ToList();
                if (realityObjs.Count == 1)
                {
                    var firstObject = realityObjs.First();
                    this.ReportParams["ПроверяемыйДом"] = string.Format("{0}, {1}", firstObject.Municipality.Name, firstObject.Address);

                    if (firstObject.FiasAddress != null)
                    {
                        this.ReportParams["НомерДома"] = firstObject.FiasAddress.House;
                        this.ReportParams["УлицаДома"] = firstObject.FiasAddress.StreetName;
                        this.ReportParams["Город"] = firstObject.FiasAddress.PlaceName;
                        this.ReportParams["ДомАдрес1"] = firstObject.FiasAddress.StreetName + ", " + firstObject.FiasAddress.PlaceName;
                    }
                }

                var primaryBaseStateQuery = primaryAppCitsDomain.GetAll()
                        .Where(x => x.BaseStatementAppealCits.Inspection.Id == this.requirement.Document.Inspection.Id);

                if (this.CodeTemplate == "BlockGJI_Requirement_1")
                {
                    var primaryAppCitRealObj = appCitsRealObjDomain.GetAll()
                          .Where(x => primaryBaseStateQuery.Any(y => y.BaseStatementAppealCits.AppealCits.Id == x.AppealCits.Id))
                          .Select(x => x.RealityObject)
                          .FirstOrDefault();

                    if (primaryAppCitRealObj != null)
                    {
                        this.ReportParams["ПроверяемыйДом"] = string.Format("{0}, {1}", primaryAppCitRealObj.Municipality.Name, primaryAppCitRealObj.Address);

                        if (primaryAppCitRealObj.FiasAddress != null)
                        {
                            this.ReportParams["НомерДома"] = primaryAppCitRealObj.FiasAddress.House;
                            this.ReportParams["УлицаДома"] = primaryAppCitRealObj.FiasAddress.StreetName;
                            this.ReportParams["Город"] = primaryAppCitRealObj.FiasAddress.PlaceName;
                            this.ReportParams["ДомАдрес1"] = primaryAppCitRealObj.FiasAddress.StreetName + ", " + primaryAppCitRealObj.FiasAddress.PlaceName;
                        }
                    }
                }

                var primaryAppDescrip =
                    primaryBaseStateQuery.Select(x => x.BaseStatementAppealCits.AppealCits.Description)
                        .ToList()
                        .AggregateWithSeparator(", ");

                var appealCitDescription = inspectionAppealCitsDomain.GetAll()
                   .Where(x => x.Inspection.Id == this.requirement.Document.Inspection.Id && !primaryBaseStateQuery.Any(y => y.BaseStatementAppealCits.AppealCits.Id == x.AppealCits.Id))
                   .Select(x => x.AppealCits.Description)
                   .ToList()
                   .AggregateWithSeparator(", ");

                this.ReportParams["ОписаниеПроблемы"] = appealCitDescription.Any()
                    ? "{0}, {1}".FormatUsing(primaryAppDescrip, appealCitDescription)
                    : primaryAppDescrip;
                this.ReportParams["ДатаПредоставления"] = this.requirement.MaterialSubmitDate.HasValue ? this.requirement.MaterialSubmitDate.Value.ToShortDateString() : string.Empty;
                this.ReportParams["ДополнительныеМатериалы"] = this.requirement.AddMaterials;
                this.ReportParams["ДатаПриказа"] = this.requirement.Document.DocumentDate.HasValue ? this.requirement.Document.DocumentDate.Value.ToShortDateString() : string.Empty;
                this.ReportParams["НомерПриказа"] = this.requirement.Document.DocumentNumber;
                this.ReportParams["ДатаПроведенияПроверки"] = this.requirement.InspectionDate.HasValue
                                                        ? this.requirement.InspectionDate.Value
                                                                        .ToShortDateString()
                                                        : string.Empty;

                this.ReportParams["Час"] = (this.requirement.InspectionHour.HasValue ? this.requirement.InspectionHour.Value : 0).ToString();
                this.ReportParams["Мин"] = (this.requirement.InspectionMinute.HasValue ? this.requirement.InspectionMinute.Value : 0).ToString();

                this.ReportParams["ДатаиВремя"] = this.requirement.InspectionDate.HasValue
                    ? "{0} в {1} час. {2} мин.".FormatUsing(
                    this.requirement.InspectionDate.Value.ToLongDateString(),
                    this.requirement.InspectionHour.ToStr().PadLeft(2, '0'),
                    this.requirement.InspectionMinute.ToStr().PadLeft(2, '0')) : string.Empty;

                if (this.requirement.Document.TypeDocumentGji == TypeDocumentGji.Disposal)
                {
                    var disposal = disposalDomain.GetAll().FirstOrDefault(x => x.Id == this.requirement.Document.Id);
                    if (disposal != null)
                    {
                        var kindCheckStr = string.Empty;
                        if (disposal.KindCheck != null)
                        {
                            switch (disposal.KindCheck.Code)
                            {
                                case TypeCheck.PlannedExit:
                                    kindCheckStr = "плановой выездной";
                                    break;
                                case TypeCheck.NotPlannedExit:
                                    kindCheckStr = "внеплановой выездной";
                                    break;
                                case TypeCheck.PlannedDocumentation:
                                    kindCheckStr = "плановой документарной";
                                    break;
                                case TypeCheck.NotPlannedDocumentation:
                                    kindCheckStr = "внеплановой документарной";
                                    break;
                                case TypeCheck.InspectionSurvey:
                                    kindCheckStr = "инспекционной";
                                    break;
                            }
                        }

                        this.ReportParams["ВидПроверки"] = kindCheckStr;

                        var typeSurveyCodes = new List<string> { "12", "13", "14" };
                        var typeSurveyCode = disposalTypeSurveyDomain
                                     .GetAll().Where(x => x.Disposal.Id == this.requirement.Document.Id && typeSurveyCodes.Contains(x.TypeSurvey.Code))
                                     .Select(x => x.TypeSurvey.Code).FirstOrDefault();

                        this.ReportParams["ВидУслуги"] = typeSurveyCode == "12" ? "коммунальных" :
                                                                       typeSurveyCode == "13" ? "жилищных" :
                                                                       typeSurveyCode == "14" ? "коммунальных и жилищных" : string.Empty;

                        if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji)
                        {
                            var firstPrescription = documentGjiChildrenDomain.GetAll()
                                    .Where(x => x.Children.Id == disposal.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                                    .Select(x => x.Parent)
                                    .FirstOrDefault();

                            if (firstPrescription != null)
                            {
                                this.ReportParams["НомерПредписания"] = firstPrescription.DocumentNumber;
                                this.ReportParams["ДатаПредписания"] = firstPrescription.DocumentDate.HasValue
                                                                                         ? firstPrescription.DocumentDate.Value.ToShortDateString()
                                                                                         : string.Empty;
                            }
                        }
                    }
                }
                else if (this.requirement.Document.TypeDocumentGji == TypeDocumentGji.Protocol)
                {
                    var protocol = protocolDomain.GetAll().FirstOrDefault(x => x.Id == this.requirement.Document.Id);

                    if (protocol != null)
                    {
                        if (protocol.Executant != null)
                        {
                            var exDocJurPersonCodes = new List<string> { "0", "9", "11", "8", "15", "18", "4" };
                            var exDocJCodes = new List<string> { "1", "10", "12", "13", "16", "19", "5" };

                            this.ReportParams["Исполнитель"] = protocol.Executant.Code;

                            this.ReportParams["ТипИсполнителя"] = exDocJurPersonCodes.Contains(protocol.Executant.Code) ?
                                string.Format("{0} {1}", protocol.PhysicalPerson, protocol.Contragent.Name) :
                                exDocJCodes.Contains(protocol.Executant.Code) ? protocol.PhysicalPerson : string.Empty;


                            var exDocJurPersonCodes2 = new List<string> { "3", "5", "19", "10", "1" };
                            var exDocJCodes2 = new List<string> { "8", "2", "4", "18", "0", "9" };

                            this.ReportParams["ТипИсполнителя2"] = exDocJurPersonCodes2.Contains(protocol.Executant.Code) ? "должностного лица" :
                                exDocJCodes2.Contains(protocol.Executant.Code) ? "юридического лица" : string.Empty;
                        }

                        if (protocol.Contragent != null)
                        {
                            this.ReportParams["КонтрагентРП"] = !string.IsNullOrEmpty(protocol.Contragent.NameGenitive) ? protocol.Contragent.NameGenitive : protocol.Contragent.Name;
                            this.ReportParams["СокрКонтрагент"] = protocol.Contragent.ShortName;
                        }

                        var protInspector = documentGjiInspectorDomain.GetAll()
                            .Where(x => x.DocumentGji.Id == protocol.Id).Select(x => x.Inspector).FirstOrDefault();

                        if (protInspector != null)
                        {
                            this.ReportParams["ДолжностьИнспектораРП"] = protInspector.PositionGenitive;
                            this.ReportParams["ИнспекторРП"] = protInspector.FioGenitive;
                            this.ReportParams["ТелефонИнспектора"] = protInspector.Phone;
                        }

                        var firstDisposal = documentGjiChildrenDomain
                                .GetAll()
                                .Where(x => x.Children.Id == protocol.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                                .Select(x => x.Parent)
                                .FirstOrDefault();

                        if (firstDisposal != null)
                        {
                            this.ReportParams["НомерПриказа"] = firstDisposal.DocumentNumber;
                        }

                        var firstPrescription = documentGjiChildrenDomain
                                        .GetAll()
                                        .Where(x => x.Children.Id == protocol.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                                        .Select(x => x.Parent)
                                        .FirstOrDefault();

                        if (firstPrescription != null)
                        {
                            this.ReportParams["НомерПредписания"] = firstPrescription.DocumentNumber;
                            this.ReportParams["ДатаПредписания"] = firstPrescription.DocumentDate.HasValue
                                                                                     ? firstPrescription.DocumentDate.Value.ToShortDateString()
                                                                                     : string.Empty;
                        }

                        var adminCase = documentGjiChildrenDomain
                                .GetAll()
                                .Where(x => x.Children.Id == protocol.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.AdministrativeCase)
                                .Select(x => x.Parent)
                                .FirstOrDefault();

                        if (adminCase != null)
                        {
                            var adminCaseDoc = administrativeCaseDocDomain.GetAll().FirstOrDefault(x => x.AdministrativeCase.Id == adminCase.Id
                                && x.TypeAdminCaseDoc == TypeAdminCaseDoc.DefinitionInformation);

                            if (adminCaseDoc != null)
                            {
                                var adminCaseDocDate = adminCaseDoc.DocumentDate.HasValue
                                                                                         ? adminCaseDoc.DocumentDate.Value.ToShortDateString()
                                                                                         : string.Empty;
                                this.ReportParams["НомерОпределения"] = adminCaseDoc.DocumentNumber;
                                this.ReportParams["ДатаОпределения"] = adminCaseDocDate;
                                this.ReportParams["ОпределениеОбИстребованииСведений"] =
                                    string.Format("от {0} №{1} ({2})", adminCaseDocDate, adminCaseDoc.DocumentNumber, this.ReportParams["ПроверяемыйДом"].ToStr());
                            }
                        }

                    }
                }

                if (this.requirement.TypeRequirement == TypeRequirement.RequirementOnProtocol)
                {
                    var articleLaws7_21 = this.articleLaws.Where(x => x.ArticleLaw.Article == "7.21").Select(x => x.ArticleLaw.Code).ToList();

                    if (articleLaws7_21.Contains("1") && articleLaws7_21.Contains("2"))
                    {
                        this.ReportParams["ПерепланировкаПереустройство"] = "самовольной перепланировки и переустройства";
                    }
                    else if (articleLaws7_21.Contains("1"))
                    {
                        this.ReportParams["ПерепланировкаПереустройство"] = "самовольного переустройства";
                    }
                    else if (articleLaws7_21.Contains("2"))
                    {
                        this.ReportParams["ПерепланировкаПереустройство"] = "самовольной перепланировки";
                    }
                }
                else if (this.requirement.TypeRequirement == TypeRequirement.RequirementOnCheck)
                {
                    this.ReportParams["ДатаДокумента"] = this.requirement.DocumentDate.HasValue
                                                                           ? RuDateAndMoneyConverter.DateToTextLong(this.requirement.DocumentDate.Value) + " г."
                                                                                        : string.Empty;
                }

                if (this.requirement.Document.Inspection.Contragent != null)
                {
                    this.ReportParams["ЮрЛицо"] = this.requirement.Document.Inspection.Contragent.Name;

                    this.ReportParams["ЮрЛицоФактическийАдрес"] = this.requirement.Document.Inspection.Contragent.FiasFactAddress != null
                                                                ? this.requirement.Document.Inspection.Contragent.FiasFactAddress.AddressName
                                                                : string.Empty;
                }


                var provDocs =
                    disposalProvDocs.GetAll()
                                    .Where(x => x.Disposal.Id == this.requirement.Document.Id)
                                    .Select(x => new { x.ProvidedDoc.Name, x.Description })
                                    .ToList();

                //изменил поле ПредоставляемыеДокументы по комменту к задаче 37582
                //var docsResult = string.Empty;
                //var i = 0;
                //foreach (var provDoc in provDocs)
                //{
                //    if (!string.IsNullOrEmpty(docsResult))
                //    {
                //        docsResult += "\n";
                //    }
                //    docsResult += "\t- " + (!string.IsNullOrEmpty(provDoc.Description) ? provDoc.Description : provDoc.Name) + (i < provDocs.Count() - 1 ? ";" : ".");
                //    i++;
                //}

                this.ReportParams["ПредоставляемыеДокументы"] = this.requirement.AddMaterials; //docsResult;

                this.ReportParams["ПроверяемоеЛицо"] = this.requirement.Document.Inspection.Contragent.Name;
                this.ReportParams["АдресКонтрагентаФакт"] = this.requirement.Document.Inspection.Contragent.FactAddress;
            }
            finally
            {
                this.Container.Release(requirementDomain);
                this.Container.Release(documentGjiInspectorDomain);
                this.Container.Release(requirementArticleLawDomain);
                this.Container.Release(inspectionGjiRealityObjectDomain);
                this.Container.Release(inspectionAppealCitsDomain);
                this.Container.Release(disposalDomain);
                this.Container.Release(disposalTypeSurveyDomain);
                this.Container.Release(documentGjiChildrenDomain);
                this.Container.Release(protocolDomain);
                this.Container.Release(administrativeCaseDocDomain);
                this.Container.Release(disposalProvDocs);
                this.Container.Release(primaryAppCitsDomain);
                this.Container.Release(appCitsRealObjDomain);
            }
        }

        /// <summary>
        /// Получение шаблона 
        /// </summary>
        /// <returns></returns>
        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        /// <summary>
        /// Получение кода шаблона 
        /// </summary>
        private void GetCodeTemplate()
        {
            if (this.requirement == null)
            {
                return;
            }

            this.CodeTemplate = "TomskBlockGJI_Requirement";
        }
    }
}