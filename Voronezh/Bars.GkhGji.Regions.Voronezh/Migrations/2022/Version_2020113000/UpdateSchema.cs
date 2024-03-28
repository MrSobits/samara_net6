namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020113000
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020113000")]
    [MigrationDependsOn(typeof(Version_2020111700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GJI_APPEAL_CITIZENS", new Column("CASE_NUMBER", DbType.String, 150));
        }

        public override void Down()
        {

        }

    }
}


