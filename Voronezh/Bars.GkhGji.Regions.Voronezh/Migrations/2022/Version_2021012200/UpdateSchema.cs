namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021012200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Migration("2021012200")]
    [MigrationDependsOn(typeof(Version_2021011500.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {

            Database.AddColumn("GJI_CH_SMEV_SOCIAL_HIRE", new Column("MESSAGE_ID", DbType.String, 500));
           
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_SOCIAL_HIRE", "MESSAGE_ID");
        }


    }
}
