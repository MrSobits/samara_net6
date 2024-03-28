namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Объекты управления устава
    /// </summary>
    public class UstavOuExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "USTAVOU";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var data = this.ProxySelectorFactory.GetSelector<UstavProxy>()
                .ExtProxyListCache;

            var protocolFile = this.AddFilesToExport(data, x => x.ProtocolMeetingFile);

            return this.AddFilesToExport(protocolFile, x => x.ProtocolMeetingExcludeFile)
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Id.ToStr(),
                        x.RealityObjectId.ToStr(),
                        this.GetDate(x.StartDate),
                        this.GetDate(x.EndDate),
                        x.StatusOu.ToStr(),
                        x.IsContractReason.ToStr(),
                        this.GetStrId(x.ProtocolMeetingFile),
                        x.IsManagementContract.ToStr(),
                        x.IsExclusionReason.ToStr(),
                        this.GetStrId(x.ProtocolMeetingExcludeFile),
                        this.GetDate(x.ExcludeDate)
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 5:
                case 6:
                case 8:
                    return row.Cells[cell.Key].IsEmpty();
                case 7:
                    return row.Cells[6] == "2" && row.Cells[cell.Key].IsEmpty();
                case 9:
                case 11:
                    return row.Cells[5] == "2" && row.Cells[cell.Key].IsEmpty();
                case 10:
                    return row.Cells[5] == "2" && row.Cells[9] == "2" && row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Устав",
                "Дом",
                "Дата начала предоставления услуг дому",
                "Дата окончания предоставления услуг дому",
                "Статус объекта управления устава",
                "Основанием является договор управления",
                "Файл с протоколом собрания собственников",
                "Управление многоквартирным домом осуществляется управляющей организацией по договору управления",
                "Основание исключения",
                "Файл с протоколом собрания собственников",
                "Дата исключения объекта управления из ДУ"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(UstavExportableEntity),
                typeof(HouseExportableEntity));
        }
    }
}