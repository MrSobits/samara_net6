namespace Bars.GkhGji.Regions.Stavropol.Report.Notification
{
	using Bars.B4;
	using Bars.B4.DataAccess;
	using Bars.B4.IoC;
	using B4.Modules.Reports;
	using Bars.B4.Utils;
	using Bars.Gkh.Entities;
	using Bars.Gkh.Report;
	using Bars.Gkh.Utils;
	using Bars.GkhGji.Entities;
	using Bars.GkhGji.Enums;
	using Bars.GkhGji.Report;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;

	using Bars.B4.Modules.Analytics.Reports.Enums;

	public class ActCheckNotificationStimulReport : GjiBaseStimulReport
    {
        public ActCheckNotificationStimulReport()
			: base(new ReportTemplateBinary(Properties.Resources.Notification_V_ST))
        {
        }

		#region Properties

		public override string Id
        {
            get { return "ActCheckNotificationReport"; }
        }

        public override string CodeForm
        {
            get { return "ActCheck"; }
        }

        public override string Name
        {
            get { return "Уведомление о составлении протокола"; }
        }

        public override string Description
        {
            get { return "Уведомление о составлении протокола (акт проверки)"; }
        }

		public override StiExportFormat ExportFormat
		{
			get { return StiExportFormat.Word2007; }
		}

		protected override string CodeTemplate { get; set; }

		#endregion

		#region Fields

		private long _documentId;
		private ActCheck _actCheck;
		private Disposal _disposal;
		private Protocol _protocol;

		#endregion

		public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _documentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
			_actCheck = Container.Resolve<IDomainService<ActCheck>>().Get(_documentId);
			if (_actCheck == null)
			{
				throw new ReportProviderException("Не удалось получить акт проверки");
			}

			_disposal = GetParentDocument(_actCheck, TypeDocumentGji.Disposal) as Disposal;
			_protocol = GetChildDocument(_actCheck, TypeDocumentGji.Protocol) as Protocol;
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "Notification_V_ST",
                            Name = "Notification_V_ST",
                            Description = "Уведомление о составлении протокола",
                            Template = Properties.Resources.Notification_V_ST
                        }
                };
        }

		public override Stream GetTemplate()
		{
			CodeTemplate = "Notification_V_ST";
			return base.GetTemplate();
		}

		public override void PrepareReport(ReportParams reportParams)
		{
            FillCommonFields(_actCheck);

			// зональную инспекцию получаем через муниципальное образование первого дома
			var insGjiRoDomain = Container.ResolveDomain<InspectionGjiRealityObject>();
			using (Container.Using(insGjiRoDomain))
			{
				var realityObjs = insGjiRoDomain.GetAll()
					.Where(x => x.Inspection.Id == _actCheck.Inspection.Id)
					.Select(x => x.RealityObject)
					.ToArray();

				var firstRo = realityObjs.FirstOrDefault();
				if (firstRo != null)
				{
					var zonalInsDomain = Container.ResolveDomain<ZonalInspectionMunicipality>();
					using (Container.Using(zonalInsDomain))
					{
						var zonalInspection = zonalInsDomain.GetAll()
							.Where(x => x.Municipality.Id == firstRo.Municipality.Id)
							.Select(x => x.ZonalInspection)
							.FirstOrDefault();

						if (zonalInspection != null)
						{
							this.ReportParams["Адрес1ГосЯзык"] = zonalInspection.Address;
							this.ReportParams["Телефон"] = zonalInspection.Phone;
						}
					}
				}
			}

			if (_disposal != null)
			{
				this.ReportParams["ДатаРаспоряжения"] = _disposal.DocumentDate.HasValue
					? _disposal.DocumentDate.Value.ToString("«dd» MMMM yyyy", new CultureInfo("ru-RU"))
					: string.Empty;
			}

			var contragent = _actCheck.Inspection.Contragent;
			if (contragent != null)
			{
				this.ReportParams["ИНН"] = contragent.Inn;
				this.ReportParams["УправОрг"] = contragent.Name;
				this.ReportParams["УправОргСокр"] = contragent.ShortName;
				this.ReportParams["ЮрАдрес"] = contragent.FiasJuridicalAddress != null
					? contragent.FiasJuridicalAddress.AddressName
					: contragent.JuridicalAddress;

				this.ReportParams["ФактАдрес"] = contragent.FiasFactAddress != null
					? contragent.FiasFactAddress.AddressName
					: contragent.FactAddress;
			}

			if (_protocol != null)
			{
				var protocolDefinitionDomain = Container.ResolveDomain<ProtocolDefinition>();
				using (Container.Using(protocolDefinitionDomain))
				{
					var protocolDefinition =
						protocolDefinitionDomain.GetAll()
							.FirstOrDefault(
								x => x.Protocol.Id == _protocol.Id && x.TypeDefinition == TypeDefinitionProtocol.TimeAndPlaceHearing);
					if (protocolDefinition != null)
					{
						this.ReportParams["ДатаНазнач"] = protocolDefinition.DocumentDate.HasValue
							? protocolDefinition.DocumentDate.Value.ToString("«dd» MMMM yyyy", new CultureInfo("ru-RU"))
							: string.Empty;

						this.ReportParams["ВремНазнач"] = protocolDefinition.TimeDefinition.HasValue
							? protocolDefinition.TimeDefinition.Value.ToString("HH:mm", new CultureInfo("ru-RU"))
							: string.Empty;
					}
				}
			}

			var insDomain = Container.ResolveDomain<DocumentGjiInspector>();
			var inspector = insDomain.GetAll()
				.Where(x => x.DocumentGji.Id == _disposal.Id)
				.Select(x => x.Inspector)
				.FirstOrDefault();

			if (inspector != null)
			{
				this.ReportParams["Должность"] = inspector.Position;
				this.ReportParams["Исполнитель"] = inspector.FioAblative;
				this.ReportParams["ТелефонИнспектора"] = inspector.Phone;
			}
		}
    }
}