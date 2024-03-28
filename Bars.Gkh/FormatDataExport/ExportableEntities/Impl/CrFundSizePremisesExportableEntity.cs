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
    /// Информация о размере фонда КР по помещениям (crfundsizepremises.csv)
    /// </summary>
    public class CrFundSizePremisesExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "CRFUNDSIZEPREMISES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<CrFundSizePremisesProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.CrFundSizeId.ToStr(),
                        x.PersAccountId.ToStr(),
                        this.GetDecimal(x.OverpaymentOrDebtOnStartPeriod),
                        this.GetDecimal(x.Contribution),
                        this.GetDecimal(x.Penalty),
                        this.GetDecimal(x.PaidContribution),
                        this.GetDecimal(x.PaidPenalty),
                        this.GetDecimal(x.OverpaymentOrDebtOnEndPeriod)
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
                "Информация о размере фонда КР по дому",
                "Лицевой счет",
                "Задолженность/переплата на начало периода",
                "Начислено взносов за период",
                "Начислено пени за период",
                "Уплачено взносов за период",
                "Уплачено пени за период",
                "Задолженность/переплата на конец периода"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(CrFundSizeExportableEntity), typeof(KvarExportableEntity));
        }
    }
}