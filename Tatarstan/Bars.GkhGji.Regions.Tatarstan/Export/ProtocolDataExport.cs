namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    using System;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Выгрузка для <see cref="Protocol"/>
    /// </summary>
    public class ProtocolDataExport : BaseDataExportService
    {
        /// <inheritdoc />
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProtocolService>();

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