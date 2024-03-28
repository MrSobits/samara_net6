namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016040801
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016040801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2016040100.UpdateSchema))]

    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
	        this.Database.AddEntityTable(
		        "GKH_CONSTRUCT_OBJ_PHOTO",
		        new Column("DATE", DbType.Date),
		        new Column("NAME", DbType.String, ColumnProperty.Null, 150),
		        new Column("PHOTO_GROUP", DbType.Int32, 4, ColumnProperty.Null, 10),
		        new Column("DESCRIPTION", DbType.String, 500),
		        new RefColumn("OBJECT_ID", ColumnProperty.NotNull, "GKH_CONSTRUCT_OBJ_PHOTO_OBJ", "GKH_CONSTRUCTION_OBJECT", "ID"),
		        new RefColumn("FILE_ID", ColumnProperty.Null, "GKH_CONSTRUCT_OBJ_PHOTO_OBJ_F", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_CONSTRUCT_OBJ_PHOTO");
        }
    }
}
