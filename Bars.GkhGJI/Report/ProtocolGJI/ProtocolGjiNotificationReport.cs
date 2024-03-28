namespace Bars.GkhGji.Report
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ProtocolGjiNotificationReport : GjiBaseReport
    {
        private long DefinitionId { get; set; }

        protected override string CodeTemplate { get; set; }

        public ProtocolGjiNotificationReport() : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_Notification_RD))
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
                            Name = "Protocol_Notification",
                            Code = "BlockGJI_Notification_RD",
                            Template = Properties.Resources.BlockGJI_Notification_RD
                        }
                };
        }

        public override string Id
        {
            get { return "ProtocolNotification"; }
        }

        public override string CodeForm
        {
            get { return "ProtocolDefinition"; }
        }

        public override string Name
        {
            get { return "Уведомление о рассмотрении дела"; }
        }

        public override string Description
        {
            get { return "Уведомление о рассмотрении дела"; }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var definition = Container.Resolve<IDomainService<ProtocolDefinition>>().Get(DefinitionId);

            var cultureInfo = CultureInfo.GetCultureInfo("ru-RU");

            if (definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }

            var protocol = definition.Protocol;

            // Заполняем общие поля
            FillCommonFields(reportParams, protocol);

            if (protocol.Inspection.Contragent != null)
            {
                reportParams.SimpleReportParams["УправОрг"] = protocol.Inspection.Contragent.Name;

                if (protocol.Inspection.Contragent.FiasFactAddress != null)
                {
                    reportParams.SimpleReportParams["АдресКонтрагентаФакт"] =
                        protocol.Inspection.Contragent.FiasFactAddress.AddressName;
                }
            }

            reportParams.SimpleReportParams["ДатаПротокола"] =
                protocol.DocumentDate.HasValue
                    ? protocol.DocumentDate.Value.ToString("dd MMMM yyyy г.", cultureInfo)
                    : null;
            
            reportParams.SimpleReportParams["Номер"] = protocol.DocumentNumber;

            var listRoAdrProtocol =
                Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
                    .Where(x => x.Document.Id == protocol.Id)
                    .Select(x => x.InspectionViolation.RealityObject.FiasAddress.AddressName)
                    .Distinct()
                    .ToList();

            reportParams.SimpleReportParams["ДомАдресПротокола"] = string.Join("; ", listRoAdrProtocol);

            var quryAppeals = Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                        .Where(x => x.Inspection.Id == protocol.Inspection.Id);

            var appeals = quryAppeals
                .Select(x => new
                {
                    appealId = x.AppealCits.Id,
                    x.AppealCits.Correspondent,
                    x.AppealCits.DocumentNumber,
                    x.AppealCits.DateFrom
                })
                .ToList();

            var quryAppealsId = quryAppeals.Select(x => x.AppealCits.Id);

            var appealsRo =
                Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll()
                    .Where(x => quryAppealsId.Contains(x.AppealCits.Id))
                    .Select(x => new
                    {
                        appId = x.AppealCits.Id,
                        muName = x.RealityObject.Municipality.Name,
                        address = x.RealityObject.Address
                    })
                    .ToList();

            var appealAddress = new StringBuilder();
            foreach (var appeal in appeals)
            {
                var addressRo = string.Empty;
                var appealRo = appealsRo.Where(x => x.appId == appeal.appealId).ToList();
                if (appealRo.Count > 0)
                {
                    var item = appealRo.First();
                    addressRo = string.Format("{0}, {1}", item.muName, item.address);
                }

                var appealsDocNumDateStr = string.Empty;
                if (!string.IsNullOrEmpty(appeal.DocumentNumber))
                {
                    appealsDocNumDateStr = string.Format("{0} от {1}",
                        appeal.DocumentNumber,
                        appeal.DateFrom.HasValue ? appeal.DateFrom.Value.ToShortDateString() : string.Empty);
                }

                if (!string.IsNullOrEmpty(appeal.Correspondent))
                {
                    if (appealAddress.Length > 0) appealAddress.Append("; ");

                    appealAddress.AppendFormat("{0} (вх. {1}), проживающего по адресу {2}", appeal.Correspondent, appealsDocNumDateStr, addressRo);
                }
            }

            reportParams.SimpleReportParams["РеквизитыОбращения"] = appealAddress.ToString();

            if (protocol.Stage != null && protocol.Stage.Parent != null)
            {

                var disposalData =
                    Container.Resolve<IDomainService<Disposal>>().GetAll()
                        .Where(x => x.Stage.Id == protocol.Stage.Parent.Id)
                        .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .Select(x => new
                        {
                            x.Id,
                            x.DocumentDate,
                            x.DocumentNumber,
                            x.TypeDisposal,
                            IssuedPosition = x.IssuedDisposal.Position,
                            IssuedFio = x.IssuedDisposal.ShortFio
                        })
                        .FirstOrDefault();

                if (disposalData != null)
                {
                    reportParams.SimpleReportParams["ДатаРаспоряжения"] = disposalData.DocumentDate.HasValue
                                                                   ? disposalData.DocumentDate.Value.ToString("dd MMMM yyyy г.", cultureInfo)
                                                                   : string.Empty;
                    reportParams.SimpleReportParams["НомерРаспоряжения"] = disposalData.DocumentNumber;

                    reportParams.SimpleReportParams["РуководительДолжность"] = disposalData.IssuedPosition;
                    reportParams.SimpleReportParams["РуководительФИОСокр"] = disposalData.IssuedFio;

                    var queryInspectorId = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                                     .Where(x => x.DocumentGji.Id == disposalData.Id)
                                                     .Select(x => x.Inspector.Id);

                    var listLocality = Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll()
                                        .Where(x => queryInspectorId.Contains(x.Inspector.Id))
                                        .Select(x => x.ZonalInspection.Locality)
                                        .Distinct()
                                        .ToList();

                    reportParams.SimpleReportParams["НаселПунктОтдела"] = string.Join("; ", listLocality);

                    var strPurposeDetails = string.Empty;
                    if (protocol.Inspection.TypeBase == TypeBase.CitizenStatement && disposalData.TypeDisposal != TypeDisposalGji.DocumentGji)
                    {
                        strPurposeDetails = reportParams.SimpleReportParams["РеквизитыОбращения"].ToString();
                    }
                    else
                    {
                        strPurposeDetails = reportParams.SimpleReportParams["ДомАдресПротокола"].ToString();
                    }

                    reportParams.SimpleReportParams["ЦельРеквизиты"] = strPurposeDetails;
                }

                if (disposalData != null && disposalData.DocumentDate.HasValue && protocol.Inspection.Contragent != null)
                {
                    var contact = Container.Resolve<IDomainService<ContragentContact>>().GetAll()
                                 .Where(x => x.Contragent.Id == protocol.Inspection.Contragent.Id)
                                 .Where(x => x.DateEndWork == null || x.DateEndWork <= disposalData.DocumentDate.Value.AddDays(1))
                                 .Where(x => x.DateStartWork == null || x.DateStartWork >= disposalData.DocumentDate.Value)
                                 .Where(x => x.Position.Code == "1" || x.Position.Code == "4")
                                 .Select(x => new { PositionName = x.Position.Name, x.FullName })
                                 .FirstOrDefault();

                    if (contact != null)
                    {
                        reportParams.SimpleReportParams["ФИОРукОрг"] = contact.PositionName + " " + contact.FullName;
                    }
                }

                var actDatа = Container.Resolve<IDomainService<DocumentGji>>()
                         .GetAll()
                         .Where(x => x.Stage.Parent.Id == protocol.Stage.Parent.Id)
                         .Where(x => x.TypeDocumentGji == TypeDocumentGji.ActCheck)
                         .Select(x => new { x.DocumentDate, x.Id, x.DocumentNumber })
                         .FirstOrDefault();
                
                if (actDatа != null)
                {                    
                    var actDate = actDatа.DocumentDate.HasValue ? 
                                                        actDatа.DocumentDate.Value.ToString("dd MMMM yyyy г.", cultureInfo)
                                                        : string.Empty;

                    var actNum = string.Format("{0} от {1}", actDatа.DocumentNumber, actDate);

                    reportParams.SimpleReportParams["ДатаАкта"] = actDate;

                    reportParams.SimpleReportParams["НомерПроверки"] = actNum;
                    reportParams.SimpleReportParams["ДатаПроверки"] = actDate;

                    var listAddressRo = Container.Resolve<IDomainService<ActCheckRealityObject>>()
                                 .GetAll()
                                 .Where(x => x.ActCheck.Id == actDatа.Id)
                                 .Select(x => new {x.RealityObject.FiasAddress.AddressName, x.RealityObject.FiasAddress.PlaceName})
                                 .ToList();

                    var addresses = string.Join("; ", listAddressRo.Select(x => x.AddressName).ToArray());
                    reportParams.SimpleReportParams["ДомаИАдреса"] = addresses;
                }                
            }

            var listInspectionGjiRealityObject = Container.Resolve<IDomainService<InspectionGjiRealityObject>>()
                                                                 .GetAll()
                                                                 .Where(x => x.Inspection.Id == protocol.Inspection.Id)
                                                                 .Select(x => x.RealityObject.FiasAddress.PlaceName)
                                                                 .ToList();

            if (listInspectionGjiRealityObject.Count == 1)
            {
                reportParams.SimpleReportParams["НасПункт"] = listInspectionGjiRealityObject.First();
            }            
        }
    }
}