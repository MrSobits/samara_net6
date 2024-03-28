namespace Bars.Gkh.SystemDataTransfer.Domain
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using Bars.B4.Application;
    using Bars.B4.Utils;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.DataTransfer;
    using Bars.Gkh.Services.ServiceContracts.DataTransfer;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Клиент для WCF интеграции с внешней системой ЖКХ.Комплекс
    /// </summary>
    public class DataTransferIntegrationClient : ClientBase<IDataTransferService>, IDataTransferService
    {
        public DataTransferIntegrationClient(Binding binding, EndpointAddress address) : 
            base(binding, address)
        { }

        /// <inheritdoc />
        public Result CreateExportTask(CreateExportTaskParams createExportTaskParams)
        {
            return this.InTryCatch(() => this.Channel.CreateExportTask(createExportTaskParams));
        }

        /// <inheritdoc />
        public Result CreateImportTask(CreateImportTaskParams createImportTaskParams)
        {
            return this.InTryCatch(() => this.Channel.CreateImportTask(createImportTaskParams));
        }

        /// <inheritdoc />
        public Result Notify(NotificationParams notificationParams)
        {
            return this.InTryCatch(() => this.Channel.Notify(notificationParams));
        }

        /// <inheritdoc />
        public Result SetSuccessSectionImport(SectionProgressParams sectionProgressParams)
        {
            return this.InTryCatch(() => this.Channel.SetSuccessSectionImport(sectionProgressParams));
        }

        private Result InTryCatch(Func<Result> func)
        {
            try
            {
                return func();
            }
            catch (Exception exception) when(exception.IsNot<FaultException>())
            {
                ApplicationContext.Current.Container.Resolve<ILogger>().LogError(exception, "Ошибка при выполнении интеграции");
                throw;
            }
        }
    }
}