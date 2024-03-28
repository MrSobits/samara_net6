namespace Bars.GkhDi.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Услуги к документам об утверждении тарифов ЖКУ
    /// </summary>
    public class TarifUslugaExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "TARIFUSLUGA";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Ogv |
            FormatDataExportProviderFlags.Oms;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<TarifProxy>()
                .ExtProxyListCache
                .Where(x => x.CommunalServiceType.HasValue)
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.CommunalServiceType.ToStr(),
                        this.GetDecimal(x.TarifCost),
                        x.TarifComponentCount.ToStr(),
                        x.TarifRateCount.ToStr()
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Документ об утверждении тарифов ЖКУ",
                "Вид коммунальной услуги",
                "Тариф",
                "Количество компонентов в цене тарифа",
                "Количество ставок тарифа"
            };
        }
    }
}