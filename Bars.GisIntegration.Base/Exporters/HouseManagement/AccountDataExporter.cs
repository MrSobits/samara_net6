namespace Bars.GisIntegration.Base.Exporters.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement;
    using Bars.GisIntegration.Base.Tasks.SendData.HouseManagement;
    using Base.Exporters;

    /// <summary>
    /// Класс экспортер данных ЛС
    /// </summary>
    public class AccountDataExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование метода
        /// </summary>
        public override string Name => "Экспорт сведений о лицевых счетах";

        /// <summary>
        /// Получить список методов, от которых зависит текущий
        /// </summary>
        /// <returns>Список методов, от которых зависит текущий</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");

            try
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
                    "Экспорт сведений по дому", //любой из 3-х методов по домам
                    "Экспорт списка справочников",
                    "Экспорт данных справочников"
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
            }
        }

        /// <summary>
        /// Описание экспортера
        /// </summary>
        public override string Description => "Операция позволяет экспортировать в ГИС ЖКХ информацию о лицевых счетах поставщика информации. Сведения передаются в разрезе одного дома и включают в себя информацию о плательщике(собственнике/арендаторе), сведения о доле.";

        /// <summary>
        /// Порядок экспортера в списке
        /// </summary>
        public override int Order => 160;

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportAccountDataTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        /// <returns>Тип задачи подготовки данных</returns>
        public override Type PrepareDataTaskType => typeof(AccountPrepareDataTask);
    }
}
