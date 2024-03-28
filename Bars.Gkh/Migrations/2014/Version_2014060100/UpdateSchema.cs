namespace Bars.Gkh.Migrations._2014.Version_2014060100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.MigrationsVersion_2014052400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_ROOM", "NOTATION", DbType.String, 300);
            Database.AddColumn("GKH_ROOM", "HAS_NO_NUM", DbType.Boolean, ColumnProperty.NotNull, false);
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_ROOM", "NOTATION");
            Database.RemoveColumn("GKH_ROOM", "HAS_NO_NUM");
        }
    }
}