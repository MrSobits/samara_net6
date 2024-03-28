namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021012201
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Migration("2021012201")]
    [MigrationDependsOn(typeof(Version_2021012200.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {

            Database.AddColumn("GJI_CH_SMEV_EGRN", new Column("CADASTRAL_NUMBER", DbType.String, 50));
           
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_EGRN", "CADASTRAL_NUMBER");
        }


    }
}
