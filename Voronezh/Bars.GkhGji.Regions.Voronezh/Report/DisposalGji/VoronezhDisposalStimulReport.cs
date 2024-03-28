namespace Bars.GkhGji.Regions.Voronezh.Report
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
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    
    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.Entities;
    using GkhGji.Enums;
    using GkhGji.Report;
    using Gkh.Authentification;
    
    /// <summary>
    /// Отчет для приказа
    /// </summary>
    public class VoronezhDisposalStimulReport : GjiBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// Конструктор
        /// </summary>
        public VoronezhDisposalStimulReport() : base(new ReportTemplateBinary(Properties.Resources.ChelyabinskDisposal))
        {
        }

        #endregion .ctor

        #region Properties

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name => "Приказ";

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description => "Приказ";

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id => "VoronezhDisposal";

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm => "Disposal";

        /// <summary>
        /// Формат экспорта
        /// </summary>
        public override StiExportFormat ExportFormat => StiExportFormat.Word2007;

        #endregion Properties

        private long DocumentId { get; set; }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
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
                    Code = "ChelyabinskDisposal",
                    Name = "ChelyabinskDisposal",
                    Description = "Приказ",
                    Template = Properties.Resources.ChelyabinskDisposal
                }
            };
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        public override void PrepareReport(ReportParams reportParams)
        {
            var disposalDomain = this.Container.ResolveDomain<ChelyabinskDisposal>();

            try
            {
                var disposal = disposalDomain.Get(this.DocumentId);

                if (disposal == null)
                    return;

                this.FillCommonFields(disposal);

                if (!disposal.KindCheck.Return(x => x.Name).IsEmpty())
                {
                    this.ReportParams["ВидПроверкиРП"] = this.GetMorpher().Проанализировать(disposal.KindCheck.Name).Родительный;
                }

                this.ReportParams["Id"] = this.DocumentId.ToString();
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

                this.ReportParams["ОснованиеОбследования"] = this.GetDocumentBase(disposal);
                this.DataSources.Add(new MetaData
                {
                    SourceName = "ФактыНарушений",
                    MetaType = nameof(ФактыНарушений),
                    Data = this.GetFactViolations(disposal)
                });

                this.FillDisposalSubentities(disposal);

                this.FillParentPrescription(disposal);

                this.FillInspection(disposal);

                this.GetUserInspector();
            }
            finally
            {
                this.Container.Release(disposalDomain);
            }
        }

        private void FillDisposalSubentities(Disposal disposal)
        {
            var expertDomain = this.Container.ResolveDomain<DisposalExpert>();
            var docInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            var disposalTypeSurveyDomain = this.Container.ResolveDomain<DisposalTypeSurvey>();
            var typesInspFoundationDomain = this.Container.ResolveDomain<TypeSurveyInspFoundationGji>();
            var typesGoalsDomain = this.Container.ResolveDomain<TypeSurveyGoalInspGji>();
            var disposalProvDocsDomain = this.Container.ResolveDomain<ChelyabinskDisposalProvidedDoc>();
            var verifSubjectDomain = this.Container.ResolveDomain<DisposalVerificationSubject>();

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
                this.Container.Release(expertDomain);
                this.Container.Release(docInspectorDomain);
                this.Container.Release(disposalTypeSurveyDomain);
                this.Container.Release(typesInspFoundationDomain);
                this.Container.Release(typesGoalsDomain);
                this.Container.Release(disposalProvDocsDomain);
            }
        }

        private void FillParentPrescription(Disposal disposal)
        {
            var prescription = this.GetParentDocument(disposal, TypeDocumentGji.Prescription);

            if (prescription != null)
            {
                this.ReportParams["НомерПредписания"] = prescription.DocumentNumber;
                this.ReportParams["ДатаПредписания"] = prescription.DocumentDate.ToDateString();
            }
        }

        private void FillInspection(DocumentGji document)
        {
            if (document.Inspection == null)
                return;

            var inspectionDomain = this.Container.ResolveDomain<InspectionGji>();
            var inspectionAppealsDomain = this.Container.ResolveDomain<InspectionAppealCits>();
            var baseJurPersonDomain = this.Container.ResolveDomain<BaseJurPerson>();
            var baseProsClaimDomain = this.Container.ResolveDomain<BaseProsClaim>();

            try
            {
                //т.к. у документа гжи lazy ссылка на проверку
                var inspection = inspectionDomain.Get(document.Inspection.Id);

                if (inspection == null)
                    return;

                this.FillInspectionSubentities(inspection);

                this.ReportParams["ФИО"] = inspection.PhysicalPerson;
                this.ReportParams["ОбъектПроверки"] = inspection.PersonInspection.GetEnumMeta().Display;

                if (inspection.Contragent != null)
                {
                    this.ReportParams["ИНН"] = inspection.Contragent.Inn;
                    this.ReportParams["ЮридическийАдрес"] = inspection.Contragent.FiasJuridicalAddress != null
                        ? inspection.Contragent.FiasJuridicalAddress.AddressName
                        : inspection.Contragent.AddressOutsideSubject;
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
            finally
            {
                this.Container.Release(inspectionDomain);
                this.Container.Release(inspectionAppealsDomain);
                this.Container.Release(baseJurPersonDomain);
                this.Container.Release(baseProsClaimDomain);
            }
        }

        private void FillInspectionSubentities(InspectionGji inspection)
        {
            if (inspection == null) return;

            var inspectionRoDomain = this.Container.ResolveDomain<InspectionGjiRealityObject>();

            try
            {
                this.DataSources.Add(new MetaData
                {
                    SourceName = "ПроверяемыеДома",
                    MetaType = nameof(Object),
                    Data =  inspectionRoDomain.GetAll()
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
                this.Container.Release(inspectionRoDomain);
            }
        }

        private List<ФактыНарушений> GetFactViolations(ChelyabinskDisposal disposal)
        {
            var serviceDisposalFactViolation = this.Container.ResolveDomain<DisposalFactViolation>();
            using (this.Container.Using(serviceDisposalFactViolation))
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

        private void GetUserInspector()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            try
            {
                var oper = userManager.GetActiveOperator();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Инспектор",
                    MetaType = nameof(Object),
                    Data = new
                    {
                        ДолжностьИнспектораОператора = oper?.Inspector.Position,
                        ФИОИнспектораОператора = oper?.Inspector.ShortFio,
                        ТелефонИнспектораОператора = oper?.Inspector.Phone,
                        EmailИнспектораОператора = oper?.Inspector.Email
                    }
                });
            }
            finally
            {
                this.Container.Release(userManager);
            }
        }

    }
}