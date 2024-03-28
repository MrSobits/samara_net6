namespace Bars.GkhGji.Regions.Nso.Report
{
    using System.Linq;

    using Bars.B4;
    
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class JurPersonInspectionPlanReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private readonly IDomainService<BaseJurPerson> _jurPersonDomain;
        private readonly IDomainService<InspectionGjiRealityObject> _inspectionRoDomain;
        private readonly IDomainService<BaseJurPersonContragent> _inspectionContragentDomain;

        private int year;

        public JurPersonInspectionPlanReport(
            IDomainService<BaseJurPerson> jurPersonDomain,
            IDomainService<InspectionGjiRealityObject> inspectionRoDomain,
            IDomainService<BaseJurPersonContragent> inspectionContragentDomain)
            : base(new ReportTemplateBinary(Properties.Resources.JurPersonInspectionPlanReport))
        {
            _jurPersonDomain = jurPersonDomain;
            _inspectionRoDomain = inspectionRoDomain;
            _inspectionContragentDomain = inspectionContragentDomain;
        }

        public override string Name
        {
            get { return "План проведения плановых проверок ЮЛ и ИП"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string Desciption
        {
            get { return "План проведения плановых проверок ЮЛ и ИП"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.JurPersonInspectionPlan"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.JurPersonInspectionPlan";
            }
        }

        public override string ReportGenerator { get; set; }

        public override void SetUserParams(BaseParams baseParams)
        {
            year = baseParams.Params.GetAs<int>("year");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["Год"] = year;

            var jurPersons = _jurPersonDomain.GetAll()
                .Where(x => x.DateStart != null 
                    && x.DateStart.Value.Year == year)
                .ToList();

            var inspectionRos =
                _inspectionRoDomain.GetAll().ToList();

            var inspectionContragents = 
                _inspectionContragentDomain.GetAll().ToList();

            var num = 0;

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");

            foreach (var jurPerson in jurPersons)
            {
                section.ДобавитьСтроку();
                section["НомерПП"] = ++num;

                if (jurPerson.Contragent != null)
                {
                    section["НаимЮрЛица"] = jurPerson.Contragent.Name;

                    var orgForm = jurPerson.Contragent.OrganizationForm;
                    if (orgForm != null)
                    {
                        section["АдресЮрЛица"] = jurPerson.Contragent.OrganizationForm.Name == "Индивидуальные предприниматели"
                                             ? string.Empty
                                             : jurPerson.Contragent.JuridicalAddress;
                        section["АдресИП"] = jurPerson.Contragent.OrganizationForm.Name == "Индивидуальные предприниматели"
                                                 ? jurPerson.Contragent.JuridicalAddress
                                                 : string.Empty;
                    }

                    section["ФактичАдрес"] = jurPerson.Contragent.FactAddress;
                    section["ОГРН"] = jurPerson.Contragent.Ogrn;
                    section["ИНН"] = jurPerson.Contragent.Inn;
                    if (jurPerson.TypeBaseJuralPerson == TypeBaseJurPerson.StateRegistrationAfter3Years)
                    {
                        section["ДатаРегистрации"] = jurPerson.Contragent.DateRegistration.HasValue
                            ? jurPerson.Contragent.DateRegistration.Value.ToShortDateString()
                            : string.Empty;
                    }
                    if (jurPerson.TypeBaseJuralPerson == TypeBaseJurPerson.LastWorkAfter3Years)
                    {
                        var lastCheck = _jurPersonDomain.GetAll()
                            .Where(x => x.Contragent != null)
                            .Where(x => x.Contragent.Id == jurPerson.Contragent.Id
                                && x.TypeFact == TypeFactInspection.Done)
                                .OrderByDescending(x => x.DateStart)
                                .FirstOrDefault();
                        if (lastCheck != null)
                        {
                            section["ДатаПослПроверки"] = lastCheck.DateStart.HasValue
                                ? lastCheck.DateStart.Value.ToShortDateString()
                                : string.Empty;
                        }
                    }
                    if (jurPerson.TypeBaseJuralPerson == TypeBaseJurPerson.StartBusinessAfter3Years)
                    {
                        section["ДатаНачалаДеятельности"] = jurPerson.Contragent.ActivityDateStart.HasValue
                            ? jurPerson.Contragent.ActivityDateStart.Value.ToShortDateString()
                            : string.Empty;
                    }
                }

                var realityObjects =
                    inspectionRos.Where(x => x.Inspection.Id == jurPerson.Id)
                        .Where(x => x.RealityObject != null)
                        .Select(x => x.RealityObject.Address)
                        .ToArray();
                section["АдресаДомов"] = string.Join(",", realityObjects);

                section["ИныеОснования"] = jurPerson.AnotherReasons;

                section["ДатаНачалаПроверки"] = jurPerson.DateStart.HasValue
                                                 ? jurPerson.DateStart.Value.ToShortDateString()
                                                 : string.Empty;

                section["СрокДней"] = jurPerson.CountDays.HasValue
                    ? jurPerson.CountDays.Value.ToString()
                    : string.Empty;

                section["СрокЧасов"] = jurPerson.CountHours.HasValue
                    ? jurPerson.CountHours.Value.ToString()
                    : string.Empty;

                section["ФормаПроверки"] = jurPerson.TypeForm.GetEnumMeta().Display;

                var contragents =
                    inspectionContragents.Where(x => x.BaseJurPerson.Id == jurPerson.Id)
                        .Where(x => x.Contragent != null)
                        .Select(x => x.Contragent.Name)
                        .ToArray();

                section["НаименованиеКонтрагентов"] = string.Join(",", contragents);
            }
        }
    }
}