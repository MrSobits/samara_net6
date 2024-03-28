namespace Bars.Gkh.Export
{
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.Modules.DataExport.Domain;
    using DomainService;

    /// <summary>
    /// Экспорт Жилого дома, когда нет RegOperator в проекте
    /// </summary>
    public class RealityObjectDataExport : BaseDataExportService
    {
        /// <summary>
        /// Сервис жилого дома
        /// </summary>
        public IRealityObjectService RealityObjectService { get; set; }

        /// <summary>
        /// Сервис модификации коллекции
        /// </summary>
        public IModifyEnumerableService ModifyEnumerableService { get; set; }

        /// <summary>
        /// Метод получения Данных для Экспорта
        /// </summary>
        public override IList GetExportData(BaseParams baseParams)
        {
            var data = this.RealityObjectService.GetRealityObjectsQuery(baseParams).ToList();

            if (this.ModifyEnumerableService != null)
            {
                data = this.ModifyEnumerableService.ReplaceProperty(data,
                        ".",
                        x => x.Address,
                        x => x.FullAddress,
                        x => x.PlaceName,
                        x => x.Municipality,
                        x => x.Settlement)
                    .ToList();
            }

            return data;
        }
    }
}