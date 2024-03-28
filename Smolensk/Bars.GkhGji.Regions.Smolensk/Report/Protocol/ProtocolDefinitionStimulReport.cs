using System.IO;

namespace Bars.GkhGji.Regions.Smolensk.Report.Protocol
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4.IoC;
    using Entities.Protocol;
    using Gkh.Entities;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.Entities;
    using B4;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    using GkhGji.Enums;

    public class ProtocolDefinitionStimulReport : GkhBaseStimulReport
    {
        #region .ctor

        public ProtocolDefinitionStimulReport() : base(new ReportTemplateBinary(Properties.Resources.Protocol_Definition_1))
        {

        }

        #endregion .ctor

        #region Properties

        public override string Id
        {
            get { return "ProtocolDefinitionStimulReport"; }
        }

        public override string Name
        {
            get { return "Определение протокола"; }
        }

        public override string Description
        {
            get { return "Определение протокола"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string CodeForm
        {
            get { return "ProtocolDefinition"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        #endregion Properties

        protected long DefinitionId;
        protected ProtocolDefinitionSmol definition { get; set; }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "Protocol_Definition_1",
                    Code = "Protocol_Definition_1",
                    Description = "об отказе в удовлетворении ходатайства",
                    Template = Properties.Resources.Protocol_Definition_1
                },

                new TemplateInfo
                {
                    Name = "Protocol_Definition_2",
                    Code = "Protocol_Definition_2",
                    Description = "о возвращении протокола об административном правонарушении и других материалов дела должностному лицу",
                    Template = Properties.Resources.Protocol_Definition_2
                },

                new TemplateInfo
                {
                    Name = "Protocol_Definition_3",
                    Code = "Protocol_Definition_3",
                    Description = "о передаче дела об административном правонарушении",
                    Template = Properties.Resources.Protocol_Definition_3
                },

                new TemplateInfo
                {
                    Name = "Protocol_Definition_4",
                    Code = "Protocol_Definition_4",
                    Description = "о продлении срока рассмотрения дела об административном правонарушении",
                    Template = Properties.Resources.Protocol_Definition_4
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
            switch (definition.Return(x => x.TypeDefinition, TypeDefinitionProtocol.DenialPetition))
            {
                case TypeDefinitionProtocol.DenialPetition:
                    CodeTemplate = "Protocol_Definition_1";
                    break;
                case TypeDefinitionProtocol.ReturnProtocol:
                    CodeTemplate = "Protocol_Definition_2";
                    break;
                case TypeDefinitionProtocol.TransferCase:
                    CodeTemplate = "Protocol_Definition_3";
                    break;
                case TypeDefinitionProtocol.TermAdministrativeInfraction:
                    CodeTemplate = "Protocol_Definition_4";
                    break;
            }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DefinitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();

            var service = Container.ResolveDomain<ProtocolDefinitionSmol>();
            try
            {
                definition = service.FirstOrDefault(x => x.Id == DefinitionId);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var definitionDomain = Container.ResolveDomain<ProtocolDefinitionSmol>();
            var protArtLawDomain = Container.ResolveDomain<ProtocolArticleLaw>();
            var docInspDomain = Container.ResolveDomain<DocumentGjiInspector>();

            ProtocolDefinitionSmol definition;
            ArticleLawGji[] articles;
            Inspector[] inspectors;

            using (Container.Using(definitionDomain, protArtLawDomain, docInspDomain))
            {
                definition = definitionDomain.Get(DefinitionId);

                if (definition == null)
                {
                    throw new ReportProviderException("Не удалось получить определение");
                }
                
                articles = protArtLawDomain.GetAll()
                    .Where(x => x.Protocol.Id == definition.Protocol.Id)
                    .Where(x => x.ArticleLaw != null)
                    .Select(x => x.ArticleLaw)
                    .ToArray();

                inspectors = docInspDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == definition.Protocol.Id)
                    .Where(x => x.Inspector != null)
                    .Select(x => x.Inspector)
                    .ToArray();
            }

            FillCommonFields(articles, inspectors);

            if (definition.TypeDefinition == TypeDefinitionProtocol.DenialPetition || definition.TypeDefinition == TypeDefinitionProtocol.TermAdministrativeInfraction)
            {
                this.ReportParams["Установлено"] = definition.DescriptionSet;
                this.ReportParams["РезультатОпределения"] = definition.DefinitionResult;
            }

            if (definition.TypeDefinition == TypeDefinitionProtocol.ReturnProtocol || definition.TypeDefinition == TypeDefinitionProtocol.TransferCase || definition.TypeDefinition == TypeDefinitionProtocol.TermAdministrativeInfraction)
            {
                this.ReportParams["НаселенныйПункт"] =
                Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
                    .Where(x => x.Document.Id == definition.Protocol.Id && x.InspectionViolation.RealityObject.FiasAddress != null)
                    .Select(x => x.InspectionViolation.RealityObject.FiasAddress.PlaceName)
                    .FirstOrDefault();
            }
        }

        protected void FillCommonFields(ArticleLawGji[] articles, Inspector[] inspectors)
        {
            this.ReportParams["ДатаОпределения"] =
                definition.DocumentDate.HasValue
                    ? definition.DocumentDate.Value.ToShortDateString()
                    : null;
            
            if (definition.TypeDefinition == TypeDefinitionProtocol.TermAdministrativeInfraction)
            {
                this.ReportParams["ПродлитьДо"] = definition.ExtendUntil.HasValue
                                           ? definition.ExtendUntil.Value.ToShortDateString()
                                           : string.Empty;
            }

            this.ReportParams["Инспектор"] = inspectors.FirstOrDefault(x => x.Fio != null).Return(x => x.Fio);
            this.ReportParams["НомерПротокола"] = definition.Protocol.Return(x => x.DocumentNumber);
            this.ReportParams["СтатьяЗакона"] = articles.AggregateWithSeparator(x => x.Name, ", ");
            this.ReportParams["Правонарушитель"] =
                definition.Protocol.Return(x => x.Contragent).Return(x => x.Name)
                ?? definition.Protocol.PhysicalPerson;

            if (definition.IssuedDefinition != null)
            {
                this.ReportParams["ДолжностьРуководителя"] = definition.IssuedDefinition.Position;
                this.ReportParams["Руководитель"] = definition.IssuedDefinition.Fio;
            }
        }
    }
}