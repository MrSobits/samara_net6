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
    /// Региональные операторы капитального ремонта
    /// </summary>
    public class RegopExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "REGOP";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<RegopProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.DocNumber,
                        this.GetDate(x.DocDate),
                        x.StaffCount.ToStr(),
                        x.ParentContragent.ToStr(),
                        x.HasPaymentInformSystem.ToStr(),
                        x.HasCrInformSystem.ToStr(),
                        x.PaymentModel.ToStr(),
                        x.PaymentDay.ToStr(),
                        x.PaymentMonth.ToStr()
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Контрагент",
                "Номер основания наделения полномочиями",
                "Дата основания наделения полномочиями",
                "Количество штатных единиц",
                "Курирующий орган государственной власти",
                "Наличие у организации информационной системы, автоматизирующей учет фонда КР и выставление платежных документов на уплату взносов",
                "Наличие у субъекта РФ информационной системы, автоматизирующей планирование и формирование программы капитального ремонта",
                "Модель выставления платежных документов на оплату взносов на капитальный ремонт:",
                "День месяца, до которого выставляются платежные документы для оплаты взносов на капитальный ремонт",
                "Месяц, в котором выставляются платежные документы для оплаты взносов на капитальный ремонт"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(ContragentExportableEntity));
        }
    }
}