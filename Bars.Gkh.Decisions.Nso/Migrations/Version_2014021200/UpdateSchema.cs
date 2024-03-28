namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014021200
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014021101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("DEC_MONTHLY_FEE", "DECISION_VALUE");
            Database.AddColumn("DEC_MONTHLY_FEE", new Column("DECISION_VALUE", DbType.String, 1000, ColumnProperty.Null));
        }

        public override void Down()
        {
        }
    }
}