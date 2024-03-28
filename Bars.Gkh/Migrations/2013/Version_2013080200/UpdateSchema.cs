namespace Bars.Gkh.Migrations.Version_2013080200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013080200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013080101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_OBJ_METDEV_WRK",
               new Column("METDEV_ID", DbType.Int64, ColumnProperty.NotNull));

            Database.AddIndex("IND_OBJ_METDEV_WRK_ELM", false, "GKH_OBJ_METDEV_WRK", "METDEV_ID");
            Database.AddForeignKey("FK_OBJ_METDEV_WRK_LFT", "GKH_OBJ_METDEV_WRK", "METDEV_ID", "GKH_OBJ_COMMET_DEV", "ID");
            Database.AddForeignKey("FK_OBJ_METDEV_WRK_BASE", "GKH_OBJ_METDEV_WRK", "ID", "GKH_OBJ_BASE_WRK", "ID");
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_OBJ_METDEV_WRK");
        }
    }
}