namespace Bars.GkhGji.Regions.Tomsk.Report.ProtocolGji
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using B4.Utils;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    using Entities;

    using Gkh.Report;
    using GkhGji.Entities;
    using GkhGji.Enums;

    public class ProtocolGjiDefinitionStimulReport : GkhBaseStimulReport
    {
        private long DefinitionId { get; set; }

        private TomskProtocolDefinition definition;
        public TomskProtocolDefinition Definition
        {
            get { return definition; }
            set { definition = value; }
        }

        protected override string CodeTemplate { get; set; }

        public ProtocolGjiDefinitionStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.Protocol_Definition_3))
        {
        }

        public override string Id
        {
            get { return "ProtocolDefinitionStimulReport"; }
        }

        public override string CodeForm
        {
            get { return "ProtocolDefinition"; }
        }

        public override string Name
        {
            get { return "Определение протокола"; }
        }

        public override string Description
        {
            get { return "Определение протокола"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DefinitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "Protocol_Definition_3",
                    Code = "Protocol_Definition_3",
                    Description = "о назначении времени и места рассмотрения ",
                    Template = Properties.Resources.Protocol_Definition_3
                }
            };
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        private void GetCodeTemplate()
        {
            var definitionDomain = Container.ResolveDomain<ProtocolDefinition>();
            var definition = definitionDomain.Get(DefinitionId);

            switch (definition.Return(x => x.TypeDefinition, TypeDefinitionProtocol.TimeAndPlaceHearing))
            {
                case TypeDefinitionProtocol.TimeAndPlaceHearing:
                    CodeTemplate = "Protocol_Definition_3";
                    break;
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            Definition = Container.Resolve<IDomainService<TomskProtocolDefinition>>().Load(DefinitionId);

            if (Definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }

            var protocol = definition.Protocol;

            //Report["НомерОпределения"] = definition.DocumentNum;
            this.ReportParams["ДатаОпределения"] = definition.DocumentDate.HasValue
                                                                     ? definition.DocumentDate.Value.ToShortDateString()
                                                                     : string.Empty;

            this.ReportParams["НаселенныйПункт"] =
                Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
                    .Where(x => x.Document.Id == protocol.Id && x.InspectionViolation.RealityObject.FiasAddress != null)
                    .Select(x => x.InspectionViolation.RealityObject.FiasAddress.PlaceName)
                    .FirstOrDefault();

            if (definition.IssuedDefinition != null)
            {
                this.ReportParams["Руководитель"] = definition.IssuedDefinition.Fio;
                //Report["РуководительФИО"] = definition.IssuedDefinition.ShortFio;
                this.ReportParams["ДолжностьРуководителя"] = definition.IssuedDefinition.Position;
                //Report["КодРуководителяФИО(сИнициалами)"] =
                //    definition.IssuedDefinition.Position + " - " + definition.IssuedDefinition.ShortFio;
            }

            this.ReportParams["ДатаПротокола"] = protocol.DocumentDate.HasValue
                                                                   ? protocol.DocumentDate.Value.ToShortDateString()
                                                                   : string.Empty;

            this.ReportParams["НомерПротокола"] = protocol.DocumentNumber;

            var contrName = protocol.Contragent.Return(x => x.ShortName);
            this.ReportParams["Правонарушитель"] = contrName;
            //Report["Контрагент"] = contrName;

            this.ReportParams["ИнспекторТворП"] =
                Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                         .Where(x => x.DocumentGji.Id == protocol.Id)
                         .Select(x => x.Inspector.FioAblative)
                         .FirstOrDefault();

            var articles = Container.Resolve<IDomainService<ProtocolArticleLaw>>().GetAll()
                .Where(x => x.Protocol.Id == protocol.Id)
                .Select(x => x.ArticleLaw)
                .FirstOrDefault();
                //.ToList(); // должна печататься только одна статья, если забито больше 1 типа это косяк пользователя


            if (articles != null)
            {
                var artStr = articles.Name;

                var atrDescription = articles.Description;

                this.ReportParams["ОписаниеСтатьи"] = !string.IsNullOrEmpty(atrDescription)
                                                                      ? atrDescription.ToLower().TrimEnd(new[] { ',', ' ' })
                                                                      : string.Empty;

                this.ReportParams["СтатьяЗакона"] = !string.IsNullOrEmpty(artStr)
                                                                      ? artStr.Replace("КоАП РФ", "").TrimEnd(new[] { ',', ' ' })
                                                                      : string.Empty;
            }

            //if (articles.Any())
            //{
            //    var artStr = articles.Aggregate(string.Empty, (x, y) => x + (y.Name + ", "));

            //    Report["СтатьяЗакона"] = !string.IsNullOrEmpty(artStr)
            //                                                          ? artStr.TrimEnd(new[] { ',', ' ' })
            //                                                          : string.Empty;
            //}

            //var dispId = base.GetParentDocument(protocol, TypeDocumentGji.Disposal);

            //if (dispId != null)
            //{
            //    var disposal = Container.Resolve<IDomainService<Disposal>>().Load(dispId.Id);
            //    var queryInspectorId = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
            //                                         .Where(x => x.DocumentGji.Id == disposal.Id)
            //                                         .Select(x => x.Inspector.Id).ToList();

            //    var listLocality = Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll()
            //                        .Where(x => queryInspectorId.Contains(x.Inspector.Id))
            //                        .Select(x => x.ZonalInspection.Locality)
            //                        .Distinct()
            //                        .ToList();

            //    Report["НаселПунктОтдела"] = string.Join("; ", listLocality);
            //}

            //if (protocol.Executant != null)
            //{
            //    var listTypeContragent = new List<string> { "0", "9", "11", "8", "15", "18", "4", "2" };
            //    var listTypeContrPhysicalPerson = new List<string> { "1", "10", "12", "13", "16", "19", "5", "3" };
            //    var listTypePhysicalPerson = new List<string> { "6", "7", "14" };

            //    var contr1 = string.Empty;
            //    var withRespect = string.Empty;
            //    var issue = string.Empty;
            //    var contragentName = protocol.Contragent != null ? protocol.Contragent.Name : string.Empty;
            //    var physicalPerson = protocol.PhysicalPerson;

            //    if (listTypeContragent.Contains(protocol.Executant.Code))
            //    {
            //        contr1 = contragentName;
            //        withRespect = contragentName;
            //    }
            //    if (listTypeContrPhysicalPerson.Contains(protocol.Executant.Code))
            //    {
            //        contr1 = contragentName + ", " + physicalPerson;
            //        withRespect = contragentName + " - " + physicalPerson;
            //        issue = physicalPerson;
            //    }
            //    if (listTypePhysicalPerson.Contains(protocol.Executant.Code))
            //    {
            //        contr1 = physicalPerson;
            //        withRespect = physicalPerson;
            //        issue = physicalPerson;
            //    }

            //    Report["Контрагент1"] = contr1;
            //    Report["Вотношении"] = withRespect;
            //    Report["Вручено"] = issue;
            //}


            DateTime? dateProcessing = null;
            var hour = 0;
            var minute = 0;

            if (definition.DateOfProceedings.HasValue)
            {
                dateProcessing = definition.DateOfProceedings.Value;
                hour = definition.TimeDefinition.HasValue ? definition.TimeDefinition.Value.Hour : 0;
                minute = definition.TimeDefinition.HasValue ? definition.TimeDefinition.Value.Minute : 0;
            }
            else if (definition.Protocol.DateOfProceedings.HasValue)
            {

                dateProcessing = definition.Protocol.DateOfProceedings.Value;

                hour = definition.Protocol.HourOfProceedings;
                minute = definition.Protocol.MinuteOfProceedings;
            }

            if (dateProcessing.HasValue)
            {
                var str = dateProcessing.Value.ToShortDateString() + " г.";

                if (hour > 0 || minute > 0)
                {
                    str += " в " + hour.ToString("D2") + " ч. " + minute.ToString("D2") + " мин.";
                }

                this.ReportParams["ДатаИВремяРассмотренияДела"] = str;
            }

            this.ReportParams["МестоРассмотрения"] = definition.PlaceReview;

            var adminCase = this.GetParentDocument(protocol, TypeDocumentGji.AdministrativeCase);

            if (adminCase != null)
            {
                this.ReportParams["НомерДела"] = adminCase.DocumentNumber;
            }

            //var actDoc = GetParentDocument(protocol, TypeDocumentGji.ActCheck);

            //if (actDoc != null)
            //{
            //    Report["ДатаАкта"] = actDoc.DocumentDate != null
            //                                                      ? actDoc.DocumentDate.ToDateTime().ToShortDateString()
            //                                                      : string.Empty;

            //    Report["НомерАкта"] = actDoc.DocumentNumber;
            //}

            //this.FillRegionParams(reportParams, definition);
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

        //protected virtual void FillRegionParams(ReportParams reportParams, ProtocolDefinition definition)
        //{
        //    var documentGjiInspectorDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();
        //    var protocolArticleLawDomain = Container.Resolve<IDomainService<ProtocolArticleLaw>>();

        //    using (Container.Using(documentGjiInspectorDomain, protocolArticleLawDomain))
        //    {
        //        if (definition.Protocol != null)
        //        {
        //            var definitionIssuerCode = Container.Resolve<IDomainService<Inspector>>().GetAll()
        //                .Where(x => x.Id == definition.IssuedDefinition.Id)
        //                .Select(x => x.Code)
        //                .FirstOrDefault();

        //            switch (definitionIssuerCode)
        //            {
        //                case "021":
        //                    Report["Кабинет"] = "411";
        //                    break;
        //                case "001":
        //                    Report["Кабинет"] = "410";
        //                    break;
        //                case "019":
        //                    Report["Кабинет"] = "409";
        //                    break;
        //            }

        //            var inspectorFioAblative = documentGjiInspectorDomain.GetAll()
        //               .Where(x => x.DocumentGji.Id == definition.Protocol.Id)
        //               .Select(x => x.Inspector.FioAblative)
        //               .FirstOrDefault();

        //            Report["ИнспекторТворП"] = inspectorFioAblative;

        //            if (definition.Protocol.Executant != null)
        //            {
        //                switch (definition.Protocol.Executant.Code)
        //                {
        //                    case "0":
        //                    case "4":
        //                    case "8":
        //                    case "9":
        //                    case "11":
        //                    case "15":
        //                    case "18":
        //                        if (definition.Protocol.Contragent != null)
        //                        {
        //                            Report["ТипИсполнителя"] = definition.Protocol.Contragent.ShortName;
        //                        }

        //                        break;
        //                    case "1":
        //                    case "5":
        //                    case "10":
        //                    case "12":
        //                    case "13":
        //                    case "16":
        //                    case "19":
        //                        if (definition.Protocol.Contragent != null)
        //                        {
        //                            Report["ТипИсполнителя"] = definition.Protocol.Contragent.ShortName + " " + definition.Protocol.PhysicalPerson;
        //                        }

        //                        break;
        //                    case "6":
        //                    case "7":
        //                    case "14":
        //                        Report["ТипИсполнителя"] = definition.Protocol.PhysicalPerson;
        //                        break;
        //                }
        //            }

        //            var articles = protocolArticleLawDomain.GetAll()
        //                .Where(x => x.Protocol.Id == definition.Protocol.Id)
        //                .Select(x => x.ArticleLaw.Name + " - " + x.ArticleLaw.Description)
        //                .ToList();

        //            Report["СтатьяИОписание"] = articles.Aggregate("", (x, y) => x + (!string.IsNullOrEmpty(x) ? "; " + y : y));

        //            DateTime? dateProcessing = null;
        //            var hour = 0;
        //            var minute = 0;

        //            if (definition.DateOfProceedings.HasValue)
        //            {
        //                dateProcessing = definition.DateOfProceedings.Value;
        //                hour = definition.TimeDefinition.HasValue ? definition.TimeDefinition.Value.Hour : 0;
        //                minute = definition.TimeDefinition.HasValue ? definition.TimeDefinition.Value.Minute : 0;
        //            }
        //            else if (definition.Protocol.DateOfProceedings.HasValue)
        //            {

        //                dateProcessing = definition.Protocol.DateOfProceedings.Value;

        //                hour = definition.Protocol.HourOfProceedings;
        //                minute = definition.Protocol.MinuteOfProceedings;
        //            }

        //            if (dateProcessing.HasValue)
        //            {
        //                var str = dateProcessing.Value.ToShortDateString()+" г.";
                            
        //                if (hour > 0 || minute > 0)
        //                {
        //                    str += hour.ToString("D2") + " ч. " + minute.ToString("D2") + " мин.";
        //                }

        //                Report["ДатаИВремяРассмотренияДела"] = str;
        //            }
        //        }

        //        if (definition.IssuedDefinition != null)
        //        {
        //            Report["ДолжностьДЛ"] = definition.IssuedDefinition.Position;
        //            Report["ДЛВынесшееОпределение"] = definition.IssuedDefinition.Fio;
        //        }
        //    }
        //}
    }
}