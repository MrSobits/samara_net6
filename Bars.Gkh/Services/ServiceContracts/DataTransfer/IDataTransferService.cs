namespace Bars.Gkh.Services.ServiceContracts.DataTransfer
{
    using System.ServiceModel;

    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.DataTransfer;

    /// <summary>
    /// Интерфейс интеграции двух систем ЖКХ.Комплекс
    /// </summary>
    [ServiceContract]
    public interface IDataTransferService
    {
        [OperationContract]
        [XmlSerializerFormat]
        Result CreateExportTask(CreateExportTaskParams createExportTaskParams);

        [OperationContract]
        [XmlSerializerFormat]
        Result CreateImportTask(CreateImportTaskParams createImportTaskParams);

        [OperationContract]
        [XmlSerializerFormat]
        Result Notify(NotificationParams notificationParams);

        [OperationContract]
        [XmlSerializerFormat]
        Result SetSuccessSectionImport(SectionProgressParams sectionProgressParams);
    }
}