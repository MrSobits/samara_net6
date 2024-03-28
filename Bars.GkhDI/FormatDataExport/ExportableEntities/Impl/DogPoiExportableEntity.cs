namespace Bars.GkhDi.FormatDataExport.ExportableEntities.Impl
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

    /// <summary>
    /// Договоры на пользование общим имуществом
    /// </summary>
    public class DogPoiExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DOGPOI";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<DogPoiProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.RealityObjectId.ToStr(),
                        x.IndividualAccountId.ToStr(),
                        x.ContragentId.ToStr(),
                        x.DocumentNumber,
                        this.GetDate(x.DocumentCreateDate),
                        this.GetDate(x.DocumentStartDate),
                        this.GetDate(x.DocumentPlanedEndDate),
                        this.GetDate(x.ActionEndDate),
                        x.Subject,
                        x.Comment,
                        this.GetDecimal(x.CostContract),
                        x.DestinationPayment,
                        x.Status.ToStr(),
                        x.RevocationReason
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
                case 4:
                case 5:
                case 6:
                case 7:
                case 13:
                    return row.Cells[cell.Key].IsEmpty();
                case 2:
                case 3:
                    return row.Cells[2].IsEmpty() && row.Cells[3].IsEmpty();
                case 14:
                    return row.Cells[13] == "2";
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Дом",
                "Физическое лицо (Арендатор/Наниматель)",
                "Организация (Арендатор/Наниматель)",
                "Номер договора",
                "Дата заключения договора",
                "Дата начала действия договора ",
                "Планируемая дата окончания действия договора",
                "Дата окончания действия",
                "Предмет договора",
                "Комментарий",
                "Размер платы за предоставление в пользование части общего имущества собственников помещений в МКД",
                "Направления расходования средств, внесённых за пользование частью общего имущества",
                "Статус",
                "Причина аннулирования"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            var entityCodeList = this.GetEntityCodeList(typeof(ContragentExportableEntity),
                typeof(HouseExportableEntity));
            entityCodeList.Add("IND");

            return entityCodeList;
        }
    }
}