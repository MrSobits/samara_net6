namespace Bars.GisIntegration.Base.Exporters.Infrastructure
{
    using System;
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Infrastructure;
    using Bars.GisIntegration.Base.Tasks.SendData.Infrastructure;

    /// <summary>
    /// Экспортёр ОКИ
    /// </summary>
    public class RkiDataExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Управление ОКИ в РКИ";

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description => "Операция позволяет организациям с полномочиями «РСО», «ОМС» выполнять основные действия с ОКИ в РКИ: Создание, Редактирование, Удаление";

        /// <summary>
        /// Получить список методов, от которых зависит текущий
        /// </summary>
        /// <returns>Список методов, от которых зависит текущий</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
       //     var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");

            try
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
       //             dataProviderExporter.Name,
                    "Экспорт списка справочников",
                    "Экспорт данных справочников"
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
       //         this.Container.Release(dataProviderExporter);
            }
        }

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 130;

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportRkiDataTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(RkiPrepareDataTask);

        /// <summary>
        /// Наименование хранилища данных ГИС для загрузки вложений
        /// </summary>
        public override FileStorageName? FileStorage => FileStorageName.Rki;
    }
}