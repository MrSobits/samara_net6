namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017091600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017091600")]
    [MigrationDependsOn(typeof(Version_2017082100.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017091100.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017091300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.RemoveTable("LAWSUIT_OWNER_INFO");

            this.Database.AddEntityTable("REGOP_LAWSUIT_OWNER_INFO",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("OWNER_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("AREA_SHARE", DbType.Decimal, ColumnProperty.NotNull),
                new Column("DEBT_BASE_TARIFF_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("DEBT_DECISION_TARIFF_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("PENALTY_DEBT", DbType.Decimal, ColumnProperty.NotNull),
                new RefColumn("PERSONAL_ACCOUNT_ID", ColumnProperty.NotNull, "LAWSUIT_OWNER_INFO_PERSONAL_ACCOUNT", "REGOP_PERS_ACC", "ID"),
                new RefColumn("START_PERIOD_ID", ColumnProperty.NotNull, "LAWSUIT_OWNER_INFO_START_PERIOD", "REGOP_PERIOD", "ID"),
                new RefColumn("END_PERIOD_ID", ColumnProperty.NotNull, "LAWSUIT_OWNER_INFO_END_PERIOD", "REGOP_PERIOD", "ID"),
                new RefColumn("LAWSUIT_ID", ColumnProperty.NotNull, "LAWSUIT_OWNER_INFO_LAWSUIT", "CLW_LAWSUIT", "ID")
            );

            this.Database.AddJoinedSubclassTable("REGOP_LAWSUIT_IND_OWNER_INFO", "REGOP_LAWSUIT_OWNER_INFO", "REGOP_LAWSUIT_IND_OWNER_INFO",
                new Column("SURNAME", DbType.String, ColumnProperty.NotNull),
                new Column("FIRST_NAME", DbType.String, ColumnProperty.NotNull),
                new Column("SECOND_NAME", DbType.String, ColumnProperty.Null)
            );

            this.Database.AddJoinedSubclassTable("REGOP_LAWSUIT_LEGAL_OWNER_INFO", "REGOP_LAWSUIT_OWNER_INFO", "REGOP_LAWSUIT_LAWSUIT_OWNER_INFO",
                new Column("CONTRAGENT_NAME", DbType.String, ColumnProperty.NotNull),
                new Column("INN", DbType.String, ColumnProperty.NotNull),
                new Column("KPP", DbType.String, ColumnProperty.NotNull)
            );

            this.Database.AddUniqueConstraint("UNIQUE_PERSONAL_ACCOUNT_REGOP_LAWSUIT_OWNER_INFO",
                "REGOP_LAWSUIT_OWNER_INFO",
                "NAME",
                "PERSONAL_ACCOUNT_ID");
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_LAWSUIT_IND_OWNER_INFO");
            this.Database.RemoveTable("REGOP_LAWSUIT_LEGAL_OWNER_INFO");
            this.Database.RemoveTable("REGOP_LAWSUIT_OWNER_INFO");
        }
    }
}