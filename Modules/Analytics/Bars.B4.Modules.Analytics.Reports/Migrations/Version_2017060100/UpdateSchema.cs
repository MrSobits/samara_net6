namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2017060100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017060100")]
    [MigrationDependsOn(typeof(Version_2017041900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RemoveTable("AL_STORED_REPORT_HISTORY");

            this.Database.AddEntityTable("AL_REPORT_HISTORY",
               new Column("REPORT_TYPE", DbType.Int32, ColumnProperty.NotNull),
               new Column("REPORT_ID", DbType.Int64),
               new Column("DATE", DbType.DateTime, ColumnProperty.NotNull),
               new Column("NAME", DbType.String, 250), 
               new Column("PARAM_VALUES", DbType.Binary),
               new RefColumn("FILE_ID", ColumnProperty.NotNull, "AL_REPORT_HISTORY_FILE", "B4_FILE_INFO", "ID"),
               new RefColumn("CATEGORY_ID", ColumnProperty.NotNull, "AL_REPORT_HISTORY_CATEGORY", "B4_PRINT_FORM_CATEGORY", "ID"),
               new RefColumn("USER_ID", "AL_REPORT_HISTORY_USER", "B4_USER", "ID"));    

            this.Database.AddIndex("REPORT_HISTORY_TYPE_ID_IDX", false, "AL_REPORT_HISTORY", "REPORT_ID", "REPORT_TYPE");
        }

        public override void Down()
        {
            this.Database.RemoveTable("AL_REPORT_HISTORY");
        }
    }
}