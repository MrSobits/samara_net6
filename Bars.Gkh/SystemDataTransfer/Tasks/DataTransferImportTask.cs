namespace Bars.Gkh.SystemDataTransfer.Tasks
{
    using System;
    using System.IO;
    using System.ServiceModel;

    using Bars.B4;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using Bars.Gkh.SystemDataTransfer.Domain;

    using Castle.MicroKernel.Lifestyle;

    using Microsoft.Extensions.Logging;

    /// <summary>
    ///  Задача Quartz импорта данных и дальнейшего проброса результата
    /// </summary>
    public class DataTransferImportTask : BaseTask
    {
        /// <inheritdoc />
        public override void Execute(DynamicDictionary dictionary)
        {
            var guid = dictionary.GetAs<Guid>("guid");

            using (this.Container.BeginScope())
            {
                var dataTransferProvider = this.Container.Resolve<IDataTransferProvider>();
                var dataTransferService = this.Container.Resolve<ISystemIntegrationService>();

                Action<string, bool> action = (section, success) =>
                {
                    dataTransferService.HandleSectionImportState(guid, section, success, true);
                };

                try
                {
                    dataTransferProvider.OnSectionImportDone += action;

                    dataTransferService.NotifyStartImport(guid);

                    dataTransferProvider.Import(dictionary.GetAs<Stream>("stream"));
                    dataTransferService.SendImportResult(guid, new BaseDataResult());
                }
                catch (FaultException exception)
                {
                    this.Container.Resolve<ILogger>().LogError(exception, "Ошибка во время отправки результата импорта");
                }
                catch (Exception exception)
                {
                    this.Container.Resolve<ILogger>().LogError(exception, "Ошибка во время импорта");
                    dataTransferService.SendImportResult(guid, BaseDataResult.Error(exception.Message));
                }
                finally
                {
                    dataTransferProvider.OnSectionImportDone -= action;

                    this.Container.Release(dataTransferProvider);
                    this.Container.Release(dataTransferService);
                }
            }
        }
    }
}