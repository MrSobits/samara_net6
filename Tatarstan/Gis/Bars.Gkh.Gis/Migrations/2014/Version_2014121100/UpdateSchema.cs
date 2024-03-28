namespace Bars.Gkh.Gis.Migrations._2014.Version_2014121100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2014121100")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014120900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GIS_RSOFILE_LOAD_INFO", new[]
            {
                new Column("LOAD_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("ACCRUAL_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("FILE_HASH", DbType.Binary, ColumnProperty.NotNull),

                new RefColumn("LOADED_FILE_ID", "GIS_RSOFILE_LOAD_INFO_LFR", "GIS_LOADED_FILE_REGISTER", "ID")
            });
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_RSOFILE_LOAD_INFO");
        }
    }
}