namespace Bars.GkhGji.Regions.Stavropol.FormatDataExport.ExportableEntities
{
    using System.Collections.Generic;

    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;

    /// <summary>
    /// Нормативные правовые акты
    /// </summary>
    public class NpaExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "NPA";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return new List<ExportableRow>();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>();
        }
    }
}