namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Договоры на пользование общим имуществом
    /// </summary>
    public class PoiExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "POI";

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
                        x.KindCommomFacilities,
                        x.AppointmentCommonFacilities,
                        this.GetDecimal(x.AreaOfCommonFacilities),
                        string.Empty, // 5. Наименование владельца
                        string.Empty, // 6. ИНН владельца
                        x.Id.ToStr(),
                        this.GetDecimal(x.CostContract)
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1, 2, 4, 5, 6, 7 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Наименование общего имущества",
                "Назначение общего имущества",
                "Площадь общего имущества",
                "Наименование владельца",
                "ИНН владельца",
                "Договор на пользование общим имуществом",
                "Стоимость по договору в месяц"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DOGPOI"
            };
        }
    }
}