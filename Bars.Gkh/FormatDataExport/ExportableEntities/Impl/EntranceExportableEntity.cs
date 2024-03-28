namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Подъезды
    /// </summary>
    public class EntranceExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "ENTRANCE";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.RegOpCr |
            FormatDataExportProviderFlags.Oms |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<EntranceProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.HouseId.ToStr(),
                        x.Number.ToStr(),
                        this.GetNotZeroValue(x.FloorSize),
                        this.GetDate(x.BuildDate),
                        this.GetDate(x.TerminationDate),
                        x.IsSupplierConfirmed.ToStr()
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => new List<int> { 0, 1, 2 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код подъезда в системе отправителя",
                "Уникальный идентификатор дома",
                "Номер подъезда",
                "Этажность",
                "Дата постройки",
                "Дата прекращения существования объекта",
                "Информация подтверждена поставщиком, ответственным за размещение сведений"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(HouseExportableEntity));
        }
    }
}