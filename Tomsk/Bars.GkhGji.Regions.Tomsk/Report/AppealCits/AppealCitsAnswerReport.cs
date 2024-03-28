namespace Bars.GkhGji.Regions.Tomsk.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;

    public class AppealCitsAnswerReport : GkhBaseReport
    {
        private long AppealCitsAnswerId { get; set; }

        public AppealCitsAnswerReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_AppealCitsAnswer_1))
        {
        }

        public override string Id
        {
            get { return "AppealCitsAnswer"; }
        }

        public override string CodeForm
        {
            get { return "AppealCitsAnswer"; }
        }

        public override string Name
        {
            get { return "AppealCitsAnswer"; }
        }

        public override string Description
        {
            get { return "Ответ по обращению граждан"; }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            AppealCitsAnswerId = userParamsValues.GetValue<object>("AppealCitsAnswerId").ToLong();
        }

        public override string ReportGeneratorName
        {
            get { return "DocIoGenerator"; }
        }

        protected override string CodeTemplate { get; set; }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Code = "BlockGJI_AppealCitsAnswer_1",
                                   Name = "AppealCitsAnswer",
                                   Description =
                                       "Ответ по обращению граждан",
                                   Template = Properties.Resources.BlockGJI_AppealCitsAnswer_1
                               }

                       };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            CodeTemplate = "BlockGJI_AppealCitsAnswer_1";
            
            var appealCitsAnswer = Container.Resolve<IDomainService<AppealCitsAnswer>>().Get(AppealCitsAnswerId);

            var appealCits = Container.Resolve<IDomainService<GkhGji.Entities.AppealCits>>().GetAll()
                .Where(x => x.Id == appealCitsAnswer.AppealCits.Id)
                .Select(x => new
                {
                    appealCitId = x.Id,
                    SuretyFio = x.Surety.Fio,
                    SuretyPosition = x.Surety.Position
                })
                .FirstOrDefault();

            var appealCitsAnswerData = Container.Resolve<IDomainService<AppealCitsAnswer>>().GetAll()
                .Where(x => x.AppealCits.Id == appealCits.appealCitId)
                .Select(x => new
                {
                    x.Executor.Fio,
                    x.Description, 
                    ExecutorId = (long?)x.Executor.Id,
                    x.Executor.Phone
                })
                .FirstOrDefault();
            
            if (appealCits != null)
            {
                reportParams.SimpleReportParams["Post"] = appealCits.SuretyPosition ?? string.Empty;
                reportParams.SimpleReportParams["FIO"] = appealCits.SuretyFio ?? string.Empty;
            }

            if (appealCitsAnswerData != null)
            {
                reportParams.SimpleReportParams["Executor"] = appealCitsAnswerData.Fio ?? string.Empty;
                reportParams.SimpleReportParams["Description"] = appealCitsAnswerData.Description ?? string.Empty;
                reportParams.SimpleReportParams["Phone"] = appealCitsAnswerData.Phone ?? string.Empty;

                var inspectorEmail = appealCitsAnswerData.ExecutorId.HasValue 
                    ? Container.Resolve<IDomainService<Operator>>().GetAll()
                        .Where(x => x.Inspector.Id == appealCitsAnswerData.ExecutorId.Value)
                        .Select(x => x.User.Email)
                        .FirstOrDefault()
                    : string.Empty;

                reportParams.SimpleReportParams["Email"] = inspectorEmail ?? string.Empty;
            }
            
        }
        
    }
}
