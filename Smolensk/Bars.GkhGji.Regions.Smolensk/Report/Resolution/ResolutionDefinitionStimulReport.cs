using System.IO;

namespace Bars.GkhGji.Regions.Smolensk.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.GkhGji.Regions.Smolensk.Entities.Resolution;

    using Gkh.Report;
    using GkhGji.Entities;
    using B4;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    using GkhGji.Enums;

    public class ResolutionDefinitionStimulReport : GkhBaseStimulReport
    {
        #region .ctor
        public ResolutionDefinitionStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.ResolutionDefenition))
        {

        }
        #endregion .ctor

        #region Injections
        public IDomainService<ResolutionDefinitionSmol> ResolutionDefinitionDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentChildrenDomain { get; set; }
        public IDomainService<InspectionGjiRealityObject> InspectionRoDomain { get; set; }
        public IDomainService<ProtocolViolation> ProtocolViolDomain { get; set; }
        #endregion

        #region Properties

        public override string Id
        {
            get { return "ResolutionDefinitionStimulReport"; }
        }

        public override string Name
        {
            get { return "Определение постановления"; }
        }

        public override string Description
        {
            get { return "Определение постановления"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string CodeForm
        {
            get { return "ResolutionDefinition"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        #endregion Properties

        protected long DefinitionId;
        protected ResolutionDefinitionSmol Definition { get; set; }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Name = "SmolenskResolutionDefinition_1",
                            Code = "SmolenskResolutionDefinition_1",
                            Description = "о предоставлении (отказе в предоставлении) отсрочки (рассрочки) исполнения",
                            Template = Properties.Resources.ResolutionDefenition
                        },
                    new TemplateInfo
                        {
                            Name = "SmolenskResolutionDefinition_2",
                            Code = "SmolenskResolutionDefinition_2",
                            Description = "об исправлении описок, опечаток и арифметических ошибок",
                            Template = Properties.Resources.ResolutionDefenition
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
            switch (Definition.Return(x => x.TypeDefinition, TypeDefinitionResolution.Deferment))
            {
                case TypeDefinitionResolution.CorrectionError:
                    CodeTemplate = "SmolenskResolutionDefinition_2";
                    break;
                default:
                    CodeTemplate = "SmolenskResolutionDefinition_1";
                    break;
            }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DefinitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();

            Definition = ResolutionDefinitionDomain.FirstOrDefault(x => x.Id == DefinitionId);

            if (Definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            if (Definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }

            var resolution = Definition.Resolution;

            this.ReportParams["НомерОпределения"] = Definition.DocumentNum;
            this.ReportParams["ДатаОпределения"] = Definition.DocumentDate.HasValue
                                                ? Definition.DocumentDate.Value.ToShortDateString()
                                                : string.Empty;

            if (Definition.TypeDefinition == TypeDefinitionResolution.CorrectionError)
            {
                this.ReportParams["Установлено"] = Definition.DescriptionSet;
                this.ReportParams["РезультатОпределения"] = Definition.DefinitionResult;    
            }

            if (Definition.IssuedDefinition != null)
            {
                var sokrFio = !string.IsNullOrEmpty(Definition.IssuedDefinition.ShortFio) ? Definition.IssuedDefinition.ShortFio : Definition.IssuedDefinition.Fio;
                this.ReportParams["Руководитель"] = Definition.IssuedDefinition.Fio;
                this.ReportParams["РуководительФИО"] = sokrFio;
                this.ReportParams["ДолжностьРуководителя"] = Definition.IssuedDefinition.Position;
                this.ReportParams["КодРуководителяФИО_сИнициалами"] =
                    Definition.IssuedDefinition.Position + " - " + sokrFio;
            }

            this.ReportParams["ДатаПостановления"] = resolution.DocumentDate.HasValue
                                                                   ? resolution.DocumentDate.Value.ToShortDateString()
                                                                   : string.Empty;

            this.ReportParams["НомерПостановления"] = resolution.DocumentNumber;

            if (resolution.Official != null)
            {
                this.ReportParams["Инспектор"] = resolution.Official.Fio;
            }
            
            this.ReportParams["Правонарушитель"] = resolution.Contragent != null
                                                                     ? resolution.Contragent.Name
                                                                     : resolution.PhysicalPerson;
            
            this.ReportParams["Контрагент"] = resolution.Contragent != null ? resolution.Contragent.Name
                                                                : string.Empty;

            var protocol = GetParentDocument(resolution, TypeDocumentGji.Protocol);
            var placeName = string.Empty;

            if (protocol != null)
            {
                placeName = ProtocolViolDomain.GetAll()
                                      .Where(
                                          x =>
                                          x.Document.Id == protocol.Id 
                                          && x.InspectionViolation.RealityObject != null 
                                          && x.InspectionViolation.RealityObject.FiasAddress != null)
                                        .Select(x => x.InspectionViolation.RealityObject.FiasAddress.PlaceName)
                                        .FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(placeName))
            {
                placeName = InspectionRoDomain.GetAll()
                                      .Where(
                                          x =>
                                          x.Inspection.Id == resolution.Inspection.Id
                                          && x.RealityObject != null
                                          && x.RealityObject.FiasAddress != null)
                                        .Select(x => x.RealityObject.FiasAddress.PlaceName)
                                        .FirstOrDefault();
            }

            this.ReportParams["НаселенныйПункт"] = placeName;
        }

        public DocumentGji GetParentDocument(DocumentGji document, TypeDocumentGji type)
        {
            var result = document;

            if (document.TypeDocumentGji != type)
            {
                var docs = DocumentChildrenDomain.GetAll()
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