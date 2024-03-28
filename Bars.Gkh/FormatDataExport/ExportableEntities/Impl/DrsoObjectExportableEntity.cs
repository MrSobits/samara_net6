namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Предметы договора ресурсоснабжения
    /// </summary>
    public class DrsoObjectExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DRSOOBJECT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Rso;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<DrsoObjectProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        x.ResOrgContractId.ToStr(), // 2. Договор ресурсоснабжения
                        x.DictUslugaId.ToStr(), // 3. Код коммунальной услуги
                        x.CommunalResource.ToStr(), // 4. Тарифицируемый ресурс
                        x.SchemeConnectionType.ToStr(), // 5. Зависимая схема присоединения
                        this.GetDate(x.StartDate), // 6. Дата начала поставки ресурса
                        this.GetDate(x.EndDate), // 7. Дата окончания поставки ресурса
                        this.GetDecimal(x.PlanVolume), // 8. Плановый объем
                        x.OkeiCode, // 9. Код ОКЕИ
                        x.SubmissionMode // 10. Режим подачи
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1, 2, 3, 5, 6 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код ",
                "Договор ресурсоснабжения",
                "Код коммунальной услуги",
                "Тарифицируемый ресурс",
                "Зависимая схема присоединения",
                "Дата начала поставки ресурса",
                "Дата окончания поставки ресурса",
                "Плановый объем",
                "Код ОКЕИ",
                "Режим подачи"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DRSO",
                "DICTUSLUGA"
            };
        }
    }
}