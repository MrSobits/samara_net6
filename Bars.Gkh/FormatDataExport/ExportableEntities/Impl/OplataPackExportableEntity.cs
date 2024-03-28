namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Пачки оплат ЖКУ
    /// </summary>
    public class OplataPackExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "OPLATAPACK";

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> {0, 1, 2, 3, 4};

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags => FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.RegOpCr |
            FormatDataExportProviderFlags.Ogv |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Дата платежного поручения",
                "Номер платежного поручения",
                "Сумма платежей",
                "Количество платежей, вошедших в пачку",
                "Статус записи"
            };
        }

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<OplataPackProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        this.GetDate(x.Date),
                        x.Number.Cut(20),
                        this.GetDecimal(x.Sum),
                        x.Count.ToStr(),
                        x.State.ToStr()
                    }))
                .ToList();
        }
    }
}