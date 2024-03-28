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
    public class DecisionRequirementStimulReport : GjiBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DecisionRequirementStimulReport()
            : base(new ReportTemplateBinary(Resources.DecisionRequirements))
        {
        }
        #endregion

        /// <summary>
        /// Наименование отчета.
        /// </summary>
        public override string Name
        {
            get { return "Требование о представлении документов"; }
        }

        /// <summary>
        /// Описание отчета.
        /// </summary>
        public override string Description
        {
            get { return "Требование о предоставлении документов (из решения)"; }
        }

        /// <summary>
        /// Идентификатор отчета.
        /// </summary>
        public override string Id
        {
            get { return "DecisionRequirement"; }
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
                                   Code = "BlockGJI_DecisionRequirement",
                                   Name = "DecisionRequirement",
                                   Description = "Требование о предоставлении документов (из решения)",
                                   Template = Resources.DecisionRequirements
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
            this.CodeTemplate = "BlockGJI_DecisionRequirement";
        }

    }
}
