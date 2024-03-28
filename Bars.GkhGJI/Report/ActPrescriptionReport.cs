namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class ActPrescriptionReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomin { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private long[] municipalityIds;

        public ActPrescriptionReport()
            : base(new ReportTemplateBinary(Properties.Resources.ActResponsePrescription))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.ActPrescription";
            }
        }

        public override string Desciption
        {
            get { return "Реестр предписаний"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ActPrescription"; }
        }

        public override string Name
        {
            get { return "Реестр предписаний"; }
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
            //Предписание которые содержат записи в "Решении об отмен"
            var servicePrescriptionCancel = Container.Resolve<IDomainService<PrescriptionCancel>>().GetAll()
                                                     .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                                                     .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                                                     .Select(x => x.Prescription.Id);
            // Нарушения в предписании
            var servicePrescriptionViol =
                Container.Resolve<IDomainService<PrescriptionViol>>()
                         .GetAll()
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                         .WhereIf(dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= dateStart)
                         .WhereIf(dateEnd != DateTime.MinValue, x => x.Document.DocumentDate <= dateEnd)
                         .Where(x => !servicePrescriptionCancel.Contains(x.Document.Id));


            // Выбираем нужные поля
            var violations = servicePrescriptionViol
                                      .Select(x => new
                                      {
                                          x.InspectionViolation.Id,
                                          DocumentId = x.Document.Id,
                                          MunicipalityName = x.InspectionViolation.RealityObject.Municipality.Name,
                                          x.InspectionViolation.RealityObject.Address,
                                          x.Document.DocumentNumber,
                                          x.Document.DocumentDate,
                                          x.InspectionViolation.Violation.CodePin,
                                          x.InspectionViolation.Violation.Description,
                                          x.DatePlanRemoval,
                                          x.InspectionViolation.DateFactRemoval
                                      })
                                      .OrderBy(x => x.MunicipalityName)
                                      .ToList();

            // Запрос Id документов
            var prescriptionIDsQuery = servicePrescriptionViol.Select(x => x.Document.Id);
            // Запрос Id этапа нарушения
            var violationIDsQuery = servicePrescriptionViol.Select(x => x.InspectionViolation.Id);
            // Список предписаний
            var prescriptionsList = Container.Resolve<IDomainService<Prescription>>().GetAll()
                .Where(x => prescriptionIDsQuery.Contains(x.Id))
                .Select(x => new { x.Id, contrName = x.Contragent.Name, executantCode = x.Executant.Code })
                .ToList();

            // Делаем словарь где ключем является Id предписания
            var prescriptions = prescriptionsList.ToDictionary(x => x.Id, x => new { x.contrName, x.executantCode });

            var queryProtocolArticleLaw = Container.Resolve<IDomainService<ProtocolArticleLaw>>().GetAll()
                                                    .Where(x => x.ArticleLaw.Code == "1" || x.ArticleLaw.Code == "7")
                                                    .Select(x => x.Protocol.Id);

            var prescriptionWithProtocol = DocumentGjiChildrenDomin.GetAll()
                .Join(DocumentGjiChildrenDomin.GetAll(),
                x => x.Children.Id,
                y => y.Parent.Id,
                (x, y) => new { PrecsrAct = x, ActProtocol = y })
                .Where(x => x.PrecsrAct.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                .Where(x => x.PrecsrAct.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                .Where(x => prescriptionIDsQuery.Contains(x.PrecsrAct.Parent.Id))
                .Where(x => x.ActProtocol.Parent.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                .Where(x => x.ActProtocol.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                .Where(x => queryProtocolArticleLaw.Contains(x.ActProtocol.Children.Id))
                .Select(x => new
                {
                    prescriptionId = x.PrecsrAct.Parent.Id,
                    protocolId = x.ActProtocol.Children.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.prescriptionId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.protocolId).ToList());

            var protocolsByInspectionViolationId = Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
                .Where(x => violationIDsQuery.Contains(x.InspectionViolation.Id))
                .Select(x => new { x.InspectionViolation.Id, x.Document.DocumentNumber, x.Document.DocumentDate, DocumentId = x.Document.Id })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.ToList());

            var prescriptionActRemovalsDict = DocumentGjiChildrenDomin.GetAll()
                .Where(x => prescriptionIDsQuery.Contains(x.Parent.Id))
                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                .Select(x => new
                {
                    x.Parent.Id,
                    x.Children.DocumentNumber,
                    x.Children.DocumentDate
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => new { y.DocumentNumber, y.DocumentDate }).First());


            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var counter = 0;
            // Код исполнителя
            var contragentTypes1 = new List<string> { "0", "9", "11", "8", "15", "18", "4" };
            var contragentTypes2 = new List<string> { "1", "10", "12", "13", "16", "19", "5" };

            foreach (var violation in violations)
            {
                section.ДобавитьСтроку();
                section["Number1"] = ++counter;
                section["Municipality"] = violation.MunicipalityName;
                section["Address"] = violation.Address;
                section["DocNumber"] = violation.DocumentNumber ?? string.Empty;
                section["DocDate"] = violation.DocumentDate.HasValue
                                         ? violation.DocumentDate.Value.ToShortDateString()
                                         : string.Empty;

                // Если в словаре предписаний, Исполнитель и Контрагент не пустые идет проверка на код исполнителя
                if (prescriptions[violation.DocumentId].executantCode != null && prescriptions[violation.DocumentId].contrName != null)
                {

                    if (contragentTypes1.Contains(prescriptions[violation.DocumentId].executantCode))
                    {
                        section["Organization"] = prescriptions[violation.DocumentId].contrName;
                    }

                    if (contragentTypes2.Contains(prescriptions[violation.DocumentId].executantCode))
                    {
                        section["Organization"] = string.Format("Руководителю {0}", prescriptions[violation.DocumentId].contrName);
                    }
                }

                section["Violation"] = string.Format("{0} {1}", violation.CodePin ?? string.Empty, violation.Description ?? string.Empty);
                section["DateRemoval"] = violation.DatePlanRemoval.HasValue
                                             ? violation.DatePlanRemoval.Value.ToShortDateString()
                                             : string.Empty;

                if (protocolsByInspectionViolationId.ContainsKey(violation.Id) && prescriptionWithProtocol.ContainsKey(violation.DocumentId))
                {
                    var protocolIds = prescriptionWithProtocol[violation.DocumentId];
                    var firstOrDefault = protocolsByInspectionViolationId[violation.Id].FirstOrDefault(x => protocolIds.Contains(x.DocumentId));
                    if (firstOrDefault != null)
                        section["Execution"] = string.Format(
                            "протокол за неисполнение предписания №{0} от {1}",
                            firstOrDefault.DocumentNumber,
                            firstOrDefault.DocumentDate.GetValueOrDefault().ToShortDateString());
                }
                else if (violation.DateFactRemoval != null && prescriptionActRemovalsDict.ContainsKey(violation.DocumentId))
                {
                    var actRemovalInfo = prescriptionActRemovalsDict[violation.DocumentId];

                    section["Execution"] =
                        string.Format(
                            "акт исполнения предписания (проверки исполнения предписания) №{0} от {1}",
                            actRemovalInfo.DocumentNumber ?? string.Empty,
                            actRemovalInfo.DocumentDate.HasValue ? actRemovalInfo.DocumentDate.Value.ToShortDateString() : string.Empty);
                }
            }
        }
    }
}