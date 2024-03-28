namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Migrations.Version_2014062400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Regions.Tatarstan.Migrations.Version_2014060600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_TRANSFER_HIRE",
                new RefColumn("REC_ID", ColumnProperty.NotNull, "REGOP_TRANSFER_HIRE_R", "RF_TRANSFER_RECORD", "ID"),
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "REGOP_TRANSFER_HIRE_A", "REGOP_PERS_ACC", "ID"),
                new Column("TRANSFERRED", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("TRANSFERRED_SUM", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_TRANSFER_HIRE");
        }
    }
}