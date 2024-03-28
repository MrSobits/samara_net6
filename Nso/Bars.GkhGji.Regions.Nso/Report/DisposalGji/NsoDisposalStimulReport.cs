namespace Bars.GkhGji.Regions.Nso.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.B4.IoC;
    using B4.Utils;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;

    using Entities;
    using Entities.Disposal;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.Entities;
    using GkhGji.Enums;
    using GkhGji.Report;

	/// <summary>
	/// Отчет для приказа
	/// </summary>
	public class NsoDisposalStimulReport : GjiBaseStimulReport
    {
        #region .ctor

		/// <summary>
		/// Конструктор
		/// </summary>
        public NsoDisposalStimulReport() : base(new ReportTemplateBinary(Properties.Resources.NsoDisposal))
        {
        }

        #endregion .ctor

        #region Properties

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get { return "Приказ"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Приказ"; }
        }

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "NsoDisposal"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "Disposal"; }
        }

		/// <summary>
		/// Формат экспорта
		/// </summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

		#endregion Properties

		private long DocumentId { get; set; }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        /// <summary>
        /// Получить список шаблонов
        /// </summary>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "NsoDisposal",
                    Name = "NsoDisposal",
                    Description = "Приказ",
                    Template = Properties.Resources.NsoDisposal
                }
            };
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        public override void PrepareReport(ReportParams reportParams)
        {
            var disposalDomain = Container.ResolveDomain<NsoDisposal>();

            var disposal = disposalDomain.Get(DocumentId);

            if (disposal == null) return;

            FillCommonFields(disposal);

            if (!disposal.KindCheck.Return(x => x.Name).IsEmpty())
            {
                this.ReportParams["ВидПроверкиРП"] = GetMorpher().Проанализировать(disposal.KindCheck.Name).Родительный;
            }

            this.ReportParams["ДатаПриказа"] = disposal.DocumentDate.ToDateString("«dd» MMMM yyyy г.");
            this.ReportParams["НомерПриказа"] = disposal.DocumentNumber;

            this.ReportParams["СогласованиеСПрокуратурой"] = disposal.TypeAgreementProsecutor.GetEnumMeta().Display;

            this.ReportParams["ВремяС"] = disposal.TimeVisitStart.ToTimeString("HH час. mm мин.");
            this.ReportParams["ВремяПо"] = disposal.TimeVisitEnd.ToTimeString("HH час. mm мин.");

            this.ReportParams["ПериодПроведенияПроверкиС"] = disposal.DateStart.ToDateString("«dd» MMMM yyyy г.");
            this.ReportParams["ПериодПроведенияПроверкиПо"] = disposal.DateEnd.ToDateString("«dd» MMMM yyyy г.");

            this.ReportParams["ДЛВынесшееПриказ"] = disposal.IssuedDisposal.Return(x => x.Fio);
            this.ReportParams["ДЛВынесшееПриказДолжность"] = disposal.IssuedDisposal.Return(x => x.Position);

            this.ReportParams["ОтветственныйЗаВыполнение"] = disposal.ResponsibleExecution.Return(x => x.Fio);
            this.ReportParams["ОтветственныйЗаВыполнениеТелефон"] = disposal.ResponsibleExecution.Return(x => x.Phone);
            this.ReportParams["ОтветственныйЗаВыполнениеДолжность"] = disposal.ResponsibleExecution.Return(x => x.Position);
            this.ReportParams["ОтветственныйЗаВыполнениеЭлектроннаяПочта"] = disposal.ResponsibleExecution.Return(x => x.Email);

            this.ReportParams["ТипКонтрагента"] = disposal.Return(x => x.Inspection).Return(x => x.TypeJurPerson.GetEnumMeta().Display);

            this.ReportParams["ОснованиеОбследования"] = GetDocumentBase(disposal);

            this.DataSources.Add(new MetaData
            {
                SourceName = "ФактыНарушений",
                MetaType = nameof(ФактыНарушений),
                Data = GetFactViolations(disposal)
            });

            FillDisposalSubentities(disposal);

            FillParentPrescription(disposal);

            FillInspection(disposal);
        }

        private void FillDisposalSubentities(Disposal disposal)
        {
            var expertDomain = Container.ResolveDomain<DisposalExpert>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var disposalTypeSurveyDomain = Container.ResolveDomain<DisposalTypeSurvey>();
            var typesInspFoundationDomain = Container.ResolveDomain<TypeSurveyInspFoundationGji>();
            var typesGoalsDomain = Container.ResolveDomain<TypeSurveyGoalInspGji>();
            var disposalProvDocsDomain = Container.ResolveDomain<NsoDisposalProvidedDoc>();
            var verifSubjectDomain = Container.ResolveDomain<DisposalVerificationSubject>();

            try
            {
                var experts = expertDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new
                    {
                        ФИО = x.Expert.Name,
                        Должность = x.Expert.Code
                    })
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Эксперты",
                    MetaType = nameof(Object),
                    Data = experts
                });

                var inspectors = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == disposal.Id)
                    .Select(x => new
                    {
                        ФИО = x.Inspector.FioGenitive,
                        Должность = x.Inspector.PositionGenitive
                    })
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Инспекторы",
                    MetaType = nameof(Object),
                    Data = inspectors
                });

                var typeIds = disposalTypeSurveyDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => x.TypeSurvey.Id)
                    .ToArray();

                var foundations = typesInspFoundationDomain.GetAll()
                    .Where(x => typeIds.Contains(x.TypeSurvey.Id))
                    .Select(x => new { Наименование = x.NormativeDoc.Name })
                    .Distinct()
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ПравовыеОснованияПроверки",
                    MetaType = nameof(Object),
                    Data = foundations
                });

                var goals = typesGoalsDomain.GetAll()
                    .Where(x => typeIds.Contains(x.TypeSurvey.Id))
                    .Select(x => new { Наименование = x.SurveyPurpose.Name })
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ЗадачиПроверки",
                    MetaType = nameof(Object),
                    Data = goals
                });

                var provDocs = disposalProvDocsDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
					.OrderBy(x => x.Order)
                    .Select(x => new { Наименование = x.ProvidedDoc.Name })
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ПредоставляемыеДокументы",
                    MetaType = nameof(Object),
                    Data = provDocs
                });

                var verifSubjs = verifSubjectDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new { Наименование = x.SurveySubject.Name })
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ПредметыПроверки",
                    MetaType = nameof(Object),
                    Data = verifSubjs
                });
            }
            finally
            {
                Container.Release(expertDomain);
                Container.Release(docInspectorDomain);
                Container.Release(disposalTypeSurveyDomain);
                Container.Release(typesInspFoundationDomain);
                Container.Release(typesGoalsDomain);
                Container.Release(disposalProvDocsDomain);
            }
        }

        private void FillParentPrescription(Disposal disposal)
        {
            var prescription = GetParentDocument(disposal, TypeDocumentGji.Prescription);

            if (prescription != null)
            {
                this.ReportParams["НомерПредписания"] = prescription.DocumentNumber;
                this.ReportParams["ДатаПредписания"] = prescription.DocumentDate.ToDateString();
            }
        }

        private void FillInspection(DocumentGji document)
        {
            if (document.Inspection == null) return;

            var inspectionDomain = Container.ResolveDomain<InspectionGji>();
            var inspectionAppealsDomain = Container.ResolveDomain<InspectionAppealCits>();
            var baseJurPersonDomain = Container.ResolveDomain<BaseJurPerson>();
            var baseProsClaimDomain = Container.ResolveDomain<BaseProsClaim>();
            //т.к. у документа гжи lazy ссылка на проверку
            var inspection = inspectionDomain.Get(document.Inspection.Id);

            if (inspection == null) return;

            FillInspectionSubentities(inspection);

            this.ReportParams["ФИО"] = inspection.PhysicalPerson;
            this.ReportParams["ОбъектПроверки"] = inspection.PersonInspection.GetEnumMeta().Display;

            if (inspection.Contragent != null)
            {
                this.ReportParams["ИНН"] = inspection.Contragent.Inn;
                this.ReportParams["ЮридическийАдрес"] = inspection.Contragent.FiasJuridicalAddress != null ? inspection.Contragent.FiasJuridicalAddress.AddressName : inspection.Contragent.AddressOutsideSubject;
                this.ReportParams["ЮридическоеЛицоРП"] = inspection.Contragent.NameGenitive;
                this.ReportParams["ТипПредпринимательства"] = inspection.Contragent.TypeEntrepreneurship.GetEnumMeta().Display;
            }

            this.ReportParams["ТипЮридическогоЛица"] = inspection.TypeJurPerson.GetEnumMeta().Display;

            var baseJurPerson = baseJurPersonDomain.Get(inspection.Id);
            if (baseJurPerson != null)
            {
                this.ReportParams["ПланПроверокНомер"] = baseJurPerson.Plan.Return(x => x.NumberDisposal);
                this.ReportParams["ПланПроверокДата"] = baseJurPerson.Plan.Return(x => x.DateDisposal).ToDateString();
            }

            var baseProsClaim = baseProsClaimDomain.Get(inspection.Id);
            if (baseProsClaim != null)
            {
                this.ReportParams["ДокументНаименование"] = baseProsClaim.DocumentName;
                this.ReportParams["ДокументНомер"] = baseProsClaim.DocumentNumber;
                this.ReportParams["ДокументДата"] = baseProsClaim.DocumentDate.ToDateString();
            }

            var appeal = inspectionAppealsDomain.GetAll()
                .Where(x => x.Inspection.Id == inspection.Id)
                .Select(x => x.AppealCits)
                .FirstOrDefault();

            if (appeal != null)
            {
                this.ReportParams["ТипКорреспондента"] = appeal.TypeCorrespondent.GetEnumMeta().Display;
                this.ReportParams["ДатаОбращения"] = appeal.DateFrom.ToDateString();
                this.ReportParams["НомерОбращения"] = appeal.NumberGji;
            }
        }

        private void FillInspectionSubentities(InspectionGji inspection)
        {
            if (inspection == null) return;

            var inspectionRoDomain = Container.ResolveDomain<InspectionGjiRealityObject>();

            try
            {
                this.DataSources.Add(new MetaData
                {
                    SourceName = "ПроверяемыеДома",
                    MetaType = nameof(Object),
                    Data = inspectionRoDomain.GetAll()
                        .Where(x => x.Inspection.Id == inspection.Id)
                        .Select(x => new
                        {
                            МунРайонИлиГорОкруг = x.RealityObject.Municipality.Name,
                            МунОбразование = x.RealityObject.MoSettlement.Name,
                            НаселенныйПункт = x.RealityObject.FiasAddress.PlaceName,
                            Улица = x.RealityObject.FiasAddress.StreetName,
                            НомерДома = x.RealityObject.FiasAddress.House,
                            НомерКвартиры = x.RoomNums
                        })
                        .ToArray()
                });
            }
            finally
            {
                Container.Release(inspectionRoDomain);
            }
        }

        private List<ФактыНарушений> GetFactViolations(NsoDisposal disposal)
        {
            var serviceDisposalFactViolation = Container.ResolveDomain<DisposalFactViolation>();
            using(Container.Using(serviceDisposalFactViolation))
            {
                var factViols = serviceDisposalFactViolation.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new ФактыНарушений { Наименование = x.TypeFactViolation.Name })
                    .ToList();

                return factViols;
            }
        }

        private class ФактыНарушений
        {
            public string Наименование { get; set; }
        }
    }
}