namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020120201
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020120201")]
    [MigrationDependsOn(typeof(Version_2020120200.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
        "GJI_DICT_APPEAL_EXECUTION_TYPE",
        new Column("CODE", DbType.String, ColumnProperty.NotNull),
        new Column("NAME", DbType.String, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_APPEAL_EXECUTION_TYPE");
        }

    }
}
