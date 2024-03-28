namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report.DisposalGji
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Enums;
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
    public class DecisionNotificationStimulReport : GjiBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DecisionNotificationStimulReport()
            : base(new ReportTemplateBinary(Resources.DecisionNotification))
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
            get { return "Уведомление о проверке (из решения)"; }
        }

        /// <summary>
        /// Идентификатор отчета.
        /// </summary>
        public override string Id
        {
            get { return "DecisionNotification"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати.
        /// </summary>
        public override string CodeForm
        {
            get { return "Decision"; }
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
            var disposalDomain = this.Container.ResolveDomain<Decision>();
            var disposalProvideDocDomain = this.Container.Resolve<IDomainService<DecisionProvidedDoc>>();
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

                this.ReportParams["Id"] = this.DocumentId;
                this.ReportParams["DispId"] = this.DocumentId;
                // TODO: разобраться
                /*this.Report.RegData("УправляющаяОрганизация", new
                {
                    ДолжностьРуководителяДП = disposal.Inspection.Contragent.NameDative,
                    Наименование = disposal.Inspection.Contragent.Name,
                    ИНН = disposal.Inspection.Contragent.Inn,
                    ФИОРуководителяДП = post != null ? post.Position.NameDative : string.Empty,
                    ЮридическийАдрес = disposal.Inspection.Contragent.JuridicalAddress
                });

                this.Report.RegData("Уведомление", new
                {
                    Дата = disposal.NcDate,
                    НомерДокумента = disposal.NcNum
                });*/

            }
            finally
            {
                this.Container.Release(disposalDomain);
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
                                   Code = "BlockGJI_DecisionNotification",
                                   Name = "DecisionNotification",
                                   Description = "Уведомление о проверке из решения",
                                   Template = Resources.DecisionNotification
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
            this.CodeTemplate = "BlockGJI_DecisionNotification";
        }

    }
}
