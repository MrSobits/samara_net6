namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report.DisposalGji
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGji.Report;

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
            get { return "Уведомление о проверке (из распоряжения)"; }
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
            var disposalDomain = this.Container.ResolveDomain<ChelyabinskDisposal>();
            var docInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            var disposalLongTextDomain = this.Container.ResolveDomain<DisposalLongText>();
            var disposalTypeSurveyDomain = this.Container.ResolveDomain<DisposalTypeSurvey>();
            var typeSurveyGoalDomain = this.Container.ResolveDomain<TypeSurveyGoalInspGji>();
            var disposalProvideDocDomain = this.Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var contragentContact = this.Container.ResolveDomain<ContragentContact>();

            var disposal = disposalDomain.Get(this.DocumentId);

            try
            {
                if (disposal == null)
                {
                    var disposalText = this.Container.Resolve<IDisposalText>().SubjectiveCase;
                    throw new ReportProviderException(string.Format("Не удалось получить {0}", disposalText.ToLower()));
                }

                this.FillCommonFields(disposal);

                var post = contragentContact.FirstOrDefault(x => x.Contragent.Id == disposal.Inspection.Contragent.Id);

                this.ReportParams["Id"] = this.DocumentId.ToString();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "УправляющаяОрганизация",
                    MetaType = nameof(Object),
                    Data = new
                    {
                        ДолжностьРуководителяДП = disposal.Inspection.Contragent.NameDative,
                        Наименование = disposal.Inspection.Contragent.Name,
                        ИНН = disposal.Inspection.Contragent.Inn,
                        ФИОРуководителяДП = post != null ? post.Position.NameDative : string.Empty,
                        ЮридическийАдрес = disposal.Inspection.Contragent.JuridicalAddress
                    }
                });
                
                this.DataSources.Add(new MetaData
                {
                    SourceName = "Уведомление",
                    MetaType = nameof(Object),
                    Data = new
                    {
                        Дата = disposal.NcDate,
                        НомерДокумента = disposal.DocumentNumber,
                        ДатаПисьма = disposal.NcDateLatter,
                        НомерПисьма = disposal.NcNumLatter,
                        ДатаСоставленияПротокола = disposal.NoticeDateProtocol,
                        ВремяСоставленияПротокола = disposal.NoticeTimeProtocol,
                        МестоСоставленияПротокола = disposal.NoticePlaceCreation,
                        МестоВремяСбора = disposal.NoticeDescription
                    }
                });

                if (disposal.ResponsibleExecution != null)
                {
                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "ОтветственныйЗаИсполнение",
                        MetaType = nameof(Object),
                        Data = new
                        {
                            НаименованиеОтдела = this.GetZonalInspectionInspector(disposal.ResponsibleExecution.Id),
                            ДолжностьОтветственного = disposal.ResponsibleExecution.Position,
                            ФИО = disposal.ResponsibleExecution.Fio,
                            ТелефонОтветственного = disposal.ResponsibleExecution.Phone
                        }
                    });
                }
            }
            finally
            {
                this.Container.Release(disposalDomain);
                this.Container.Release(docInspectorDomain);
                this.Container.Release(disposalLongTextDomain);
                this.Container.Release(disposalTypeSurveyDomain);
                this.Container.Release(typeSurveyGoalDomain);
                this.Container.Release(disposalProvideDocDomain);
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
                                   Description = "Уведомление о проверке из распоряжения",
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

        /// <summary>
        /// Получение жилищной инспекции для поля ОтветственныйЗаИсполнение.НаименованиеОтдела
        /// </summary>
        /// <param name="idInspector"></param>
        /// <returns></returns>
        private string GetZonalInspectionInspector(long idInspector)
        {
            var zonalInspectionNames = new StringBuilder();

            // Пробегаемся по зон. инспекциям и формируем итоговую строку наименований и строку идентификаторов
            var serviceZonalInspectionInspector = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();

            var dataZonalInspection = serviceZonalInspectionInspector.GetAll()
                .Where(x => x.Inspector.Id == idInspector)
                .Select(x => new
                {
                    x.ZonalInspection.ZoneName
                })
                .ToArray();

            foreach (var item in dataZonalInspection)
            {
                if (!string.IsNullOrEmpty(item.ZoneName))
                {
                    if (zonalInspectionNames.Length > 0)
                        zonalInspectionNames.Append(", ");

                    zonalInspectionNames.Append(item.ZoneName);
                }
            }

            return zonalInspectionNames.ToString();
        }
    }
}
