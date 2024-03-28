namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Маппинг <see cref="TechnicalMistake"/>
    /// </summary>
    public class TechnicalMistakeMap : BaseImportableEntityMap<TechnicalMistake>
    {
        /// <inheritdoc />
        public TechnicalMistakeMap()
            : base("Bars.Gkh.Entities.TechnicalMistake", "GKH_TECHNICAL_MISTAKE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.StatementNumber, "Номер заявления").Column("STMNT_NUMBER");
            this.Property(x => x.FixInfo, "Описание технической ошибки").Column("FIX_INFO");
            this.Property(x => x.FixDate, "Дата исправления").Column("FIX_DATE");
            this.Property(x => x.IssuedDate, "Дата получения").Column("ISSUE_DATE");
            this.Property(x => x.DecisionNumber, "Номер решения").Column("DECISION_NUMBER");
            this.Property(x => x.DecisionDate, "Дата решения").Column("DECISION_DATE");

            this.Reference(x => x.QualificationCertificate, "Квалификационный аттестат").Column("CERTIFICATE_ID").NotNull();
            this.Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").Fetch();
        }
    }
}