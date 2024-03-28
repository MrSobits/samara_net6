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
    /// 2.16.13 Результаты квитирования по услугам ЕПД (kvisolservice.csv)
    /// </summary>
    public class KvisolServiceExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "KVISOLSERVICE";

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
                .Where(x => x.KvisolResult == 1)
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Id.ToStr(),
                        x.KvisolServiceType.ToStr(),
                        string.Empty,
                        x.EpdCapitalId.ToStr(),
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        this.GetDecimal(x.KvisolSum),
                        x.KvisolState.ToStr()
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
                "Результат квитирования",
                "Тип начисления",
                "Начисление за ЖКУ",
                "Начисление по капитальному ремонту",
                "Начисления по страховым продуктам",
                "Начисления по неустойкам и судебным расходам",
                "Задолженность",
                "Сумма квитирования (в копейках)",
                "Статус квитирования"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(KvisolExportableEntity),
                typeof(EpdCapitalExportableEntity));
        }
    }
}