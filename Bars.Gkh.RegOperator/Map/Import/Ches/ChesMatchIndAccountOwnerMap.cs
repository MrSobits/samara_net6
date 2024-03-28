namespace Bars.Gkh.RegOperator.Map.Import
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;

    using NHibernate.Mapping.ByCode.Conformist;

    public class ChesMatchIndAccountOwnerMap : JoinedSubClassMap<ChesMatchIndAccountOwner>
    {
        /// <inheritdoc />
        public ChesMatchIndAccountOwnerMap()
            : base("Несопоставленный ЧЭС ФЛ - Абонент", "CHES_MATCH_IND_ACC_OWNER")
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
    public class ChesMatchIndAccountOwnerNhMaping : JoinedSubclassMapping<ChesMatchIndAccountOwner>
    {
        public ChesMatchIndAccountOwnerNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}