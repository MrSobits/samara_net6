namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Castle.Windsor;

    public class CheckingExecutionOfPrescriptionReport : BasePrintForm
    {

        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.Now;
        private DateTime dateEnd = DateTime.Now;
        private List<long> municipalityListId = new List<long>();

        public CheckingExecutionOfPrescriptionReport()
            : base(new ReportTemplateBinary(Bars.GkhGji.Properties.Resources.CheckingExecutionOfPrescription))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.CheckingExecutionOfPrescription";
            }
        }

        public override string Name
        {
            get { return "Проверка исполнения предписаний"; }
        }

        public override string Desciption
        {
            get { return "Проверка исполнения предписаний"; }
        }

        public override string GroupName
        {
            get { return "Инспекторская деятельность"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.CheckingExecutionOfPrescription"; }
        }

        private void ParseIds(List<long> list, string Ids)
        {
            list.Clear();

            if (!string.IsNullOrEmpty(Ids))
            {
                var ids = Ids.Split(',');
                foreach (var id in ids)
                {
                    long Id = 0;
                    if (long.TryParse(id, out Id))
                    {
                        if (!list.Contains(Id))
                        {
                            list.Add(Id);
                        }
                    }
                }
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.dateStart = baseParams.Params["dateStart"].ToDateTime();
            this.dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            this.ParseIds(this.municipalityListId, baseParams.Params["municipalityIds"].ToString());
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["НачалоПериода"] = $"{this.dateStart:d MMMM yyyy}";
            reportParams.SimpleReportParams["КонецПериода"] = $"{this.dateEnd:d MMMM yyyy}";
            reportParams.SimpleReportParams["ДатаСборки"] = $"{DateTime.Now:d MMMM yyyy}";

            var serviceInspectionGjiViolStage = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>();
            var serviceRealityObject = this.Container.Resolve<IDomainService<RealityObject>>();
            var servicePrescription = this.Container.Resolve<IDomainService<Prescription>>();
            var servicePrescriptionCancel = this.Container.Resolve<IDomainService<PrescriptionCancel>>();
            var serviceDocumentGjiInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceDocumentGjiChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceDisposal = this.Container.Resolve<IDomainService<Disposal>>();
            var serviceActRemoval = this.Container.Resolve<IDomainService<ActRemoval>>();
            var serviceProtocolArticleLaw = this.Container.Resolve<IDomainService<ProtocolArticleLaw>>();

            try
            {
                var prescriptionViolationsQuery = serviceInspectionGjiViolStage.GetAll()
                .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                .Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.Prescription)
                .Where(x => x.InspectionViolation.RealityObject != null)
                .GroupBy(x => x.Document.Id)
                .Select(x => new
                {
                    Id = x.Key,
                    finalDatePlanRemoval = x.Max(y => y.DatePlanRemoval),
                    initialDatePlanRemoval = x.Min(y => y.DatePlanRemoval),
                    violationsCount = x.Count(),
                    realityObjectId = x.Max(y => y.InspectionViolation.RealityObject.Id)
                })
                .Where(x => x.finalDatePlanRemoval >= this.dateStart && x.finalDatePlanRemoval <= this.dateEnd);

            var prescriptions = prescriptionViolationsQuery
                .Select(x => new
                {
                    x.Id,
                    x.finalDatePlanRemoval,
                    x.initialDatePlanRemoval,
                    x.violationsCount,
                    x.realityObjectId
                })
                .ToList();

            var prescriptionIds = prescriptions.Select(x => x.Id).ToList();

            var realityObjectIds = prescriptions.Select(x => x.realityObjectId).Distinct().ToList();

            var realtyObjectList = new List<RealtyObjectProxy>();
            var start = 0;

            while (start < realityObjectIds.Count)
            {
                var tmpIds = realityObjectIds.Skip(start).Take(1000).ToArray();
                realtyObjectList.AddRange(
                    serviceRealityObject.GetAll()
                    .Where(x => tmpIds.Contains(x.Id))
                    .Select(x => new RealtyObjectProxy
                    {
                        Id = x.Id,
                        Address = x.Address,
                        MunicipalityName = x.Municipality.Name
                    }));

                start += 1000;
            }

            var realtyObjectDict = realtyObjectList.ToDictionary(x => x.Id);

            var prescriptionDataList = new List<PrescriptionProxy1>();
            var prescriptionInspectorsList = new List<PrescriptionInspectorsProxy>();
            var disposalPrescriptionList = new List<DocumentGjiChildrenProxy>();
            var actRemovalPrescriptionList = new List<DocumentGjiChildrenProxy>();
            var prescripCancelIds = new List<long>();

            start = 0;

            while (start < prescriptionIds.Count)
            {
                var tmpIds = prescriptionIds.Skip(start).Take(1000).ToArray();
                prescriptionDataList.AddRange(servicePrescription.GetAll()
                    .Where(x => tmpIds.Contains(x.Id))
                    .Select(x => new PrescriptionProxy1
                    {
                        Id = x.Id,
                        DocumentDate = x.DocumentDate,
                        DocumentNumber = x.DocumentNumber,
                        executant = x.Executant.Name ?? string.Empty,
                        contractor = x.Contragent.Name ?? x.PhysicalPerson
                    }));

                prescriptionInspectorsList.AddRange(serviceDocumentGjiInspector.GetAll()
                    .Where(x => tmpIds.Contains(x.DocumentGji.Id))
                    .Select(x => new PrescriptionInspectorsProxy
                    {
                        Id = x.DocumentGji.Id,
                        Fio = x.Inspector.Fio
                    }));

                disposalPrescriptionList.AddRange(serviceDocumentGjiChildren.GetAll()
                    .Where(x => tmpIds.Contains(x.Parent.Id))
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Disposal)
                    .Select(x => new { parentId = x.Parent.Id, childId = x.Children.Id })
                    .AsEnumerable()
                    .Select(x => new DocumentGjiChildrenProxy { parentId = x.parentId, childId = x.childId }));

                actRemovalPrescriptionList.AddRange(serviceDocumentGjiChildren.GetAll()
                    .Where(x => tmpIds.Contains(x.Parent.Id))
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                    .Select(x => new { parentId = x.Parent.Id, childId = x.Children.Id })
                    .AsEnumerable()
                    .Select(x => new DocumentGjiChildrenProxy { parentId = x.parentId, childId = x.childId }));

                prescripCancelIds.AddRange(
                    servicePrescriptionCancel.GetAll()
                                             .Where(x => tmpIds.Contains(x.Prescription.Id))
                                             .Select(x => x.Prescription.Id)
                                             .Distinct());

                start += 1000;
            }

            var prescriptionDataDict = prescriptionDataList.ToDictionary(x => x.Id);

            var prescriptionInspectors = prescriptionInspectorsList.GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Fio).Aggregate((curr, next) => curr + ", " + next));

            var disposalByPrescriptionDict = disposalPrescriptionList
                .GroupBy(x => x.parentId)
                .ToDictionary(x => x.Key, x => x.First().childId);

            var actRemovalByPrescriptionDict = actRemovalPrescriptionList
                .GroupBy(x => x.parentId)
                .ToDictionary(x => x.Key, x => x.First().childId);

            var disposalIds = disposalPrescriptionList.Select(x => x.childId).Distinct().ToList();

            var disposalsList = new List<DisposalProxy1>();

            start = 0;

            while (start < disposalIds.Count)
            {
                var tmpIds = disposalIds.Skip(start).Take(1000).ToArray();

                disposalsList.AddRange(serviceDisposal.GetAll()
                .Where(x => tmpIds.Contains(x.Id))
                .Select(x => new DisposalProxy1
                {
                    Id = x.Id,
                    DocumentNumber = x.DocumentNumber,
                    DocumentDate = x.DocumentDate,
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd
                }));

                start += 1000;
            }

            var disposalsDict = disposalsList.ToDictionary(x => x.Id);

            var actRemovalIds = actRemovalPrescriptionList.Select(x => x.childId).Distinct().ToList();

            start = 0;

            var actRemovalList = new List<ActRemovalProxy1>();
            var protocolList = new List<ActRemovalProtocolProxy>();
            var prescriptionsList = new List<ActRemovalProxy1>();
            var articleLawList = new List<ProtocolArticleLawProxy>();
            var actRemovalArticleLawList = new List<ActRemovalArticleLawProxy>();
            while (start < actRemovalIds.Count)
            {
                var tmpIds = actRemovalIds.Skip(start).Take(1000).ToArray();

                actRemovalList.AddRange(serviceActRemoval.GetAll()
                    .Where(x => tmpIds.Contains(x.Id))
                    .Select(x => new ActRemovalProxy1
                    {
                        Id = x.Id,
                        DocumentDate = x.DocumentDate,
                        TypeRemoval = x.TypeRemoval
                    }));

                // получаем связку, "Акт устранения нарушений" протокол и дата 
                protocolList.AddRange(serviceDocumentGjiChildren.GetAll()
                        .Where(x => tmpIds.Contains(x.Parent.Id))
                        .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                        .Select(x => new ActRemovalProtocolProxy
                        {
                            ActRemovalId = x.Parent.Id,
                            ProtocolId = x.Children.Id,
                            DocumentDate = x.Children.DocumentDate
                        }));

                var protocolIdDic1 = protocolList.ToDictionary(x => x.ProtocolId);

                // получаем протоколы, которые содержат статью "ст.19.5 ч.24 ЖК РФ"
                articleLawList.AddRange(serviceProtocolArticleLaw.GetAll()
                        .Where(x => protocolIdDic1.Keys.Contains(x.Protocol.Id))
                        .Where(x => x.ArticleLaw.Name == "ст.19.5 ч.24 ЖК РФ")
                        .Select(x => new ProtocolArticleLawProxy
                        {
                            ProtocolId = x.Protocol.Id,
                            ArticleLaw = x.ArticleLaw.Name

                        }));

                var articleLawDic = articleLawList.GroupBy(x => x.ProtocolId).ToDictionary(x => x.Key, y => y.First());

                // получаем связку, "Акт устранения нарушений" и дату  
                actRemovalArticleLawList.AddRange(serviceProtocolArticleLaw.GetAll()
                        .Where(x => articleLawDic.Keys.Contains(x.Protocol.Id))
                        .AsEnumerable()
                        .Select(
                            x => new ActRemovalArticleLawProxy
                            {
                                ActRemovalId = protocolIdDic1.ContainsKey(x.Protocol.Id) ? protocolIdDic1[x.Protocol.Id].ActRemovalId : 0,
                                DocumentDate = protocolIdDic1.ContainsKey(x.Protocol.Id) ? protocolIdDic1[x.Protocol.Id].DocumentDate : null
                            }));

                prescriptionsList.AddRange(serviceDocumentGjiChildren.GetAll()
                    .Where(x => tmpIds.Contains(x.Parent.Id))
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                    .GroupBy(x => x.Parent.Id)
                    .Select(x => new ActRemovalProxy1 { Id = x.Key, DocumentDate = x.Min(y => y.Children.DocumentDate) }));

                start += 1000;
            }

            var actRemovalDict = actRemovalList.GroupBy(x => x.Id).ToDictionary(x => x.Key, y => y.First());
            var prescriptionsDict = prescriptionsList.ToDictionary(x => x.Id);
            var actRemovalDateArticleLaw = actRemovalArticleLawList.GroupBy(x => x.ActRemovalId).ToDictionary(x => x.Key, y => y.First());

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияОсновная");

            foreach (var prescription in prescriptions
                .Where(x => realtyObjectDict.ContainsKey(x.realityObjectId))
                .OrderBy(x => realtyObjectDict[x.realityObjectId].MunicipalityName)
                .ThenBy(x => realtyObjectDict[x.realityObjectId].Address))
            {
                section.ДобавитьСтроку();
                section["Инспектор"] = prescriptionInspectors.ContainsKey(prescription.Id) ? prescriptionInspectors[prescription.Id] : string.Empty;
                section["НачальныйСрокИсполнения"] = prescription.initialDatePlanRemoval.HasValue ? prescription.initialDatePlanRemoval.Value.ToShortDateString() : string.Empty;
                section["КонечныйСрокИсполнения"] = prescription.finalDatePlanRemoval.HasValue ? prescription.finalDatePlanRemoval.Value.ToShortDateString() : string.Empty;
                section["КолвоНарушений"] = prescription.violationsCount;

                if (realtyObjectDict.ContainsKey(prescription.realityObjectId))
                {
                    var realtyObjectData = realtyObjectDict[prescription.realityObjectId];

                    section["МО"] = realtyObjectData.MunicipalityName;
                    section["Адрес"] = realtyObjectData.Address;
                }

                if (prescriptionDataDict.ContainsKey(prescription.Id))
                {
                    var prescriptionData = prescriptionDataDict[prescription.Id];

                    section["Номер"] = prescriptionData.DocumentNumber;
                    section["Дата"] = prescriptionData.DocumentDate.HasValue ? prescriptionData.DocumentDate.Value.ToShortDateString() : string.Empty;
                    section["ТипКонтрагента"] = prescriptionData.executant;
                    section["ЮрФизЛицо"] = prescriptionData.contractor;
                }

                if (prescripCancelIds.Contains(prescription.Id))
                {
                    section["Распоряжение"] = "-";
                    section["ДатаРаспоряжения"] = "-";
                    section["ДатаНачалаПП"] = "-";
                    section["ДатаОкончанияПП"] = "-";
                    section["Акт"] = "-";
                    section["ВыявленыНарушения"] = "-";
                    section["Протокол"] = "-";
                    section["Предписание"] = "-";
                }

                if (!disposalByPrescriptionDict.ContainsKey(prescription.Id))
                {
                    continue;
                }

                var currentDisposalId = disposalByPrescriptionDict[prescription.Id];

                if (!disposalsDict.ContainsKey(currentDisposalId))
                {
                    continue;
                }
                var currentDisposal = disposalsDict[currentDisposalId];

                if (!prescripCancelIds.Contains(prescription.Id))
                {
                    section["Распоряжение"] = currentDisposal.DocumentNumber;
                    section["ДатаРаспоряжения"] = currentDisposal.DocumentDate.HasValue ? currentDisposal.DocumentDate.Value.ToShortDateString() : string.Empty;
                    section["ДатаНачалаПП"] = currentDisposal.DateStart.HasValue
                                                  ? currentDisposal.DateStart.Value.ToShortDateString()
                                                  : string.Empty;
                    section["ДатаОкончанияПП"] = currentDisposal.DateEnd.HasValue
                                                     ? currentDisposal.DateEnd.Value.ToShortDateString()
                                                     : string.Empty;
                }
                if (!actRemovalByPrescriptionDict.ContainsKey(prescription.Id))
                {
                    continue;
                }

                var currentActRemovalId = actRemovalByPrescriptionDict[prescription.Id];

                if (!actRemovalDict.ContainsKey(currentActRemovalId))
                {
                    continue;
                }

                var currentActRemoval = actRemovalDict[currentActRemovalId];

                if (!prescripCancelIds.Contains(prescription.Id))
                {
                    section["Акт"] = currentActRemoval.DocumentDate.HasValue
                                         ? currentActRemoval.DocumentDate.Value.ToShortDateString()
                                         : string.Empty;

                    if (currentActRemoval.TypeRemoval == YesNoNotSet.Yes)
                    {
                        section["ВыявленыНарушения"] = "Нет";
                    }
                    else if (currentActRemoval.TypeRemoval == YesNoNotSet.No)
                    {
                        section["ВыявленыНарушения"] = "Да";

                        DateTime? articleText = null;
                        var protocolProxy = actRemovalDateArticleLaw.Get(currentActRemovalId);

                        if (protocolProxy != null)
                        {
                            articleText = protocolProxy.DocumentDate;
                        }

                        section["Протокол"] = articleText;

                        section["Предписание"] = prescriptionsDict.ContainsKey(currentActRemoval.Id)
                            ? (prescriptionsDict[currentActRemoval.Id].DocumentDate.HasValue ? prescriptionsDict[currentActRemoval.Id].DocumentDate.Value.ToShortDateString() : string.Empty)
                            : string.Empty;
                    }
                }
            }
            }
            finally
            {
                this.Container.Release(serviceInspectionGjiViolStage);
                this.Container.Release(serviceRealityObject);
                this.Container.Release(servicePrescription);
                this.Container.Release(servicePrescriptionCancel);
                this.Container.Release(serviceDocumentGjiInspector);
                this.Container.Release(serviceDocumentGjiChildren);
                this.Container.Release(serviceDisposal);
                this.Container.Release(serviceActRemoval);
                this.Container.Release(serviceProtocolArticleLaw);
            }
        }
    }

    internal sealed class RealtyObjectProxy
    {
        public long Id;
        public string Address;
        public string MunicipalityName;
    }

    internal sealed class PrescriptionProxy1
    {
        public long Id;
        public DateTime? DocumentDate;
        public string DocumentNumber;
        public string executant;
        public string contractor;
    }

    internal sealed class DisposalProxy1
    {
        public long Id;
        public DateTime? DocumentDate;
        public string DocumentNumber;
        public DateTime? DateStart;
        public DateTime? DateEnd;
    }

    internal sealed class PrescriptionInspectorsProxy
    {
        public long Id;
        public string Fio;
    }

    internal struct DocumentGjiChildrenProxy
    {
        public long parentId;
        public long childId;
    }

    internal sealed class ActRemovalProxy1
    {
        public long Id;
        public DateTime? DocumentDate;
        public YesNoNotSet TypeRemoval;
    }

    internal sealed class ActRemovalProtocolProxy
    {
        public long ActRemovalId;
        public long ProtocolId;
        public DateTime? DocumentDate;
    }

    internal sealed class ProtocolArticleLawProxy
    {
        public long ProtocolId;
        public string ArticleLaw;
    }

    internal sealed class ActRemovalArticleLawProxy
    {
        public long ActRemovalId;
        public DateTime? DocumentDate;
    }

}
