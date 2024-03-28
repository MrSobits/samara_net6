namespace Bars.GisIntegration.Base.Exporters.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement;
    using Bars.GisIntegration.Base.Tasks.SendData.HouseManagement;

    /// <summary>
    /// Экспортёр новостей для информирования граждан
    /// </summary>
    public class NotificationDataExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт новостей для информирования граждан";

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description => "Операция позволяет создание, изменение, удаление новости, а также отправка оповещения о новости адресатам через сервисы интеграции ГИС ЖКХ. Одновременно могут быть переданы данные для ВИ по созданию/редактированию, отправке адресатам, удалению новости.";

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 150;

        /// <summary>
        /// Получить список методов, от которых зависит текущий
        /// </summary>
        /// <returns>Список методов, от которых зависит текущий</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
            //var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");

            try
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
          //          dataProviderExporter.Name
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
         //       this.Container.Release(dataProviderExporter);
            }
        }

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportNotificationDataTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(NotificationPrepareDataTask);

        /// <summary>
        /// Наименование хранилища данных ГИС для загрузки вложений
        /// </summary>
        public override FileStorageName? FileStorage => FileStorageName.HomeManagement;
    }
}