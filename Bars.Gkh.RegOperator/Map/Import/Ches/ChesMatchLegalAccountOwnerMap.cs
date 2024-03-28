namespace Bars.Gkh.RegOperator.Map.Import
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;

    using NHibernate.Mapping.ByCode.Conformist;

    public class ChesMatchLegalAccountOwnerMap : JoinedSubClassMap<ChesMatchLegalAccountOwner>
    {
        /// <inheritdoc />
        public ChesMatchLegalAccountOwnerMap()
            : base("Несопоставленный ЧЭС ФЛ - Абонент", "CHES_MATCH_LEGAL_ACC_OWNER")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Inn, "ИНН").Column("INN").NotNull();
            this.Property(x => x.Kpp, "КПП").Column("KPP").NotNull();
        }
    }
    public class ChesMatchLegalAccountOwnerNhMaping : JoinedSubclassMapping<ChesMatchLegalAccountOwner>
    {
        public ChesMatchLegalAccountOwnerNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}