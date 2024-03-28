namespace Bars.Gkh.Migrations._2017.Version_2017080400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2017080400")]
    [MigrationDependsOn(typeof(Version_2017080100.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017072000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        { 
            this.Database.AddEntityTable(
                "GKH_ACTIVITY_STAGE",
                new Column("ENTITY_ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("ENYITY_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.Null),
                new Column("DATE_END", DbType.DateTime, ColumnProperty.Null),
                new Column("ACTIVITY_STAGE_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, ColumnProperty.Null),
                new FileColumn("FILE_ID", ColumnProperty.Null, "GKH_ACTIVITY_STAGE_FILE"));

            this.Database.AddIndex("ACT_STAGE_ENTITY_ID_TYPE_IND", false, "GKH_ACTIVITY_STAGE", "ENTITY_ID", "ENYITY_TYPE");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_ACTIVITY_STAGE");
        }
    }
}