namespace Bars.Gkh.Migration.Version_2013111800
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013111800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013111500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("PROJECT_DOCS", DbType.Int32, ColumnProperty.NotNull,10));
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("ENERGY_PASSPORT", DbType.Int32, ColumnProperty.NotNull, 10));
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("CONFIRM_WORK_DOCS", DbType.Int32, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "PROJECT_DOCS");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "ENERGY_PASSPORT");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "CONFIRM_WORK_DOCS");
        }
    }
}
