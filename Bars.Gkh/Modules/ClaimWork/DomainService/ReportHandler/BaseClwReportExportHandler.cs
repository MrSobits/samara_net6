namespace Bars.Gkh.Modules.ClaimWork.DomainService.ReportHandler
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Базовый обаботчик печати документов
    /// </summary>
    /// <typeparam name="T">Тип документа</typeparam>
    /// <typeparam name="TReport">Тип отчета ПИРа</typeparam>
    public abstract class BaseClwReportExportHandler<T, TReport> : IClwReportExportHandler 
        where T : DocumentClw
        where TReport : IClaimWorkCodedReport
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<T> DocumentDomain { get; set; }

        /// <inheritdoc />
        public virtual bool CanHandle(IClaimWorkCodedReport report)
        {
            return report.GetType() == typeof(TReport);
        }

        /// <inheritdoc />
        public void HandleExport(IClaimWorkCodedReport report, FileInfo file)
        {
            ArgumentChecker.NotNull(report, nameof(report));
            ArgumentChecker.NotNull(file, nameof(file));

            if (this.CanHandle(report))
            {
                var document = this.DocumentDomain.Load(report.DocumentId.ToLong());

                if (document.IsNotNull() && this.ValidateDocument(document))
                {
                    this.HandleExportInternal(document, file);
                }
            }
        }

        /// <summary>
        /// Проверить документ на возвможность обработки
        /// </summary>
        protected virtual bool ValidateDocument(T document)
        {
            return true;
        }

        /// <summary>
        /// Произвести обработку документа
        /// </summary>
        protected abstract void HandleExportInternal(T document, FileInfo fileInfo);
    }
}