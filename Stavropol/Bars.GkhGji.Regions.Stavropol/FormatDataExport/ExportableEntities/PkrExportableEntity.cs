namespace Bars.GkhGji.Regions.Stavropol.FormatDataExport.ExportableEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Impl;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Программы капитального ремонта (pkr.csv)
    /// Для Ставропольского края
    /// </summary>
    public class PkrExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "PKR";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Ogv;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<PkrProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Type.ToStr(),
                        x.Level.ToStr(),
                        x.Name,
                        x.Oktmo,
                        this.GetDate(x.StartDate),
                        this.GetDate(x.EndDate),
                        x.IsFormedOnBaseRegPrograms.ToStr(),
                        x.Purpose.Cut(100),
                        x.Task.Cut(100),
                        x.StateCustomer.ToStr(),
                        x.MainDeveloper,
                        x.Executor,
                        x.State.ToStr()
                    }))
                .Distinct()
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
                case 10:
                case 13:
                    return row.Cells[cell.Key].IsEmpty();
                
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Вид программы",
                "Уровень программы",
                "Наименование",
                "Код ОКТМО",
                "Месяц и год начала программы",
                "Месяц и год окончания программы",
                "Программа сформирована на основании региональной программы",
                "Цель программы",
                "Задачи программы",
                "Государственный заказчик (Координатор программы)",
                "Основной разработчик программы",
                "Исполнитель программы",
                "Статус программы"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(ContragentExportableEntity),
                typeof(PkrDocExportableEntity),
                typeof(PkrDocFilesExportableEntity));
        }
    }
}