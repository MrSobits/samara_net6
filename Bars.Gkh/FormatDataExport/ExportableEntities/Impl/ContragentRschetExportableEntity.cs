namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Расчетные счета
    /// </summary>
    public class ContragentRschetExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "CONTRAGENTRSCHET";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<ContragentRschetProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.SettlementAccount?.Replace(" ", string.Empty).Cut(20),
                        x.BankContragentId.ToStr(),
                        x.ContragentId.ToStr(),
                        x.CorrAccount?.Replace(" ", string.Empty).Cut(20),
                        this.GetDate(x.OpenDate),
                        this.GetDate(x.CloseDate)
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1, 2, 3, 5 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код расчетного счета в системе отправителя",
                "Расчетный счет",
                "Банк",
                "Контрагент",
                "Корреспондентский счет",
                "Дата открытия",
                "Дата закрытия"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(ContragentExportableEntity), typeof(BankExportableEntity));
        }
    }
}