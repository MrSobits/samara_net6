namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Оплата по договорам на  выполнение работ по  капитальному ремонту(paydogov.csv)
    /// </summary>
    public class PayDogovExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "PAYDOGOV";

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
                        x.ContractId.ToStr(),
                        x.State.ToStr(),
                        x.PaymentType.ToStr(),
                        x.ContragentRecipient.ToStr(),
                        x.ContragentPayer.ToStr(),
                        this.GetDate(x.PaymentDate),
                        this.GetDecimal(x.PaymentSum)
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
                "Уникальный код оплаты",
                "Код договора на выполнение работ по капитальному ремонту",
                "Статус",
                "Вид оплаты",
                "Получатель",
                "Плательщик",
                "Дата оплаты",
                "Сумма оплаты"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(DogovorPkrExportableEntity), typeof(ContragentExportableEntity));
        }
    }
}