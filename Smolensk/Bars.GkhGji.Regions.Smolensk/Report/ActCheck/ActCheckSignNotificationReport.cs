using Slepov.Russian.Morpher;

namespace Bars.GkhGji.Regions.Smolensk.Report.ProtocolGji
{
    using System.Collections.Generic;

    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    using Gkh.Report;

    using GkhGji.Entities;

    using System.IO;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    /// <summary> Уведомление  на подписание акта и протокола по результатам проверки </summary>
    public class ActCheckSignNotificationReport : GjiBaseStimulReport
    {
        #region .ctor

        public ActCheckSignNotificationReport()
            : base(new ReportTemplateBinary(Properties.Resources.ActCheckSignNotification))
        {

        }

        #endregion .ctor

        #region Properties

        public override string Id
        {
            get { return "ActCheckSignNotification"; }
        }

        public override string Name
        {
            get { return "Уведомление на подписание акта"; }
        }

        public override string Description
        {
            get { return "Уведомление  на подписание акта и протокола по результатам проверки"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string CodeForm
        {
            get { return "ActCheck"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        #endregion Properties

        protected long DocumentId;

        protected ActCheckSmol ActCheck;

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            var protocolDomain = Container.ResolveDomain<ActCheckSmol>();

            using (Container.Using(protocolDomain))
            {
                ActCheck = protocolDomain.FirstOrDefault(x => x.Id == DocumentId);
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "ActCheckSignNotification",
                    Name = "ActCheckSignNotification",
                    Description = "Уведомление  на подписание акта и протокола по результатам проверки",
                    Template = Properties.Resources.ActCheckSignNotification
                }
            };
        }

        private void GetCodeTemplate()
        {
            CodeTemplate = "ActCheckSignNotification";
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            if (ActCheck == null)
            {
                return;
            }

            FillCommonFields(ActCheck);
            FillActCheckData();
        }

	    private void FillActCheckData()
	    {
		    var childProtocol = GetChildDocument(ActCheck, TypeDocumentGji.Protocol) as Protocol;
		    var parentDisposal = GetParentDocument(ActCheck, TypeDocumentGji.Disposal) as Disposal;

			var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

		    this.ReportParams["УведомлениеДата"] = ActCheck.DateNotification.HasValue
			    ? ActCheck.DateNotification.Value.ToShortDateString()
			    : string.Empty;

		    this.ReportParams["УведомлениеНомер"] = ActCheck.Return(x => x.NumberNotification, string.Empty);
		    this.ReportParams["НомерАкта"] = ActCheck.Return(x => x.DocumentNumber, string.Empty);
		    this.ReportParams["ДатаАкта"] = ActCheck.DocumentDate.HasValue
			    ? ActCheck.DocumentDate.Value.ToShortDateString()
			    : string.Empty;
            this.ReportParams["ОбъектПроверки"] = ActCheck.Inspection.PersonInspection.GetEnumMeta().Display;

		    if (childProtocol != null)
		    {
                //Report["ЮрЛицо"] = childProtocol.Return(x => x.Contragent).Return(x => x.Name, string.Empty);
                //Report["ЮрЛицоСокр"] = childProtocol.Return(x => x.Contragent).Return(x => x.ShortName, string.Empty);
                //Report["АдресЮл"] = childProtocol.Return(x => x.Contragent).Return(x => x.JuridicalAddress, string.Empty);
                //Report["ТелефонЮл"] = childProtocol.Return(x => x.Contragent).Return(x => x.Phone, string.Empty);
			    this.ReportParams["ТипИсполнителя"] = childProtocol.Return(x => x.Executant).Return(x => x.Code, string.Empty);
		    }

            if (ActCheck.Inspection.Contragent != null)
            {
                var inspectionContragent = ActCheck.Inspection.Contragent;
                this.ReportParams["ЮрЛицо"] = inspectionContragent.Return(x => x.Name, string.Empty);
                this.ReportParams["ЮрЛицоСокр"] = inspectionContragent.Return(x => x.ShortName, string.Empty);
                this.ReportParams["АдресЮл"] = inspectionContragent.Return(x => x.JuridicalAddress, string.Empty);
                this.ReportParams["ТелефонЮл"] = inspectionContragent.Return(x => x.Phone, string.Empty);
            }

		    if (parentDisposal != null)
		    {
				var kindCheck = parentDisposal.Return(x => x.KindCheck).Return(x => x.Name, string.Empty);

				this.ReportParams["ВидПроверки"] = kindCheck;
				this.ReportParams["ВидПроверкиСкл"] = склонятель.Проанализировать(kindCheck).Дательный.ToLower();

			    this.ReportParams["НомерПриказа"] = parentDisposal.Return(x => x.DocumentNumber, string.Empty);
			    this.ReportParams["ДатаПриказа"] = parentDisposal.DocumentDate.HasValue
				    ? parentDisposal.DocumentDate.Value.ToShortDateString()
				    : string.Empty;
		    }
	    }
    }
}
