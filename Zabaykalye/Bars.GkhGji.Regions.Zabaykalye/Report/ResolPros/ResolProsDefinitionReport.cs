using Bars.GkhGji.Regions.Zabaykalye.Entities;
using Bars.GkhGji.Regions.Zabaykalye.Enums;
using Bars.GkhGji.Report;

namespace Bars.GkhGji.Regions.Zabaykalye.Report
{
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;

    public class ResolProsDefinitionReport : GjiBaseReport
    {
        private long DefinitionId { get; set; }

        protected override string CodeTemplate { get; set; }

        public ResolProsDefinitionReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_Definition_3))
        {
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
                    Name = "ResolPros_Definition_3",
                    Code = "BlockGJI_Definition_3",
                    Description = "о назначении времени и места рассмотрения ",
                    Template = Properties.Resources.BlockGJI_Definition_3
                },
                new TemplateInfo
                {
                    Name = "ResolPros_Definition_8",
                    Code = "BlockGJI_Definition_8",
                    Description = "об отложении рассмотрения дела об административном правонарушении",
                    Template = Properties.Resources.BlockGJI_Definition_8
                }
            };
        }

        public override string Id
        {
            get { return "ResolProsDefinition"; }
        }

        public override string CodeForm
        {
            get { return "ResolProsDefinition"; }
        }

        public override string Description
        {
            get { return "Определение постановления прокуратуры"; }
        }

        public override string Name
        {
            get { return "Oпpeд. пocтaнoв. пpoкyp."; }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var definition = Container.Resolve<IDomainService<ResolProsDefinition>>().Load(DefinitionId);

            if (definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }

            switch (definition.TypeDefinition)
            {
                case TypeDefinitionResolPros.TimeAndPlaceHearing:
                    CodeTemplate = "BlockGJI_Definition_3";
                    break;
                case TypeDefinitionResolPros.PostponeCase:
                    CodeTemplate = "BlockGJI_Definition_8";
                    break;
            }
        }
    }
}