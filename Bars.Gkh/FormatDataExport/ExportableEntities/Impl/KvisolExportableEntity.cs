namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// 2.16.12 Результаты квитирования (kvisol.csv)
    /// </summary>
    public class KvisolExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "KVISOL";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.RegOpCr |
            FormatDataExportProviderFlags.Ogv |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<OplataProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Id.ToStr(),
                        x.KvisolResult.ToStr(),
                        x.EpdId.ToStr(),
                        x.KvisolState.ToStr(),
                        this.GetDecimal(x.KvisolSum)
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
                case 4:
                case 5:
                    return row.Cells[cell.Key].IsEmpty();
                case 3:
                    return row.Cells[2] == "1" && row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код записи",
                "Код оплаты ЖКУ",
                "Результат квитирования",
                "Номер платежного документа",
                "Статус квитирования",
                "Сумма квитирования (в копейках)"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(OplataExportableEntity),
                typeof(EpdExportableEntity));
        }
    }
}