namespace Bars.Gkh.Migrations.Version_2014012301
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012301")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014012300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_ROOM", "FLOOR", DbType.Int32, ColumnProperty.Null, (object) 0);
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_ROOM", "FLOOR");
        }
    }
}