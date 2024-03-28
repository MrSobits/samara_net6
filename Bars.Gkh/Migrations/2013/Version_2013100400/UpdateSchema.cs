namespace Bars.Gkh.Migrations.Version_2013100400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013100400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013092600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("JUDGMENT_COMMON_PROP", DbType.Int32, 4, ColumnProperty.NotNull, 20));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "JUDGMENT_COMMON_PROP");
        }
    }
}