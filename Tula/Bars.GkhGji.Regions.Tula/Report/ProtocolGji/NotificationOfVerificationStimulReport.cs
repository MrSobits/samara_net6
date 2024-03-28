namespace Bars.GkhGji.Regions.Tula.Report.ProtocolGji
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using B4.Utils;
    //using Entities.Protocol;
    using Bars.B4.IoC;
    using Gkh.Entities;
    using Gkh.Report;
    using GkhGji.Entities;
    using Stimulsoft.Report;
    using B4;
    using GkhGji.Enums;
    using System.IO;
    using GkhGji.Report;

    /// <summary> Уведомление о проверке </summary>
    public class NotificationOfVerificationStimulReport : GjiBaseStimulReport
    {
        #region .ctor

        public NotificationOfVerificationStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.NotificationOfVerificationStimulReport))
        {

        }

        #endregion .ctor

        #region Properties

        public override string Id
        {
            get { return "NotificationOfVerification"; }
        }

        public override bool PrintingAllowed
        {
            get
            {
                return false;
            }
        }

        public override string Name
        {
            get { return "Уведомление о проверке"; }
        }

        public override string Description
        {
            get { return "Уведомление о проверке (из протокола)"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string CodeForm
        {
            get { return "Protocol"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        #endregion Properties

        protected long DocumentId;

        protected Protocol Protocol;

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            var protocolDomain = Container.ResolveDomain<Protocol>();

            using (Container.Using(protocolDomain))
            {
                Protocol = protocolDomain.FirstOrDefault(x => x.Id == DocumentId);
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "NotificationOfVerificationStimulReport",
                    Name = "NotificationOfVerificationStimulReport",
                    Description = "Уведомление о проверке",
                    Template = Properties.Resources.NotificationOfVerificationStimulReport
                }
            };
        }

        private void GetCodeTemplate()
        {
            CodeTemplate = "NotificationOfVerificationStimulReport";
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            if (Protocol == null)
            {
                return;
            }
            FillCommonFields(Protocol);
            FillProtocolData();
        }
        protected void FillProtocolData()
        {
            var cultureInfo = new CultureInfo("ru-RU");

            if (Protocol.Contragent != null)
            {
                Report["УправОрг"] = Protocol.Contragent.Name;
            }

            if (Protocol.Executant != null)
            {
                var firstExecutantCodeList = new List<string> { "0", "9", "11", "8", "15", "18", "4" };
                var secondExecutantCodeList = new List<string> { "1", "5", "6", "7", "10", "12", "13", "14", "16", "19" };

                var executant = Protocol.Return(x => x.Executant);

                if (firstExecutantCodeList.Contains(executant.Return(x => x.Code)))
                {
                    Report["УправОрг"] = Protocol.Contragent.Name;

                    var headContragent = Container.Resolve<IDomainService<ContragentContact>>().GetAll()
                           .Where(x => x.Contragent.Id == Protocol.Contragent.Id && x.DateStartWork.HasValue
                               && (x.DateStartWork.Value <= DateTime.Now && (!x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Now)))
                           .FirstOrDefault(x => x.Position != null && (x.Position.Code == "1" || x.Position.Code == "4"));
                    if (headContragent != null)
                    {
                        Report["ФИОРукОрг"] = headContragent.Position.Name + " - " + headContragent.FullName;
                    }
                }

                else if (secondExecutantCodeList.Contains(executant.Return(x => x.Code)))
                {
                    Report["УправОрг"] = Protocol.PhysicalPerson;
                }
            }

            if (Protocol.Stage != null && Protocol.Stage.Parent != null)
            {

                var disposal =
                    Container.Resolve<IDomainService<Disposal>>().GetAll()
                        .Where(x => x.Stage.Id == Protocol.Stage.Parent.Id)
                        .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .FirstOrDefault();


                Report["НомерРаспоряжения"] = disposal.DocumentNumber;
                Report["ДатаРаспоряжения"] = disposal.DocumentDate.ToDateTime().ToShortDateString();

                var realityObj = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                    .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                    .Select(x => x.RealityObject)
                    .FirstOrDefault();
                if (realityObj != null && realityObj.FiasAddress != null)
                {
                    Report["НасПункт"] = realityObj.FiasAddress.PlaceName;
                    Report["Дом"] = realityObj.FiasAddress.House;
                    Report["Улица"] = realityObj.FiasAddress.StreetName;
                }

                Report["Начальник"] = disposal.IssuedDisposal != null
                    ? disposal.IssuedDisposal.ShortFio
                    : string.Empty;
                if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji)
                {
                    Report["Цель"] = "Проверка исполнения предписания";

                    var basePrescription = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                        .Where(
                            x =>
                                x.Children.Id == disposal.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                        .Select(x => x.Parent)
                        .FirstOrDefault();

                    if (basePrescription != null)
                    {
                        Report["ОснованиеОбследования"] =
                            string.Format(
                                "уведомление о проведении внеплановой выездной проверки с целью проверки исполнения предписания № {0} от {1}",
                                basePrescription.DocumentNumber,
                                basePrescription.DocumentDate.HasValue
                                    ? basePrescription.DocumentDate.Value.ToString("D", cultureInfo)
                                    : string.Empty);
                    }
                }

                var inspectors = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                    .Where(x => x.DocumentGji.Id == disposal.Id)
                    .Select(x => x.Inspector)
                    .ToArray();

                if (inspectors.Any())
                {
                    Report["Исполнитель"] = inspectors.FirstOrDefault().FioAblative;
                }
            }
        }
    }
}
