namespace Bars.GkhGji.Regions.Zabaykalye.Report
{
	using Bars.B4.DataAccess;
	using Bars.B4.IoC;
	using Bars.B4.Modules.Reports;
	using Bars.B4.Utils;
	using Bars.Gkh.Report;
	using Bars.Gkh.Utils;
	using Bars.GkhGji.Entities;
	using Bars.GkhGji.Enums;
	using Bars.GkhGji.Report;
	using System.Collections.Generic;
	using System.Linq;

	using Bars.B4.Modules.Analytics.Reports.Enums;
	using Bars.B4.Modules.Analytics.Reports.Generators.Models;

	/// <summary>
    /// Печать отчета "Ответ на обращение" через Стимул
    /// </summary>
    public class AppealCitsAnswerStimulReport : GjiBaseStimulReport
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
		public AppealCitsAnswerStimulReport()
			: base(new ReportTemplateBinary(Properties.Resources.AppealCitsAnswerStimulReport))
        {
        }

        /// <summary>
        /// Формат печатной формы.
        /// </summary>
        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get
            {
				return "AppealCitsAnswer";
            }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get
            {
				return "AppealCitsAnswer";
            }
        }

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get
            {
				return "Ответ на обращение";
            }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get
            {
				return "Ответ на обращение";
            }
        }

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// ИД документа.
        /// </summary>
        private long AppelCitsId { get; set; }

		/// <summary>
		/// Обращение
		/// </summary>
        private AppealCits AppelCits { get; set; }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues">Пользовательские параметры</param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
			AppelCitsId = userParamsValues.GetValue<object>("AppelCitsId").ToLong();
			
			var appealCitsDomain = Container.ResolveDomain<AppealCits>();
	        using (Container.Using(appealCitsDomain))
	        {
				AppelCits = appealCitsDomain.Get(AppelCitsId);
				if (AppelCits == null)
		        {
					throw new ReportProviderException("Не удалось получить обращение");
		        }
	        }
        }

        /// <summary>
        /// Получить список шаблонов
        /// </summary>
        /// <returns>
        /// Список шаблонов.
        /// </returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Code = "AppealCitsAnswer",
                                   Name = "AppealCitsAnswer",
                                   Description = "Ответ на обращение",
                                   Template = Properties.Resources.AppealCitsAnswerStimulReport
                               }
                       };
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
	        var inspectionAppealCitsDomain = Container.ResolveDomain<InspectionAppealCits>();
			var disposalDomain = Container.ResolveDomain<Disposal>();
			var inspectionGjiRealityObject = Container.ResolveDomain<InspectionGjiRealityObject>();
	        var documentGjiChildrenDomain = Container.ResolveDomain<DocumentGjiChildren>();
	        var actCheckViolationDomain = Container.ResolveDomain<ActCheckViolation>();
	        var prescriptionViolDomain = Container.ResolveDomain<PrescriptionViol>();
			var protocolArticleLawDomain = Container.ResolveDomain<ProtocolArticleLaw>();
			var documentGjiInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();

	        try
	        {
		        var проверки = new List<Проверка>();

		        var baseStatementIds = inspectionAppealCitsDomain.GetAll()
			        .Where(x => x.AppealCits.Id == AppelCitsId)
			        .Select(x => x.Inspection.Id)
			        .ToArray();

		        var disposalDict = disposalDomain.GetAll()
			        .Where(x => baseStatementIds.Contains(x.Inspection.Id) && x.TypeDisposal == TypeDisposalGji.Base)
			        .AsEnumerable()
					.GroupBy(x => x.Inspection.Id)
			        .ToDictionary(x => x.Key, y => y.First());

				var realtyObjDict = inspectionGjiRealityObject.GetAll()
					.Where(x => baseStatementIds.Contains(x.Inspection.Id))
					.Select(x => new { x.Inspection.Id, x.RealityObject })
					.AsEnumerable()
					.GroupBy(x => x.Id)
					.ToDictionary(x => x.Key, x => x.First());

		        var inspectionAppealCitsList = inspectionAppealCitsDomain.GetAll()
			        .Where(x => x.AppealCits.Id == AppelCitsId).ToArray();

				foreach (var inspectionAppealCits in inspectionAppealCitsList)
				{
					var проверка = new Проверка();
					проверка.Корреспондент = AppelCits.Correspondent;
					проверка.АдресКореспондента = AppelCits.CorrespondentAddress;
					проверка.Контрагент = inspectionAppealCits.Inspection.Contragent != null ? inspectionAppealCits.Inspection.Contragent.Name : "";

					var disposal = disposalDict.Get(inspectionAppealCits.Inspection.Id);
					if (disposal != null)
					{
                        FillCommonFields(disposal);
						проверка.НомерПроверки = disposal.DocumentNumber;

						if (disposal.ResponsibleExecution != null)
						{
							проверка.Инспектор = disposal.ResponsibleExecution.Fio;
							проверка.ИнспекторРП = disposal.ResponsibleExecution.FioGenitive;
							проверка.ДолжностьИнспекторРП = disposal.ResponsibleExecution.PositionGenitive;
							проверка.ТелефонИнспектор = disposal.ResponsibleExecution.Phone;
						}

						if (disposal.IssuedDisposal != null)
						{
							проверка.РуководительСокрПриказ = disposal.IssuedDisposal.ShortFio;
							проверка.РуководительДолжностьПриказ = disposal.IssuedDisposal.Position;
						}

						var actCheck = GkhGji.Utils.Utils.GetChildDocumentByType(documentGjiChildrenDomain, disposal, TypeDocumentGji.ActCheck);
						if (actCheck != null)
						{
							проверка.НомерАктаПроверки = actCheck.DocumentNumber;
							проверка.ДатаАктаПроверки = actCheck.DocumentDate.HasValue ? actCheck.DocumentDate.Value.ToString("«dd» MMMM yyyy г.") : "";

							var violations = actCheckViolationDomain.GetAll()
								.Where(x => x.ActObject.Id == actCheck.Id)
								.Select(x => x.InspectionViolation.Violation.Name)
								.AsEnumerable()
								.AggregateWithSeparator("; ");

							проверка.ОписаниеПиН = violations;
						}

						var prescription = GkhGji.Utils.Utils.GetChildDocumentByType(documentGjiChildrenDomain, disposal, TypeDocumentGji.Prescription);
						if (prescription != null)
						{
							проверка.НомерПредписания = prescription.DocumentNumber;

							var realityObject = realtyObjDict.Get(inspectionAppealCits.Inspection.Id);

							var prescriptionViolations = prescriptionViolDomain.GetAll()
								.Where(x => x.Document.Id == prescription.Id)
								.WhereIf(realityObject != null, x => x.InspectionViolation.RealityObject.Id == realityObject.RealityObject.Id)
								.WhereIf(realityObject == null, x => x.InspectionViolation.RealityObject == null)
								.Where(x => x.DatePlanRemoval != null)
								.Select(x => x.DatePlanRemoval.Value)
								.AsEnumerable()
								.AggregateWithSeparator(x => x.ToShortDateString(), ", ");

							проверка.СрокУстраненияПредписания = prescriptionViolations;	
						}

						var protocol = GkhGji.Utils.Utils.GetChildDocumentByType(documentGjiChildrenDomain, disposal, TypeDocumentGji.Protocol);
						if (protocol != null)
						{
							проверка.НомерПротокола = protocol.DocumentNumber;

							var articleLaws = protocolArticleLawDomain.GetAll()
								.Where(x => x.Protocol.Id == protocol.Id)
								.Select(x => x.ArticleLaw.Name)
								.AsEnumerable()
								.AggregateWithSeparator(", ");

							проверка.Статья = articleLaws;
						}
					}

					проверки.Add(проверка);
				}

				this.DataSources.Add(new MetaData
				{
					SourceName = "Проверка",
					MetaType = nameof(Проверка),
					Data = проверки
				});
	        }
	        finally
	        {
				Container.Release(inspectionAppealCitsDomain);
				Container.Release(disposalDomain);
				Container.Release(inspectionGjiRealityObject);
				Container.Release(documentGjiChildrenDomain);
				Container.Release(actCheckViolationDomain);
				Container.Release(prescriptionViolDomain);
				Container.Release(protocolArticleLawDomain);
				Container.Release(documentGjiInspectorDomain);
	        }
        }

		private class Проверка
		{
			public string НомерПроверки { get; set; }
			public string Корреспондент { get; set; }
			public string АдресКореспондента { get; set; }
			public string Контрагент { get; set; }
			public string НомерАктаПроверки { get; set; }
			public string ДатаАктаПроверки { get; set; }
			public string ОписаниеПиН { get; set; }
			public string НомерПредписания { get; set; }
			public string СрокУстраненияПредписания { get; set; }
			public string НомерПротокола { get; set; }
			public string Статья { get; set; }
			public string Инспектор { get; set; }
			public string ИнспекторРП { get; set; }
			public string ДолжностьИнспекторРП { get; set; }
			public string ТелефонИнспектор { get; set; }
			public string РуководительСокрПриказ { get; set; }
			public string РуководительДолжностьПриказ { get; set; }
		}
    }
}