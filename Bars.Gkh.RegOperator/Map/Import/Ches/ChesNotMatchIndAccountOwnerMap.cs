namespace Bars.Gkh.RegOperator.Map.Import
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;

    using NHibernate.Mapping.ByCode.Conformist;

    public class ChesNotMatchIndAccountOwnerMap : JoinedSubClassMap<ChesNotMatchIndAccountOwner>
    {
        /// <inheritdoc />
        public ChesNotMatchIndAccountOwnerMap()
            : base("Несопоставленный ЧЭС ФЛ - Абонент", "CHES_NOT_MATCH_IND_ACC_OWNER")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Surname, "Фамилия").Column("SURNAME");
            this.Property(x => x.Firstname, "Имя").Column("FIRSTNAME").NotNull();
            this.Property(x => x.Lastname, "Отчество").Column("LASTNAME");
            this.Property(x => x.BirthDate, "Дата рождения").Column("BIRTH_DATE");
        }
    }
    public class ChesNotMatchIndAccountOwnerNhMaping : JoinedSubclassMapping<ChesNotMatchIndAccountOwner>
    {
        public ChesNotMatchIndAccountOwnerNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}