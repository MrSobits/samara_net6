namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;

    using Castle.Windsor;

    public class NoActionsMadeListPrescriptionsReport : BasePrintForm
    {
        private readonly IWindsorContainer _container;
        private readonly IDomainService<GkhGji.Entities.Prescription> _prescriptionDomain;
        private readonly IDomainService<DocumentGjiChildren> _docChildrenDomain;
        private readonly IDomainService<GkhGji.Entities.ActRemoval> _actRemovalDomain;
        private readonly IDomainService<DocumentGjiInspector> _documentInspectorDomain;
        private readonly IDomainService<Inspector> _inspectorDomain;
        private readonly IDomainService<InspectionGjiViolStage> _gjiViolDomain;

        private List<long> municipalityIds;

        public NoActionsMadeListPrescriptionsReport(
            IWindsorContainer container,
            IDomainService<GkhGji.Entities.Prescription> prescriptionDomain,
            IDomainService<DocumentGjiChildren> docChildrenDomain,
            IDomainService<GkhGji.Entities.ActRemoval> actRemovalDomain,
            IDomainService<DocumentGjiInspector> documentInspectorDomain,
            IDomainService<Inspector> inspectorDomain,
            IDomainService<InspectionGjiViolStage> gjiViolDomain)
            : base(new ReportTemplateBinary(Resources.NoActionsMadeListPrescriptions))
        {
            this._container = container;
            this._prescriptionDomain = prescriptionDomain;
            this._docChildrenDomain = docChildrenDomain;
            this._actRemovalDomain = actRemovalDomain;
            this._documentInspectorDomain = documentInspectorDomain;
            this._inspectorDomain = inspectorDomain;
            this._gjiViolDomain = gjiViolDomain;
        }

        public override string Name
        {
            get { return "Список предписаний, по которым не выполнены мероприятия"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string Desciption
        {
            get { return "Список предписаний, по которым не выполнены мероприятия"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.NoActionsMadeListPrescriptions"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.NoActionsMadeListPrescriptions";
            }
        }

        public override string ReportGenerator { get; set; }

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            this.municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["ДатаФормирОтчета"] = DateTime.Now.ToShortDateString();

            // получаю акты проверки предписаний согласно условиям отчета
            var actRemovals = this._actRemovalDomain.GetAll()
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Inspection.Contragent.Municipality.Id))
                .Where(x => x.TypeRemoval == YesNoNotSet.No 
                    && x.State.Name != "Черновик")
                .Select(x => x.Id);

            // беру id документов, у которых в children выбранные акты
            var prescriptionIds =
                this._docChildrenDomain.GetAll().Where(x => actRemovals.Contains(x.Children.Id)).Select(x => x.Parent.Id);

            // получаю предписания по полученным id документов
            var prescriptions = this._prescriptionDomain.GetAll().Where(x => prescriptionIds.Contains(x.Id)).ToList();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");

            foreach (var prescription in prescriptions)
            {
                section.ДобавитьСтроку();
                section["Статус"] = prescription.State.Name;
                section["ОснованиеПроверки"] = prescription.Inspection.TypeBase.GetEnumMeta().Display;

                // получаю id инспекторов, относящихся к этому документу
                var inspectionInspectors =
                    this._documentInspectorDomain.GetAll()
                        .Where(x => x.DocumentGji.Id == prescription.Id)
                        .Select(x => x.Inspector.Id)
                        .ToList();

                // получаю Фио инспекторов
                var inspectors =
                    this._inspectorDomain.GetAll()
                        .Where(x => inspectionInspectors.Contains(x.Id))
                        .Select(x => x.ShortFio)
                        .ToArray();

                section["Инспекторы"] = string.Join(",", inspectors);
                section["МуниципОбразования"] = prescription.Contragent.Return(x => x.Municipality).Return(x => x.Name);
                section["ТипИсполнителя"] = prescription.Executant.Name;
                section["Организация"] = prescription.Contragent.Name;

                var viols = this._gjiViolDomain.GetAll()
                    .Where(x => x.Document.Id == prescription.Id)
                    .ToList();

                section["КоличДомов"] = viols.Distinct(x => x.InspectionViolation.RealityObject.Id).Count();
                section["НомерДокумента"] = prescription.DocumentNumber;
                section["ДатаДокумента"] = prescription.DocumentDate;
                section["КоличНаруш"] = viols.Count();

                section["СрокИсполнения"] = string.Empty;
                var lastDateViol =
                    viols
                    .OrderBy(x => x.DatePlanRemoval != null ? x.DatePlanRemoval.Value : DateTime.MinValue)
                    .LastOrDefault();
                if (lastDateViol != null)
                {
                    if (lastDateViol.DatePlanRemoval != null)
                    {
                        section["СрокИсполнения"] = lastDateViol.DatePlanRemoval.Value.ToShortDateString();
                    }
                }
            }
        }
    }
}