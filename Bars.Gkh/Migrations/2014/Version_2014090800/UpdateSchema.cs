namespace Bars.Gkh.Migrations.Version_2014090800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014090800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014070300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CONTRAGENT", new Column("TYPE_ENTREPRENEUR", DbType.Int16, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT", "TYPE_ENTREPRENEUR");
        }
    }
}