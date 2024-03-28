namespace Bars.Gkh.Migrations._2015.Version_2015090200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015090200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015082400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_ENTRANCE",
                new RefColumn("RO_ID", ColumnProperty.NotNull, "GKH_ENTRANCE_ROID", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("RET_ID", "GKH_ENTRANCE_RETID", "OVRHL_REAL_ESTATE_TYPE", "ID"),
                new Column("CNUMBER", DbType.Int16, ColumnProperty.NotNull, 0));

            Database.AddRefColumn("GKH_ROOM", new RefColumn("ENTRANCE_ID", "GKH_ROOM_ENTR", "GKH_ENTRANCE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_ENTRANCE");

            Database.RemoveColumn("GKH_ROOM", "ENTRANCE_ID");
        }
    }
}