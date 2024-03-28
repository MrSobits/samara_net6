using System.Data;
using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.Migrations.Version_2015012300
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015012300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015012001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                     "GKH_PARAMETER",
                     new Column("KEY", DbType.String, 100, ColumnProperty.NotNull),
                     new Column("VALUE", DbType.String, 250),
                     new Column("PREFIX", DbType.String, 100));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_PARAMETER");
        }
    }
}