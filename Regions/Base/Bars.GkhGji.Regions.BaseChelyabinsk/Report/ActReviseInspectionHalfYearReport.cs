namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;

    using Castle.Windsor;

    public class ActReviseInspectionHalfYearReport : BasePrintForm
    {
         public IWindsorContainer Container { get; set; }

        private readonly IDomainService<BaseJurPerson> _jurPersonDomain;
        private readonly IDomainService<ActCheckPeriod> _actCheckPeriodDomain;
        private readonly IDomainService<GkhGji.Entities.ActCheck> _actCheckDomain;
        private readonly IDomainService<InspectionGjiViol> _violationDomain;
        private readonly IDomainService<GkhGji.Entities.Prescription> _prescriptionDomain;
        private readonly IDomainService<GkhGji.Entities.Protocol> _protocolDomain;
        private readonly IDomainService<InspectionGjiInspector> _inspectorDomain;

        private int year;
        private int halfYear;

        public ActReviseInspectionHalfYearReport(
            IDomainService<BaseJurPerson> jurPersonDomain,
            IDomainService<ActCheckPeriod> actCheckPeriodDomain,
            IDomainService<GkhGji.Entities.ActCheck> actCheckDomain,
            IDomainService<InspectionGjiViol> violationDomain,
            IDomainService<GkhGji.Entities.Prescription> prescriptionDomain,
            IDomainService<GkhGji.Entities.Protocol> protocolDomain,
            IDomainService<InspectionGjiInspector> inspectorDomain)
            : base(new ReportTemplateBinary(Resources.ActReviseInspectionHalfYear))
        {
            this._jurPersonDomain = jurPersonDomain;
            this._actCheckPeriodDomain = actCheckPeriodDomain;
            this._actCheckDomain = actCheckDomain;
            this._violationDomain = violationDomain;
            this._prescriptionDomain = prescriptionDomain;
            this._protocolDomain = protocolDomain;
            this._inspectorDomain = inspectorDomain;
        }

        public override string Name
        {
            get { return "Акт сверки плановых проверок за полугодие года"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string Desciption
        {
            get { return "Акт сверки плановых проверок за полугодие года"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ActReviseInspectionHalfYear"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.ActReviseInspectionHalfYear";
            }
        }

        public override string ReportGenerator { get; set; }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.year = baseParams.Params.GetAs<int>("year");
            this.halfYear = baseParams.Params.GetAs<int>("halfYear");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["Год"] = this.year;
            reportParams.SimpleReportParams["Полугодие"] = this.halfYear;

            List<BaseJurPerson> jurPersons;

            if (this.halfYear == 1)
            {
                jurPersons = this._jurPersonDomain.GetAll()
                    .Where(x => x.DateStart != null 
                        && x.DateStart.Value.Year == this.year
                        && x.DateStart.Value.Month >= 1
                        && x.DateStart.Value.Month <= 6)
                    .ToList();
            }
            else
            {
                jurPersons = this._jurPersonDomain.GetAll()
                    .Where(x => x.DateStart != null
                        && x.DateStart.Value.Year == this.year
                        && x.DateStart.Value.Month >= 7
                        && x.DateStart.Value.Month <= 12)
                    .ToList();
            }

            var actChecksList =
                this._actCheckDomain.GetAll()
                    .OrderBy(x => x.DocumentDate)
                    .Select(x => new
                                     {
                                         InspectionId = x.Inspection.Id, 
                                         ActCheckId = x.Id,
                                         x.DocumentDate
                                     })
                    .ToList();

            var num = 0;

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");

            foreach (var jurPerson in jurPersons)
            {
                section.ДобавитьСтроку();
                section["НомерПП"] = ++num;
                section["НаименованиеЮрЛица"] = jurPerson.Contragent.Name;
                section["ДатаНачалаПроверки"] = jurPerson.DateStart.HasValue
                                                    ? jurPerson.DateStart.Value.ToShortDateString()
                                                    : string.Empty;
                //section["ФактичДатаНачала"] = jurPerson.DateStart.HasValue
                //                                    ? jurPerson.DateStart.Value.ToShortDateString()
                //                                    : string.Empty;
                section["СрокДней"] = jurPerson.CountDays;
                section["СрокЧасов"] = jurPerson.CountHours;

                section["ФактичСрок"] = string.Empty;
                var actCheck = actChecksList.FirstOrDefault(x => x.InspectionId == jurPerson.Id && x.DocumentDate.HasValue);
                if (actCheck != null)
                {
                    section["ФактичДатаНачала"] = actCheck.DocumentDate.Value.ToShortDateString();
                    var actCheckPeriods =
                        this._actCheckPeriodDomain.GetAll().Where(x => x.ActCheck.Id == actCheck.ActCheckId).ToList();

                    if (actCheckPeriods.Any())
                    {
                        var sum = new TimeSpan();
                        foreach (var actCheckPeriod in actCheckPeriods)
                        {
                            sum += actCheckPeriod.DateEnd.Value - actCheckPeriod.DateStart.Value;
                        }

                        section["ФактичСрок"] = sum.Hours;
                    }
                }

                section["ФактПроверки"] = jurPerson.TypeFact.GetEnumMeta().Display;
                section["Причина"] = jurPerson.Reason;

                section["КоличНарушений"] = this._violationDomain.GetAll().Count(x => x.Inspection.Id == jurPerson.Id);

                section["НомерПредписания"] = string.Empty;
                var prescr =
                    this._prescriptionDomain.GetAll().FirstOrDefault(x => x.Inspection.Id == jurPerson.Id);
                if (prescr != null)
                {
                    section["НомерПредписания"] = prescr.DocumentNumber;
                }

                section["НомерПротокола"] = string.Empty;
                var protocol =
                    this._protocolDomain.GetAll().FirstOrDefault(x => x.Inspection.Id == jurPerson.Id);
                if (protocol != null)
                {
                    section["НомерПротокола"] = protocol.DocumentNumber;
                }

                section["Инспектор"] = string.Empty;
                var inspector =
                    this._inspectorDomain.GetAll().FirstOrDefault(x => x.Inspection.Id == jurPerson.Id);
                if (inspector != null)
                {
                    section["Инспектор"] = inspector.Inspector.ShortFio;
                }
            }
        }
    }
}