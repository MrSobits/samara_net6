namespace Bars.Gkh.Overhaul.Hmao.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Overhaul.Hmao.DomainService;

    /// <summary>
    /// Экспорт версии КР
    /// </summary>
    public class VersionRecordsExport : BaseDataExportService
    {
        /// <summary>
        /// Сервис модификации коллекции
        /// </summary>
        public IModifyEnumerableService ModifyEnumerableService { get; set; }

        /// <inheritdoc />
        public override IList GetExportData(BaseParams baseParams)
        {
            var stage3Service = this.Container.Resolve<IStage3Service>();

            using (this.Container.Using(stage3Service))
            {
                var data = stage3Service.ListWithStructElements(baseParams).ToList();

                if (this.ModifyEnumerableService != null)
                {
                    data = this.ModifyEnumerableService.ReplaceProperty(data, ".", x => x.RealityObject).ToList();
                }

                return data;
            }
        }
    }
}