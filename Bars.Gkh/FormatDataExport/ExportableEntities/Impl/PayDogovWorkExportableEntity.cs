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
    /// Оплаты по работе в  договоре на  выполнение работ по  капитальному ремонту(paydogovwork.csv)
    /// </summary>
    public class PayDogovWorkExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "PAYDOGOVWORK";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<PayDogovProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Id.ToStr(),
                        x.WorkId.ToStr(),
                        this.GetDecimal(x.OwnerSum),
                        this.GetDecimal(x.SupportSum)
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1, 2 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код оплаты",
                "Код оплаты по договору",
                "Работа по дому",
                "Сумма оплаты за счет средств собственников",
                "Сумма оплаты за счет средств поддержки"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(PayDogovExportableEntity), typeof(WorkDogovExportableEntity));
        }
    }
}