namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019042500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Migration = Bars.B4.Modules.Ecm7.Framework.Migration;

    [Migration("2019042500")]
    [MigrationDependsOn(typeof(Version_2018102600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                            "GJI_DICT_PROSECUTOR_OFFICE",
                            new Column("TYPE", DbType.Int16, ColumnProperty.NotNull),
                            new Column("CODE", DbType.String, ColumnProperty.NotNull),
                            new Column("NAME", DbType.String.WithSize(1024), ColumnProperty.NotNull),
                            new Column("FEDERAL_DISTRICT_CODE", DbType.Int32, ColumnProperty.NotNull),
                            new Column("FEDERAL_DISTRICT_NAME", DbType.String.WithSize(300), ColumnProperty.NotNull),
                            new Column("FEDERAL_CENTER_CODE", DbType.Int32, ColumnProperty.NotNull),
                            new Column("FEDERAL_CENTER_NAME", DbType.String.WithSize(300), ColumnProperty.NotNull),
                            new Column("DISTRICT_CODE", DbType.Int32, ColumnProperty.NotNull),
                            new RefColumn("PARENT_ID", "GJI_DICT_PROSECUTOR_OFFICE_PARENT_ID", "GJI_DICT_PROSECUTOR_OFFICE", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_DICT_PROSECUTOR_OFFICE");
        }
    }
}