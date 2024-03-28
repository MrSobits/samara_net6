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
    /// Иные показатели качества предметов договора ресурсоснабжения
    /// </summary>
    public class DrsoObjectOtherQualityExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DRSOOBJECTOTHERQUALITY";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Rso;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<DrsoObjectQualityProxy>()
                .ExtProxyListCache
                .Where(x => !x.QualityType.HasValue)
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        x.DrsoId.ToStr(), // 2. Предмет договора ресурсоснабжения
                        x.QualityName.ToStr(), // 3. Наименование показателя качества
                        this.GetDecimal(x.QualityValue), // 4. Значение показателя качества
                        x.OkeiCode.Cut(50) // 5. Код ОКЕИ
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => this.GetAllFieldIds();

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Предмет договора ресурсоснабжения",
                "Наименование показателя качества",
                "Установленное значение показателя качества",
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