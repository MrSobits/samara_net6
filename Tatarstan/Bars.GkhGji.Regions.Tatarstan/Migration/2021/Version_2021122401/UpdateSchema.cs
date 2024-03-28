namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021122401
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2021122401")]
    [MigrationDependsOn(typeof(Tatarstan.Migration.Version_2018090400.UpdateSchema))]
    [MigrationDependsOn(typeof(Tatarstan.Migration.Version_2018090700.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2021121200.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2021122100.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2021122400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable("GJI_VISIT_SHEET",
                "GJI_DOCUMENT",
                "GJI_DOCUMENT_VISIT_SHEET",
                new RefColumn("EXECUTING_INSPECTOR_ID",
                    "GJI_VISIT_SHEET_INSPECTOR",
                    "GKH_DICT_INSPECTOR",
                    "ID"),
                new Column("VISIT_DATE_START", DbType.Date),
                new Column("VISIT_DATE_END", DbType.DateTime),
                new Column("VISIT_TIME_START", DbType.DateTime),
                new Column("VISIT_TIME_END", DbType.DateTime),
                new Column("HAS_COPY", DbType.Int16, ColumnProperty.NotNull, (int) YesNoNotSet.NotSet));

            this.Database.AddEntityTable("GJI_VISIT_SHEET_INFO",
                new RefColumn("VISIT_SHEET_ID",
                    ColumnProperty.NotNull,
                    "GJI_VISIT_SHEET_INFO_VISIT_SHEET",
                    "GJI_VISIT_SHEET",
                    "ID"),
                new Column("INFO", DbType.String.WithSize(1000)),
                new Column("COMMENT", DbType.String.WithSize(1000)));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GJI_VISIT_SHEET_INFO");
            this.Database.RemoveTable("GJI_VISIT_SHEET");
        }
    }
}