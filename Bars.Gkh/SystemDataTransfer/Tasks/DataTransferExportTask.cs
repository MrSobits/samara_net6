namespace Bars.Gkh.SystemDataTransfer.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.ServiceModel;

    using Bars.B4;

    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using Bars.Gkh.SystemDataTransfer.Domain;

    using Castle.MicroKernel.Lifestyle;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Задача Quartz экспорта данных и дальнейшего проброса результата
    /// </summary>
    public class DataTransferExportTask : BaseTask
    {
        /// <inheritdoc />
        public override void Execute(DynamicDictionary dynamicDictionary)
        {
            var guid = dynamicDictionary.GetAs<Guid>("guid");

            using (this.Container.BeginScope())
            {
                var dataTransferProvider = this.Container.Resolve<IDataTransferProvider>();
                var dataTransferService = this.Container.Resolve<ISystemIntegrationService>();
                try
                {
                    // сообщаем о взятии задачи в работу
                    dataTransferService.NotifyStartExport(guid);

                    var resultStream = dataTransferProvider.Export(
                        dynamicDictionary.GetAs("typeNames", new List<string>()),
                        dynamicDictionary.GetAs("exportDependencies", false));

                    dataTransferService.SendExportResult(guid, new GenericDataResult<Stream>(resultStream));
                }
                catch (FaultException exception)
                {
                    this.Container.Resolve<ILogger>().LogError(exception, "Ошибка во время отправки результата экспорта");
                }
                catch (Exception exception)
                {
                    this.Container.Resolve<ILogger>().LogError(exception, "Ошибка во время отправки результата импорта");
                    dataTransferService.SendExportResult(guid, new GenericDataResult<Stream>(null, exception.Message) { Success = false });
                }
                finally
                {
                    this.Container.Release(dataTransferProvider);
                    this.Container.Release(dataTransferService);
                }
            }
        }
    }
}