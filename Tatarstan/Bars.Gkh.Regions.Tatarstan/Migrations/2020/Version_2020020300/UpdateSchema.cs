namespace Bars.Gkh.Regions.Tatarstan.Migrations._2020.Version_2020020300

{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020020300")]
    [MigrationDependsOn(typeof(_2019.Version_2019122400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_OUTDOOR_IMAGE",
                new Column("DATE_IMAGE", DbType.DateTime),
                new Column("NAME", DbType.String, 100),
                new Column("IMAGES_GROUP", DbType.Int32, 4),
                new Column("DESCRIPTION", DbType.String, 300),
                new RefColumn("OUTDOOR_ID", "GKH_OUTDOOR_IMAGE_OUTDOOR", "GKH_REALITY_OBJECT_OUTDOOR", "ID"),
                new RefColumn("PERIOD_ID", "GKH_OUTDOOR_IMAGE_PERIOD", "GKH_DICT_PERIOD", "ID"),
                new RefColumn("WORK_ID", "GKH_OUTDOOR_IMAGE_WORK", "CR_DICT_WORK_OUTDOOR", "ID"),
                new RefColumn("FILE_ID", "GKH_OUTDOOR_IMAGE_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_OUTDOOR_IMAGE");
        }
    }
}