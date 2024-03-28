namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Gkh.Authentification;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Report;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class Protocol197DefinitionReport : GjiBaseStimulReport

    {
        private long DefinitionId { get; set; }

        private Protocol197Definition definition;

        public override StiExportFormat ExportFormat
        {
            get { return GetUserFormat(); }
        }

        public Protocol197Definition Definition
        {
            get { return definition; }
            set { definition = value; }
        }

        protected override string CodeTemplate { get; set; }

        public Protocol197DefinitionReport() : base(new ReportTemplateBinary(Properties.Resources.Protocol197Definition))
        {
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DefinitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();
            var Definition = Container.Resolve<IDomainService<Protocol197Definition>>().Load(DefinitionId);
            if (Definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }

            switch (Definition.TypeDefinition)
            {
                case TypeDefinitionProtocol.ReturnProtocol:
                    CodeTemplate = "BlockGJI_Definition_1";
                    break;
                case TypeDefinitionProtocol.TimeAndPlaceHearing:
                    CodeTemplate = "BlockGJI_Definition_3";
                    break;
                case TypeDefinitionProtocol.TransferCase:
                    CodeTemplate = "BlockGJI_Definition_4";
                    break;
                case TypeDefinitionProtocol.About:
                    CodeTemplate = "BlockGJI_Definition_6";
                    break;
                case TypeDefinitionProtocol.PostponeCase:
                    CodeTemplate = "BlockGJI_Definition_8";
                    break;
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "Protocol197_Definition_1",
                    Code = "BlockGJI197_Definition_1",
                    Description =
                        "о возвращении протокола об административном правонарушении и других материалов дела должностному лицу",
                    Template = Properties.Resources.Protocol197Definition
                },
                new TemplateInfo
                {
                    Name = "Protocol197_Definition_3",
                    Code = "BlockGJI197_Definition_3",
                    Description = "о назначении времени и места рассмотрения ",
                    Template = Properties.Resources.Protocol197Definition
                },
                new TemplateInfo
                {
                    Name = "Protocol197_Definition_4",
                    Code = "BlockGJI197_Definition_4",
                    Description = "о передаче дела об административном правонарушении",
                    Template = Properties.Resources.Protocol197Definition
                },
                new TemplateInfo
                {
                    Name = "Protocol197_Definition_6",
                    Code = "BlockGJI197_Definition_6",
                    Description = "о приводе",
                    Template = Properties.Resources.Protocol197Definition
                },
                new TemplateInfo
                {
                    Name = "Protocol197_Definition_8",
                    Code = "BlockGJI197_Definition_8",
                    Description = "об отложении рассмотрения дела об административном правонарушении",
                    Template = Properties.Resources.Protocol197Definition
                }
            };
        }

        public override string Id
        {
            get { return "Protocol197Definition"; }
        }

        public override string CodeForm
        {
            get { return "Protocol197Definition"; }
        }

        public override string Name
        {
            get { return "Определение протокола 19.7"; }
        }

        public override string Description
        {
            get { return "Определение протокола 19.7"; }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            Definition = Container.Resolve<IDomainService<Protocol197Definition>>().Load(DefinitionId);

            if (Definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }
           

            var protocol = definition.Protocol197; 

            this.ReportParams["Id"] = definition.Id;
        
        }
        private StiExportFormat GetUserFormat()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            try
            {
                var oper = userManager.GetActiveOperator();
                if (oper != null)
                {
                    var ext = oper.ExportFormat;
                    switch (ext)
                    {
                        case OperatorExportFormat.docx:
                            return StiExportFormat.Word2007;
                        case OperatorExportFormat.odt:
                            return StiExportFormat.Odt;
                        case OperatorExportFormat.pdf:
                            return StiExportFormat.Pdf;
                        default: return StiExportFormat.Word2007;
                    }
                }
                else
                {
                    return StiExportFormat.Word2007;
                }

            }
            finally
            {
                this.Container.Release(userManager);
            }

        }


    }
}