namespace Bars.GkhGji.Regions.Sahalin.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;

    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Nso.Entities;
    using GkhGji.Report;

    public class DisposalGjiStimulReport : GjiBaseStimulReport
    {
        #region .crot & properties

        public DisposalGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.SahalinDisposal))
        {
        }

        private long DocumentId { get; set; }
        private NsoDisposal Disposal { get; set; }

        public override string Id
        {
            get { return "Disposal"; }
        }

        public override string CodeForm
        {
            get { return "Disposal"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }
        public override string Name
        {
            get { return "Приказ"; }
        }

        public override string Description
        {
            get { return "Приказ"; }
        }

        protected override string CodeTemplate { get; set; }

        #endregion

        #region DomainServices

        public IDomainService<NsoDisposal> NsoDisposalDomain { get; set; }
        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }
        public IDomainService<DisposalTypeSurvey> DisposalTypeSurveyDomain { get; set; }
        public IDomainService<TypeSurveyGoalInspGji> TypeSurveyGoalInspGjiDomain { get; set; }

        #endregion

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            Disposal = NsoDisposalDomain.GetAll().FirstOrDefault(x => x.Id == DocumentId);
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "Disposal",
                            Name = "Disposal",
                            Description = "Приказ",
                            Template = Properties.Resources.SahalinDisposal
                        }
                };
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        private void GetCodeTemplate()
        {
            CodeTemplate = "Disposal";
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            if (Disposal == null)
            {
                throw new ReportProviderException("Не удалось получить приказ");
            }
            FillCommonFields(Disposal);

            // ДЛ, вынесшее приказ
            var issuedDispInspector = Disposal.IssuedDisposal;
            var issuedDispInspectorId = issuedDispInspector != null ? issuedDispInspector.Id : 0L;

            // Цели типов обследований
            var typeSurveyQuery =
                DisposalTypeSurveyDomain.GetAll().Where(x => x.Disposal.Id == Disposal.Id).Select(x => x.TypeSurvey);
            var goals =
                TypeSurveyGoalInspGjiDomain.GetAll()
                    .Where(x => typeSurveyQuery.Any(y => y.Id == x.TypeSurvey.Id))
                    .Select(x => x.SurveyPurpose.Name)
                    .AggregateWithSeparator(", ");

            var disposalProxy =
                new
                {
                    Дата =
                        Disposal.DocumentDate.HasValue
                            ? Disposal.DocumentDate.Value.ToShortDateString()
                            : string.Empty,
                    НомерДокумента = Disposal.DocumentNumber,
                    ПериодС =
                        Disposal.DateStart.HasValue ? Disposal.DateStart.Value.ToShortDateString() : string.Empty,
                    ПериодПо = Disposal.DateEnd.HasValue ? Disposal.DateEnd.Value.ToShortDateString() : string.Empty,
                    ВидПроверки =
                        Disposal.KindCheck != null ? Disposal.KindCheck.Code.GetEnumMeta().Display : string.Empty,
                    СрокПроверки = Disposal.PeriodCorrect,
                    ДЛВынПриказ = issuedDispInspector != null ? issuedDispInspector.Position + "\n" + issuedDispInspector.Fio : string.Empty,
                    ЦелиПроверки = goals
                };

            this.DataSources.Add(new MetaData
            {
                SourceName = "Приказ",
                MetaType = nameof(Object),
                Data = disposalProxy
            });

            if (Disposal.Inspection != null && Disposal.Inspection.Contragent != null)
            {
                var contragent =
                    new
                    {
                        Контрагент = Disposal.Inspection.Contragent.Name,
                        ФактАдрес = Disposal.Inspection.Contragent.FactAddress,
                        ЮрАдрес = Disposal.Inspection.Contragent.FiasJuridicalAddress != null 
                            ? Disposal.Inspection.Contragent.FiasJuridicalAddress.AddressName
                            : string.Empty,
                        КраткоеНаименование = Disposal.Inspection.Contragent.ShortName
                    };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Контрагенты",
                    MetaType = nameof(Object),
                    Data = contragent
                });
            }

            var inspectors =
                DocumentGjiInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == Disposal.Id)
                    .Select(x => new
                    {
                        ФИО = x.Inspector.Fio,
                        Телефон = x.Inspector.Phone,
                        ФИОТп = x.Inspector.FioAblative,
                        ФИОВп = x.Inspector.FioAccusative,
                        ДолжностьТп = x.Inspector.PositionAblative,
                        ДолжностьВп = x.Inspector.PositionAccusative,
                        ДолжностьРп = x.Inspector.PositionGenitive,
                        ФИОСокр = x.Inspector.ShortFio,
                        ДЛВынПриказ = x.Inspector.Id == issuedDispInspectorId
                    })
                    .ToList();

            this.DataSources.Add(new MetaData
            {
                SourceName = "Инспекторы",
                MetaType = nameof(Object),
                Data = inspectors
            });
            

            //.ToString(CultureInfo.GetCultureInfo("ru-RU"));
        }

        public class DisposalProxy
        {
            public string Дата { get; set; }
            public string НомерДокумента { get; set; }
            public string ПериодС { get; set; }
            public string ПериодПо { get; set; }
            public string ВидПроверки { get; set; }
            public string ДлВынПриказ { get; set; }
            public string ЦелиПроверки { get; set; }
            public string СрокПроверки { get; set; }
            public string Инспекторы { get; set; }
            public string Контрагенты { get; set; }
        }
    }
}