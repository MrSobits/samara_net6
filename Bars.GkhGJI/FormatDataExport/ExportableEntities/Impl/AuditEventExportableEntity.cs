namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Мероприятия проверки
    /// </summary>
    public class AuditEventExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "AUDITEVENT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        private const string OffSite = "Провести комплексное обследование дома, направленное " +
            "на обеспечение сохранности жилищного фонда, надлежащее содержание и ремонт " +
            "конструктивных элементов, инженерных систем и придомовой территории. " +
            "В случае выявления нарушений принять предусмотренные законодательством меры по " +
            "их устранению.";

        private const string Documentary = "Провести проверку документов на соответствие " +
            "действующему жилищному законодательству.";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<AuditProxy>()
                .ExtProxyListCache
                .OrderBy(x => x.Id)
                .Select((x, i) => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Проверка
                        x.Id.ToString(), // 2. Номер
                        this.GetMessage(x.AuditForm), // 3. Мероприятие
                        string.Empty // 4. Дополнительная информация
                    }))
                .ToList();
        }

        private string GetMessage(int? auditForm)
        {
            switch (auditForm)
            {
                case 1: return AuditEventExportableEntity.Documentary;
                case 2: return AuditEventExportableEntity.OffSite;
                case 3: return AuditEventExportableEntity.OffSite;
                default: return null;
            }
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1, 2 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Проверка",
                "Номер",
                "Мероприятие",
                "Дополнительная информация"
           };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(AuditExportableEntity));
        }
    }
}