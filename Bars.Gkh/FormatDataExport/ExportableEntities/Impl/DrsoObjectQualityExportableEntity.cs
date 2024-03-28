namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Показатели качества предметов договора ресурсоснабжения
    /// </summary>
    public class DrsoObjectQualityExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DRSOOBJECTQUALITY";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Rso;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<DrsoObjectQualityProxy>().ExtProxyListCache
                .Where(x => x.QualityType.HasValue)
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        x.DrsoId.ToStr(), // 2. Предмет договора ресурсоснабжения
                        x.QualityType.ToStr(), // 3. Показатель качества коммунальных ресурсов
                        this.GetDecimal(x.QualityValue), // 4. Значение показателя качества
                        this.GetDecimal(x.StartValue), // 5. Начало диапазона значения
                        this.GetDecimal(x.EndValue), // 6. Конец диапазона значения
                        x.OkeiCode.Cut(50) // 7. Код ОКЕИ
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1, 2, 6 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Предмет договора ресурсоснабжения",
                "Показатель качества коммунальных ресурсов",
                "Значение показателя качества",
                "Начало диапазона значения",
                "Конец диапазона значения",
                "Код ОКЕИ"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DRSOOBJECT",
                "DICTMEASURE"
            };
        }
    }
}