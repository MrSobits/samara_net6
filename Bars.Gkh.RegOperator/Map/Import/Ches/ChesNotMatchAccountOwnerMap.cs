namespace Bars.Gkh.RegOperator.Map.Import
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;

    using NHibernate.Mapping.ByCode.Conformist;

    public class ChesNotMatchAccountOwnerMap : PersistentObjectMap<ChesNotMatchAccountOwner>
    {
        /// <inheritdoc />
        public ChesNotMatchAccountOwnerMap()
            : base("Несопоставленный в периоде абонент", "CHES_NOT_MATCH_ACC_OWNER")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование абонента").Column("NAME").NotNull();
            this.Property(x => x.OwnerType, "Тип абонента").Column("OWNER_TYPE").NotNull();
            this.Property(x => x.PersonalAccountNumber, "Номер ЛС").Column("PERS_ACC_NUM").NotNull();
            this.Reference(x => x.Period, "Период").Column("PERIOD_ID");
        }
    }

    public class ChesNotMatchAccountOwnerNhMaping : ClassMapping<ChesNotMatchAccountOwner>
    {
        public ChesNotMatchAccountOwnerNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}