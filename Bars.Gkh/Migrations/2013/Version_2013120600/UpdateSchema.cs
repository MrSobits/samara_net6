namespace Bars.Gkh.Migrations.Version_2013120600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013120600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013120500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("CHECK_CERTIFICATE", DbType.Boolean, ColumnProperty.NotNull, true));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "CHECK_CERTIFICATE");
        }
    }
}