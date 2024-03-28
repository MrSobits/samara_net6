using System.Data;

namespace Bars.GkhGji.Migrations._2014.Version_2014112800
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2014.Version_2014112500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new Column("ABANDON_REASON", DbType.String, 512, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION", "ABANDON_REASON");
        }
    }
}