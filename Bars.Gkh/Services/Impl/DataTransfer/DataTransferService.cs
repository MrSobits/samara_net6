namespace Bars.Gkh.Services.Impl.DataTransfer
{
    using System.IO;
    // using System.ServiceModel.Activation;

    using Bars.B4;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.DataTransfer;
    using Bars.Gkh.Services.ServiceContracts.DataTransfer;
    using Bars.Gkh.SystemDataTransfer.Domain;

    // TODO wcf
    // [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DataTransferService : IDataTransferService
    {
        public ISystemIntegrationService SystemIntegrationService { get; set; }

        /// <inheritdoc />
        public Result CreateExportTask(CreateExportTaskParams createExportTaskParams)
        {
            this.SystemIntegrationService.CreateExportTask(
                createExportTaskParams.Guid, 
                createExportTaskParams.TypeNames, 
                createExportTaskParams.ExportDependencies);

            return Result.NoErrors;
        }

        /// <inheritdoc />
        public Result CreateImportTask(CreateImportTaskParams createImportTaskParams)
        {
            this.SystemIntegrationService.CreateImportTask(createImportTaskParams.Guid, new MemoryStream(createImportTaskParams.Data));
            return Result.NoErrors;
        }

        /// <inheritdoc />
        public Result Notify(NotificationParams notificationParams)
        {
            this.SystemIntegrationService.ProcessNotify(
                notificationParams.Guid,
                notificationParams.OperationType,
                new BaseDataResult(notificationParams.Success, notificationParams.Message));

            return Result.NoErrors;
        }

        /// <inheritdoc />
        public Result SetSuccessSectionImport(SectionProgressParams sectionProgressParams)
        {
            this.SystemIntegrationService.HandleSectionImportState(sectionProgressParams.Guid,
                sectionProgressParams.Name,
                sectionProgressParams.Success,
                false);

            return Result.NoErrors;
        }
    }
}