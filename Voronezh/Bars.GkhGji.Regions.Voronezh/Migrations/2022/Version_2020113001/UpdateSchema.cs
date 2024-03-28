namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020113001
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020113001")]
    [MigrationDependsOn(typeof(Version_2020113000.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPCIT_RESOLUTION", new Column("EXECUTED", DbType.Int32, 4, ColumnProperty.NotNull, 30)); //по умолчанию не задано
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPCIT_RESOLUTION", "EXECUTED"); 
        }

    }
}


