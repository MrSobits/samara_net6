namespace Bars.GkhGji.Regions.Tula.Report
{
    using System;
    using System.Linq;
    using Bars.B4;
    
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class RegistrationOutgoingDocuments : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private long[] municipalityIds;

        public RegistrationOutgoingDocuments()
            : base(new ReportTemplateBinary(Properties.Resources.RegistrationOutgoingDocuments))
        {
        }
        
        public override string Name
        {
            get { return "Регистрация исходящих документов"; }
        }

        public override string Desciption
        {
            get { return "Регистрация исходящих документов"; }
        }

        public override string GroupName
        {
            get { return "Обращения ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.RegistrationOutgoingDocuments"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "GkhGji.Report.RegistrationOutgoingDocuments";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var appealCitsRealityObject = Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    x.AppealCits.Id,
                    MuName = x.RealityObject.Municipality.Name
                })
                .AsEnumerable()
                .OrderBy(x => x.MuName)
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.ToList());

            var appealCitsRealityObjectIds = appealCitsRealityObject.Select(x => x.Key).ToList();

            var appealCitsAnswerData = Container.Resolve<IDomainService<AppealCitsAnswer>>().GetAll()
                .Where(x => appealCitsRealityObjectIds.Contains(x.AppealCits.Id))
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .Select(x => new
                {
                    AppealCitsId = x.AppealCits.Id,
                    x.DocumentNumber,
                    x.DocumentDate,
                    x.Addressee.Name,
                    x.Description,
                    x.Executor.Fio
                })
                .ToList();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var num = 1;


            foreach (var appealCits in appealCitsRealityObject)
            {
                foreach (var answer in appealCitsAnswerData.Where(x => x.AppealCitsId == appealCits.Key))
                {
                    section.ДобавитьСтроку();

                    var appealCitsId = answer.AppealCitsId;
                    section["Num"] = num++;
                    section["MU"] = appealCitsRealityObject.Where(x => x.Key == appealCitsId).Select(y => y.Value.Select(x => x.MuName).FirstOrDefault()).FirstOrDefault();
                    section["RegNum"] = answer.DocumentNumber;
                    section["DateReg"] = answer.DocumentDate;
                    section["RecipientName"] = answer.Name;
                    section["Summary"] = answer.Description;
                    section["Executant"] = answer.Fio;
                }
            }

            reportParams.SimpleReportParams["DateStart"] = dateStart.ToDateTime().ToShortDateString();
            reportParams.SimpleReportParams["DateEnd"] = dateEnd.ToDateTime().ToShortDateString();
        }
    }
}
