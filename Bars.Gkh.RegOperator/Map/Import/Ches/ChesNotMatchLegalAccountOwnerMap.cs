namespace Bars.Gkh.RegOperator.Map.Import
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;

    using NHibernate.Mapping.ByCode.Conformist;

    public class ChesNotMatchLegalAccountOwnerMap : JoinedSubClassMap<ChesNotMatchLegalAccountOwner>
    {
        /// <inheritdoc />
        public ChesNotMatchLegalAccountOwnerMap()
            : base("Несопоставленный ЧЭС ФЛ - Абонент", "CHES_NOT_MATCH_LEGAL_ACC_OWNER")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Inn, "ИНН").Column("INN").NotNull();
            this.Property(x => x.Kpp, "КПП").Column("KPP").NotNull();
        }
    }
    public class ChesNotMatchLegalAccountOwnerNhMaping : JoinedSubclassMapping<ChesNotMatchLegalAccountOwner>
    {
        public ChesNotMatchLegalAccountOwnerNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}