namespace Bars.GkhGji.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Report.StatisticsAppealsCits;

    using Castle.Windsor;

    public class StatisticsAppealsCitsReport : BasePrintForm
    {
        private DateTime dateStart = DateTime.MinValue;

        private DateTime dateEnd = DateTime.MaxValue;

        private long[] municipalityIds;

        public StatisticsAppealsCitsReport()
            : base(new ReportTemplateBinary(Properties.Resources.StatisticsAppealsCits))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.StatisticsAppealsCits";
            }
        }

        public override string Name
        {
            get { return "Статистические данные о работе с обращениями ГЖИ"; }
        }

        public override string Desciption
        {
            get { return "Статистические данные о работе с обращениями ГЖИ"; }
        }

        public override string GroupName
        {
            get { return "Обращения ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.StatisticsAppealsCits"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            var m = baseParams.Params["municipalityIds"].ToString();
            this.municipalityIds = !string.IsNullOrEmpty(m) ? m.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var appealCitsForType = GetData();

            // reportParams.SimpleReportParams["дата"] = dateEnd.GetDateTimeFormats()[51];
            reportParams.SimpleReportParams["дата"] = string.Format("с {0} по {1}", dateStart.ToShortDateString(), dateEnd.ToShortDateString());
            reportParams.SimpleReportParams["ВсегоОбращений"] = appealCitsForType.CountAppealCits;
            reportParams.SimpleReportParams["ЛичныйПрием"] = appealCitsForType.AcceptedPersonalAppointment;
            reportParams.SimpleReportParams["Руководством"] = appealCitsForType.Leadership;
            reportParams.SimpleReportParams["ВидеоКонференция"] = appealCitsForType.VideoConference;
            reportParams.SimpleReportParams["ПисьменныхОбращений"] = appealCitsForType.WrittenAppeal;
            reportParams.SimpleReportParams["ДоложеноРуководству"] = appealCitsForType.ReportedLeadership;
            reportParams.SimpleReportParams["ВзятоНаКонтроль"] = appealCitsForType.TakenOnControl;
            reportParams.SimpleReportParams["СВыездомНаМесто"] = appealCitsForType.VerifiedOnSpot;
            reportParams.SimpleReportParams["НарушенияПрав"] = appealCitsForType.ViolApplicantRight;
            reportParams.SimpleReportParams["ДЛПонеслиНаказание"] = appealCitsForType.OfficialsPunished;
            reportParams.SimpleReportParams["ИнтернетПриемная"] = appealCitsForType.ThroughInternet;
        }

        private Record GetData()
        {
            var codesSuretyResolve = new[] { "1", "2" };
            var verifiedOnSpot = Container.Resolve<IDomainService<InspectionAppealCits>>()
                         .GetAll()
                         .WhereIf(municipalityIds.Length == 0, y => Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll().Any(x => x.AppealCits.Id == y.AppealCits.Id))
                         .WhereIf(municipalityIds.Length > 0, y => Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll().Any(x => x.AppealCits.Id == y.AppealCits.Id && municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                         .Where(x => x.AppealCits.DateFrom >= dateStart && x.AppealCits.DateFrom <= dateEnd && codesSuretyResolve.Contains(x.AppealCits.SuretyResolve.Code))
                         .Where(y => Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll().Any(x => x.ActCheck.Inspection.Id == y.Inspection.Id && x.HaveViolation != YesNoNotSet.NotSet))
                         .Select(x => x.AppealCits.Id)
                         .AsEnumerable()
                         .Distinct()
                         .Count();

            var codesExecutant = new[] { "1", "3", "5", "10", "12", "13", "16", "19" };

            var officialsPunished = Container.Resolve<IDomainService<InspectionAppealCits>>()
                         .GetAll()
                         .WhereIf(municipalityIds.Length == 0, y => Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll().Any(x => x.AppealCits.Id == y.AppealCits.Id))
                         .WhereIf(municipalityIds.Length > 0, y => Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll().Any(x => x.AppealCits.Id == y.AppealCits.Id && municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                         .Where(x => x.AppealCits.DateFrom >= dateStart && x.AppealCits.DateFrom <= dateEnd)
                         .Where(y => Container.Resolve<IDomainService<Resolution>>().GetAll().Any(x => x.Inspection.Id == y.Inspection.Id && x.Sanction.Code != "0" && x.Sanction.Code != "2" && codesExecutant.Contains(x.Executant.Code)))
                         .Select(x => x.AppealCits.Id)
                         .AsEnumerable()
                         .Distinct()
                         .Count();

            // В поле «Резолюция» значение: проверить с выходом на место или проверить с выходом на место (по согласованию с прокуратурой) 1.1. Нет Распоряжения.
            var takenOnControl = Container.Resolve<IDomainService<InspectionAppealCits>>()
                         .GetAll()
                         .WhereIf(municipalityIds.Length == 0, y => Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll().Any(x => x.AppealCits.Id == y.AppealCits.Id))
                         .WhereIf(municipalityIds.Length > 0, y => Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll().Any(x => x.AppealCits.Id == y.AppealCits.Id && municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                         .Where(x => x.AppealCits.DateFrom >= dateStart && x.AppealCits.DateFrom <= dateEnd && (x.AppealCits.SuretyResolve.Code == "1" || x.AppealCits.SuretyResolve.Code == "2"))
                         .Where(x => x.AppealCits.State.Code == "В работе")
                         .Select(x => x.AppealCits)
                         .AsEnumerable()
                         .Distinct(x => x.Id)
                         .Count();

            var countAppealCits = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>()
                                           .GetAll()
                                           .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                                           .Where(x => x.AppealCits.DateFrom >= this.dateStart && x.AppealCits.DateFrom <= this.dateEnd)
                                           .Select(x => x.AppealCits.Id)
                                           .AsEnumerable()
                                           .Distinct()
                                           .Count();

            var violApplicantRight = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>()
                                                  .GetAll()
                                                  .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                                                  .Where(x => x.AppealCits.DateFrom >= this.dateStart && x.AppealCits.DateFrom <= this.dateEnd && x.AppealCits.RedtapeFlag.Code == 3)
                                                  .Select(x => x.AppealCits.Id)
                                                  .AsEnumerable()
                                                  .Distinct()
                                                  .Count();

            var appealCitsSource = this.Container.Resolve<IDomainService<AppealCitsSource>>().GetAll()
                .WhereIf(municipalityIds.Length == 0, y => Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll().Any(x => x.AppealCits.Id == y.AppealCits.Id))
                .WhereIf(municipalityIds.Length > 0, y => Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll().Any(x => x.AppealCits.Id == y.AppealCits.Id && municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                .Where(x => (x.RevenueForm.Code == "10" || x.RevenueForm.Code == "12" || x.RevenueForm.Code == "13" || x.RevenueForm.Code == "1" || x.RevenueForm.Code == "5"))
                .Where(x => x.AppealCits.DateFrom >= this.dateStart && x.AppealCits.DateFrom <= this.dateEnd)
                .Select(x =>
                    new
                        {
                            x.AppealCits.Id, 
                            x.RevenueForm.Code, 
                            IsSurety = x.AppealCits.Surety != null
                        })
                .ToArray();

            var acceptedPersonalAppointment = appealCitsSource.Where(x => (x.Code == "10" || x.Code == "12" || x.Code == "13")).Distinct(x => x.Id).Count();

            var leadership = appealCitsSource.Where(x => x.Code == "12").Distinct(x => x.Id).Count();
            var videoConference = appealCitsSource.Where(x => x.Code == "13").Distinct(x => x.Id).Count();
            var writtenAppeal = appealCitsSource.Where(x => x.Code == "1").Distinct(x => x.Id).Count();
            var reportedLeadership = appealCitsSource.Where(x => x.Code == "1" && x.IsSurety).Distinct(x => x.Id).Count();
            var throughInternet = appealCitsSource.Where(x => x.Code == "5").Distinct(x => x.Id).Count();

            return new Record
            {
                VerifiedOnSpot = verifiedOnSpot,
                CountAppealCits = countAppealCits,
                AcceptedPersonalAppointment = acceptedPersonalAppointment,
                Leadership = leadership,
                VideoConference = videoConference,
                WrittenAppeal = writtenAppeal,
                ReportedLeadership = reportedLeadership,

                ViolApplicantRight = violApplicantRight,
                OfficialsPunished = officialsPunished,
                ThroughInternet = throughInternet,
                TakenOnControl = takenOnControl
            };
        }
    }
}
