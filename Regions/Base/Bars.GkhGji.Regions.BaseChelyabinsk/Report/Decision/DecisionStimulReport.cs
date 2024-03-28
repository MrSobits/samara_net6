namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.IoC;
    using B4.Utils;
    using System;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    using Entities.Disposal;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.Entities;
    using GkhGji.Enums;
    using GkhGji.Report;
    using Gkh.Authentification;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Отчет для приказа
    /// </summary>
    public class DecisionStimulReport : GjiBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// Конструктор
        /// </summary>
        public DecisionStimulReport() : base(new ReportTemplateBinary(Properties.Resources.ChelyabinskDisposal))
        {
        }

        #endregion .ctor

        #region Properties

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get { return "Решение"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Решение"; }
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
            get { return "Decision"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "Decision"; }
        }

    
        public override StiExportFormat ExportFormat
        {
            get { return GetUserFormat(); }
        }

        ///// <summary>
        ///// Настройки экспорта
        ///// </summary>
        //public override StiExportSettings ExportSettings
        //{
        //    get
        //    {
        //        return new StiWord2007ExportSettings
        //        {
        //            RemoveEmptySpaceAtBottom = false
        //        };
        //    }
        //}

        #endregion Properties

        private long DocumentId { get; set; }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        private StiExportFormat GetUserFormat()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            try
            {
                var oper = userManager.GetActiveOperator();
                if (oper != null)
                {
                    var ext = oper.ExportFormat;
                    switch (ext)
                    {
                        case OperatorExportFormat.docx:
                            return StiExportFormat.Word2007;
                        case OperatorExportFormat.odt:
                            return StiExportFormat.Odt;
                        case OperatorExportFormat.pdf:
                            return StiExportFormat.Pdf;
                        default: return StiExportFormat.Word2007;
                    }
                }
                else
                {
                    return StiExportFormat.Word2007;
                }

            }
            finally
            {
                this.Container.Release(userManager);
            }
            
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
                    Code = "Decision",
                    Name = "Decision",
                    Description = "Решение",
                    Template = Properties.Resources.ChelyabinskDisposal
                }
            };
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        public override void PrepareReport(ReportParams reportParams)
        {
            var disposalDomain = this.Container.ResolveDomain<Decision>();

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

                this.ReportParams["Id"] = Convert.ToInt32(this.DocumentId);
                this.ReportParams["ДатаПриказа"] = disposal.DocumentDate.ToDateString("«dd» MMMM yyyy г.");
                this.ReportParams["НомерПриказа"] = disposal.DocumentNumber;

                this.ReportParams["СогласованиеСПрокуратурой"] = disposal.TypeAgreementProsecutor.GetEnumMeta().Display;

                this.ReportParams["ВремяС"] = disposal.TimeVisitStart.ToTimeString("HH час. mm мин.");
                this.ReportParams["ВремяПо"] = disposal.TimeVisitEnd.ToTimeString("HH час. mm мин.");

                this.ReportParams["ПериодПроведенияПроверкиС"] = disposal.DateStart.ToDateString("«dd» MMMM yyyy г.");
                this.ReportParams["ПериодПроведенияПроверкиПо"] = disposal.DateEnd.ToDateString("«dd» MMMM yyyy г.");

                this.ReportParams["ДЛВынесшееПриказ"] = disposal.IssuedDisposal.Return(x => x.Fio);
                this.ReportParams["ДЛВынесшееПриказДолжность"] = disposal.IssuedDisposal.Return(x => x.Position);



                this.ReportParams["ТипКонтрагента"] = disposal.Return(x => x.Inspection).Return(x => x.TypeJurPerson.GetEnumMeta().Display);

                this.ReportParams["ОснованиеОбследования"] = this.GetDocumentBase(disposal);

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

        private void FillDisposalSubentities(Decision disposal)
        {
            var expertDomain = this.Container.ResolveDomain<DecisionExpert>();
            var docInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            var disposalProvDocsDomain = this.Container.ResolveDomain<DecisionProvidedDoc>();
            var verifSubjectDomain = this.Container.ResolveDomain<DecisionVerificationSubject>();

            try
            {
                var experts = expertDomain.GetAll()
                    .Where(x => x.Decision.Id == disposal.Id)
                    .Select(x => new
                    {
                        ФИО = x.Expert.Name,
                        Должность = x.Expert.Code
                    })
                    .ToList();

                // TODO: разобраться
                /*this.Report.RegData("Эксперты", "Эксперты", experts);

                var inspectors = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == disposal.Id)
                    .Select(x => new
                    {
                        ФИО = x.Inspector.FioGenitive,
                        Должность = x.Inspector.PositionGenitive
                    })
                    .ToList();

                this.Report.RegData("Инспекторы", "Инспекторы", inspectors);


                var provDocs = disposalProvDocsDomain.GetAll()
                    .Where(x => x.Decision.Id == disposal.Id)
                    .Select(x => new { Наименование = x.ProvidedDoc.Name })
                    .ToList();

                this.Report.RegData("ПредоставляемыеДокументы", "ПредоставляемыеДокументы", provDocs);

                var verifSubjs = verifSubjectDomain.GetAll()
                    .Where(x => x.Decision.Id == disposal.Id)
                    .Select(x => new { Наименование = x.SurveySubject.Name })
                    .ToList();

                this.Report.RegData("ПредметыПроверки", "ПредметыПроверки", verifSubjs);*/
            }
            finally
            {
                this.Container.Release(expertDomain);
                this.Container.Release(docInspectorDomain);

                this.Container.Release(disposalProvDocsDomain);
            }
        }

        private void FillParentPrescription(Decision disposal)
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
                // TODO: разобраться
                /*this.Report.RegData("ПроверяемыеДома",
                    inspectionRoDomain.GetAll()
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
                        .ToArray());*/
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

                // TODO: разобраться
                /*this.Report.RegData("Инспектор",
                         new
                         {
                             ДолжностьИнспектораОператора = oper?.Inspector.Position,
                             ФИОИнспектораОператора = oper?.Inspector.ShortFio,
                             ТелефонИнспектораОператора = oper?.Inspector.Phone,
                             EmailИнспектораОператора = oper?.Inspector.Email

                         });*/
            }
            finally
            {
                this.Container.Release(userManager);
            }
        }

    }
}