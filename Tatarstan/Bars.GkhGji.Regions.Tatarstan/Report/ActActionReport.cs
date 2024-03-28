namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;
    using Bars.GkhGji.Regions.Tatarstan.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Properties;

    public class ActActionReport : GkhBaseStimulReport
    {
        public ActActionReport() : base(new ReportTemplateBinary(Resources.ActActionIsolated))
        {
            base.ExportFormat = StiExportFormat.Word2007;
        }

        protected override string CodeTemplate { get; set; }

        protected long DocumentId { get; set; }

        public override string Extention => "mrt";

        public override string Id => "ProtocolActAction";

        public override string CodeForm => "ProtocolActAction";

        public override string Name => "Протокол действия акта";

        public override string Description => "Протокол действия акта";

        // Шаблоны для акта проверки с типом TypeActCheckGji.ActActionIsolated
        private Dictionary<ActCheckActionType, TemplateInfo> ActActionTemplateInfoDict =>
            new Dictionary<ActCheckActionType, TemplateInfo>
            {
                {
                    ActCheckActionType.Inspection, new TemplateInfo
                    {
                        Name = "Протокол осмотра (Выездное обследование) по КНМ без взаимодействия",
                        Code = "ProtocolActActionInspectionAction",
                        Description = "Протокол осмотра (Выездное обследование) по КНМ без взаимодействия",
                        Template = Resources.ProtocolActActionInspectionAction
                    }
                },
                {
                    ActCheckActionType.InstrumentalExamination, new TemplateInfo
                    {
                        Name = "Протокол инструментального обследования по КНМ без взаимодействия",
                        Code = "ProtocolActActionInstrExamAction",
                        Description = "Протокол инструментального обследования по КНМ без взаимодействия",
                        Template = Resources.ProtocolActActionInstrExamAction
                    }
                }
            };

        // Шбалоны для остальных актов проверки
        private Dictionary<ActCheckActionType, TemplateInfo> ActCheckTemplateInfoDict =>
            new Dictionary<ActCheckActionType, TemplateInfo>
            {
                {
                    ActCheckActionType.Inspection, new TemplateInfo
                    {
                        Name = "Протокол осмотра по акту проверки",
                        Code = "ProtocolActCheckInspectionAction",
                        Description = "Протокол осмотра по акту проверки",
                        Template = Resources.ProtocolActCheckInspectionAction
                    }
                },
                {
                    ActCheckActionType.Survey, new TemplateInfo
                    {
                        Name = "Протокол опроса по акту проверки",
                        Code = "ProtocolActCheckSurveyAction",
                        Description = "Протокол опроса по акту проверки",
                        Template = Resources.ProtocolActCheckSurveyAction
                    }
                },
                {
                    ActCheckActionType.InstrumentalExamination, new TemplateInfo
                    {
                        Name = "Протокол инструментального обследования по акту проверки",
                        Code = "ProtocolActCheckInstrExamAction",
                        Description = "Протокол инструментального обследования по акту проверки",
                        Template = Resources.ProtocolActCheckInstrExamAction
                    }
                },
                {
                    ActCheckActionType.RequestingDocuments, new TemplateInfo
                    {
                        Name = "Протокол истребования документов по акту проверки",
                        Code = "ProtocolActCheckDocRequestAction",
                        Description = "Протокол истребования документов по акту проверки",
                        Template = Resources.ProtocolActCheckDocRequestAction
                    }
                },
                {
                    ActCheckActionType.GettingWrittenExplanations, new TemplateInfo
                    {
                        Name = "Протокол объяснения по акту проверки",
                        Code = "ProtocolActCheckExplanationAction",
                        Description = "Протокол объяснения по акту проверки",
                        Template = Resources.ProtocolActCheckExplanationAction
                    }
                }
            };

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return this.ActActionTemplateInfoDict.Values
                .Concat(this.ActCheckTemplateInfoDict.Values)
                .ToList();
        }

        public override Stream GetTemplate()
        {
            this.ChangeCodeTemplate();
            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["Id"] = this.DocumentId.ToString();
        }

        private void ChangeCodeTemplate()
        {
            var actionDomain = this.Container.Resolve<IDomainService<ActCheckAction>>();

            using (this.Container.Using(actionDomain))
            {
                var action = actionDomain.Get(this.DocumentId);
                if (action.IsNull())
                {
                    return;
                }

                var act = action.ActCheck;

                this.CodeTemplate = this.GetTemplateSourceDict(act.TypeActCheck)
                    .TryGetValue(action.ActionType, out var templateInfo)
                    ? templateInfo.Code
                    : string.Empty;
            }
        }

        private Dictionary<ActCheckActionType, TemplateInfo> GetTemplateSourceDict(TypeActCheckGji typeAct)
        {
            switch (typeAct)
            {
                case TypeActCheckGji.ActActionIsolated:
                    return this.ActActionTemplateInfoDict;
                default:
                    return this.ActCheckTemplateInfoDict;
            }
        }
    }
}