namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015070300
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015070300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015062500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_UNACCEPT_CHARGE",
                new RefColumn("STATE_ID", "REGOP_UCHARGE_ST", "B4_STATE", "ID"));

            Database.AddColumn("REGOP_UNACCEPT_CHARGE",
                new Column("FUND_FORMATION_TYPE", DbType.Int16, ColumnProperty.NotNull, -1));

            Database.AddColumn("REGOP_UNACCEPT_CHARGE",
                new Column("CONTRAGENT_ACC_NUM", DbType.String, 100));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_UNACCEPT_CHARGE", "STATE_ID");
            Database.RemoveColumn("REGOP_UNACCEPT_CHARGE", "FUND_FORMATION_TYPE");
            Database.RemoveColumn("REGOP_UNACCEPT_CHARGE", "CONTRAGENT_ACC_NUM");
        }
    }
}
