namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;

    /// <summary>
    /// Класс экспорта в файл Excel данных реестра "Предостережение"/>
    /// </summary>
    public class WarningDocDataExport : BaseDataExportService
    {
        /// <summary>
        /// Получить данные для экспорта
        /// </summary>
        /// <param name="baseParams">
        /// dateStart - период с
        /// dateEnd - период по
        /// realityObjectId - жилой дом
        /// </param>
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IWarningDocService>();

            using (this.Container.Using(service))
            {
                baseParams.Params.Add("isExport", true);
                var result = service.ListView(baseParams);
                return result.Success
                    ? (IList)result.Data
                    : throw new Exception($"Произошла ошибка при выгрузке: {result.Message}");
            }
        }
    }
}