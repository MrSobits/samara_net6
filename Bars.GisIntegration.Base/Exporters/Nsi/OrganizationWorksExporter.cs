namespace Bars.GisIntegration.Base.Exporters.Nsi
{
    using System;
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Nsi;
    using Bars.GisIntegration.Base.Tasks.SendData.Nsi;

    /// <summary>
    /// Класс экспортер записей справочника «Работы и услуги организации»
    /// </summary>
    public class OrganizationWorksExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт записей справочника «Работы и услуги организации»";

        /// <summary>
        /// Порядок импорта в списке
        /// </summary>
        public override int Order => 50;

        /// <summary>
        /// Описание экспортера
        /// </summary>
        public override string Description => "Экспорт записей справочника «Работы и услуги организации»";

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
        //    var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");

            try
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
              //      dataProviderExporter.Name,
                    "Экспортировать список справочников",
                    "Экспортировать данные справочника"
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
             //   this.Container.Release(dataProviderExporter);
            }
        }

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportOrganizationWorksTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(OrganizationWorksPrepareDataTask);
    }
}