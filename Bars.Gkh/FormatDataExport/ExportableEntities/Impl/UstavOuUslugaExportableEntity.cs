﻿namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Оказываемые услуги по уставу
    /// </summary>
    public class UstavOuUslugaExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "USTAVOUUSLUGA";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<UstavOuUslugaProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.OuId.ToStr(),
                        x.ServiceId.ToStr(),
                        this.GetDate(x.StartDate),
                        this.GetDate(x.EndDate),
                        x.IsServiceByThisContract.ToStr(),
                        this.GetStrId(x.ProtocolMeetingOwnersFile)
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
                case 4:
                    return row.Cells[cell.Key].IsEmpty();
                case 5:
                    if (row.Cells[4] == "2")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Объект управления",
                "Услуга (коммунальная или дополнительная)",
                "Дата начала предоставления услуги",
                "Дата окончания предоставления услуги",
                "Услуга предоставляется в рамках текущего устава",
                "Файл с протоколом собрания собственников"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(UstavExportableEntity),
                typeof(UstavOuExportableEntity),
                typeof(DictUslugaExportableEntity));
        }
    }
}