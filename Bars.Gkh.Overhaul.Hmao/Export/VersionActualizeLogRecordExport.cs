namespace Bars.Gkh.Overhaul.Hmao.Export
{
    using System;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;

    /// <summary>
    /// Сервис Export'а для <see cref="VersionActualizeLogRecord"/>
    /// </summary>
    public class VersionActualizeLogRecordExport : BaseDataExportService
    {
        /// <inheritdoc />
        public override IList GetExportData(BaseParams baseParams)
        {
            var versionActualizeLogRecordViewModel = this.Container.Resolve<IViewModel<VersionActualizeLogRecord>>();
            var versionActualizeLogRecordDomain = this.Container.ResolveDomain<VersionActualizeLogRecord>();

            using (this.Container.Using(versionActualizeLogRecordViewModel, versionActualizeLogRecordDomain))
            {
                baseParams.Params.Add("isExport", true);
                var result = versionActualizeLogRecordViewModel.List(versionActualizeLogRecordDomain, baseParams);

                return result.Success
                    ? (IList)result.Data
                    : throw new Exception($"Произошла ошибка при выгрузке: {result.Message}");
            }
        }
    }
}