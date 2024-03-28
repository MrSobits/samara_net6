namespace Bars.GkhGji.Regions.Sahalin.Report.Disposal
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Nso.Entities;
    using Bars.GkhGji.Regions.Nso.Entities.Disposal;
    using Bars.GkhGji.Regions.Sahalin.Properties;
    using GkhGji.Report;

    /// <summary>
    /// Печать "Уведомление о проверке" приказа.
    /// </summary>
    public class DisposalGjiNotificationStimulReport : GjiBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DisposalGjiNotificationStimulReport()
            : base(new ReportTemplateBinary(Resources.DisposalGjiNotification))
        {
        }
        #endregion

        /// <summary>
        /// Наименование отчета.
        /// </summary>
        public override string Name
        {
            get { return "Уведомление о проверке"; }
        }

        /// <summary>
        /// Описание отчета.
        /// </summary>
        public override string Description
        {
            get { return "Уведомление о проверке (из приказа)"; }
        }

        /// <summary>
        /// Идентификатор отчета.
        /// </summary>
        public override string Id
        {
            get { return "DisposalNotification"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати.
        /// </summary>
        public override string CodeForm
        {
            get { return "Disposal"; }
        }

        /// <summary>
        /// Формат печатной формы.
        /// </summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        /// <summary>
        /// Код шаблона (файла).
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Идентификатор приказа.
        /// </summary>
        private long DocumentId { get; set; }

        /// <summary>
        /// Подготовить параметры отчета.
        /// </summary>
        /// <param name="reportParams">
        /// Параметры отчета.
        /// </param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var disposalDomain = Container.ResolveDomain<NsoDisposal>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var disposalLongTextDomain = Container.ResolveDomain<DisposalLongText>();
            var disposalTypeSurveyDomain = Container.ResolveDomain<DisposalTypeSurvey>();
            var typeSurveyGoalDomain = Container.ResolveDomain<TypeSurveyGoalInspGji>();
            var disposalProvideDocDomain = Container.Resolve<IDomainService<DisposalProvidedDoc>>();

            var disposal = disposalDomain.Get(DocumentId);

            try
            {
                if (disposal == null)
                {
                    var disposalText = Container.Resolve<IDisposalText>().SubjectiveCase;
                    throw new ReportProviderException(string.Format("Не удалось получить {0}", disposalText.ToLower()));
                }

                FillCommonFields(disposal);

                var service = Container.Resolve<IDisposalService>();
                var disposalInfo = service.GetInfo(disposal.Id);

                var disposalLongDesc = disposalLongTextDomain.GetAll().FirstOrDefault(x => x.Disposal.Id == this.DocumentId);

                 var goals = disposalTypeSurveyDomain.GetAll()
                        .Join(typeSurveyGoalDomain.GetAll(),
                            x => x.TypeSurvey.Id,
                            y => y.TypeSurvey.Id,
                            (x, y) => new {x.Disposal.Id, y.SurveyPurpose.Name})
                        .Where(x => x.Id == disposal.Id)
                    .Select(x => new {Наименование = x.Name})
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ЦелиПроверки",
                    MetaType = nameof(Object),
                    Data = goals
                });

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Уведомление",
                    MetaType = nameof(Object),
                    Data = new
                    {
                        Дата = disposal.NcDate,
                        ДатаПриказа = disposal.DocumentDate,
                        НомерДокумента = disposal.DocumentNumber,
                        Документ = disposalInfo.PlanName,
                        ВидПроверки = disposal.KindCheck.Return(x => x.Name),
                        НачалоПериодаВыезда = disposal.ObjectVisitStart,
                        ЦельПроверки = goals.AggregateWithSeparator(x => x.Наименование, ", "),
                        ВремяИМестоСборПредставителей =
                            disposalLongDesc != null && disposalLongDesc.NoticeDescription != null
                                ? Encoding.UTF8.GetString(disposalLongDesc.NoticeDescription)
                                : string.Empty,
                        ДатаПротокола = disposal.NoticeDateProtocol,
                        ВремяПротокола = disposal.NoticeTimeProtocol,
                        МестоПротокола = disposal.NoticePlaceCreation,
                        ПериодПроведенияПроверки = disposal.DateStart
                    }
                });

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Руководитель",
                    MetaType = nameof(Object),
                    Data = new
                    {
                        НачальникРП = disposal.IssuedDisposal.Return(x => x.FioGenitive),
                        ДолжностьНачальникаРП = disposal.IssuedDisposal.Return(x => x.PositionGenitive),
                        НачальникВП = disposal.IssuedDisposal.Return(x => x.FioAccusative),
                        ДолжностьНачальникаВП = disposal.IssuedDisposal.Return(x => x.PositionAccusative),
                        ФИОТП = disposal.IssuedDisposal.Return(x => x.FioAblative),
                        ДолжностьТП = disposal.IssuedDisposal.Return(x => x.PositionAblative)
                    }
                });

                var contragent = disposal.Inspection.Contragent;
                if (contragent != null)
                {
                    var contragents = new
                    {
                        ЮрАдрес = contragent.JuridicalAddress,
                        Наименование = contragent.Name,
                        НаименованиеСокр = contragent.ShortName,
                        ФактАдрес = contragent.FactAddress
                    };
                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "Контрагенты",
                        MetaType = nameof(Object),
                        Data = contragents
                    });
                }

                var inspectors = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => new
                    {
                        ФИОТп = x.Inspector.FioAblative,
                        ДолжностьТп = x.Inspector.PositionAblative,
                        ФИО = x.Inspector.Fio,
                        Должность = x.Inspector.Position,
                        ФИОСокр = x.Inspector.ShortFio,
                        ФИОВп = x.Inspector.FioAccusative,
                        ДолжностьВп = x.Inspector.PositionAccusative,
                        Телефон = x.Inspector.Phone
                    })
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Инспекторы",
                    MetaType = nameof(Object),
                    Data = inspectors
                });

                var listDisposalProvidedDoc = disposalProvideDocDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new { Наименование = x.ProvidedDoc.Name })
                    .ToList();
                this.DataSources.Add(new MetaData
                {
                    SourceName = "ПредоставляемыеДокументы",
                    MetaType = nameof(Object),
                    Data = listDisposalProvidedDoc
                });
            }
            finally
            {
                Container.Release(disposalDomain);
                Container.Release(docInspectorDomain);
                Container.Release(disposalLongTextDomain);
                Container.Release(disposalTypeSurveyDomain);
                Container.Release(typeSurveyGoalDomain);
                Container.Release(disposalProvideDocDomain);
            }
        }

        /// <summary>
        /// Установить пользовательские параметры.
        /// </summary>
        /// <param name="userParamsValues">
        /// Значения пользовательских параметров.
        /// </param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        /// <summary>
        /// Получить список шаблонов.
        /// </summary>
        /// <returns>
        /// Список шаблонов
        /// </returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Code = "BlockGJI_InstructionNotification",
                                   Name = "DisposalNotification",
                                   Description = "Уведомление о проверке из приказа",
                                   Template = Resources.DisposalGjiNotification
                               }
                       };
        }

        /// <summary>
        /// Получить поток шаблона отчета (файла).
        /// </summary>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        private void GetCodeTemplate()
        {
            this.CodeTemplate = "BlockGJI_InstructionNotification";
        }
    }
}
