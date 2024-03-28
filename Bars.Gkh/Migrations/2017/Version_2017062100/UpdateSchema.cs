namespace Bars.Gkh.Migrations._2017.Version_2017062100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017062100")]
    [MigrationDependsOn(typeof(Version_2017052000.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017052200.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017061400.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017061500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_GENERAL_STATE_HISTORY",
                new Column("CHANGE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("ENTITY_ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("ENTITY_TYPE", DbType.String, 200, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 200, ColumnProperty.NotNull),
                new Column("START_STATE", DbType.String, 200),
                new Column("FINAL_STATE", DbType.String, 200, ColumnProperty.NotNull),
                new Column("USER_NAME", DbType.String, 100),
                new Column("USER_LOGIN", DbType.String, 100)
                );

            this.Database.AddIndex("IND_GENERAL_STATE_HISTORY_ID_TYPE", false, "GKH_GENERAL_STATE_HISTORY", "ENTITY_ID", "CODE");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_GENERAL_STATE_HISTORY");
        }
    }
}