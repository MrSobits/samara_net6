namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020120400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020120400")]
    [MigrationDependsOn(typeof(Version_2020120301.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GJI_FILE_REGISTER", new RefColumn("FILE_ID", ColumnProperty.Null, "GJI_CH_REG_FILE_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.ChangeColumn("GJI_FILE_REGISTER", new RefColumn("FILE_ID", ColumnProperty.NotNull, "GJI_CH_REG_FILE_ID", "B4_FILE_INFO", "ID"));
        }

    }
}
