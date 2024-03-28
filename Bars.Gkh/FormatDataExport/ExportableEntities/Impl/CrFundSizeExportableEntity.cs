namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Информация о размере фонда КР по дому (crfundsize.csv)
    /// </summary>
    public class CrFundSizeExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "CRFUNDSIZE";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.RegOpCr
            | FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<CrFundSizeProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.HouseId.ToStr(),
                        this.GetDate(x.Period),
                        x.State.ToStr(),
                        this.GetDecimal(x.FundOnStartPeriod),
                        this.GetDecimal(x.FundOnEndPeriod),
                        this.GetDecimal(x.AmountFund),
                        this.GetDecimal(x.AmountDept)
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
                "Идентификатор дома",
                "Отчетный период (месяц.год)",
                "Статус",
                "Размер фонда на начало периода",
                "Размер фонда на конец периода",
                "Сумма средств, направленных на КР за отчетный период",
                "Сумма задолженности за выполнение работ по КР на конец отчетного периода"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(HouseExportableEntity));
        }
    }
}