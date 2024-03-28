namespace Bars.Gkh.Migrations.Version_2014012400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014012301.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_ROOM", "FROM_PREV", DbType.Boolean, ColumnProperty.NotNull, false);
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_ROOM", "FROM_PREV");
        }
    }
}