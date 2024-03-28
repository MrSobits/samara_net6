namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021030100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Migration("2021030100")]
    [MigrationDependsOn(typeof(Version_2021012201.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {

            Database.AddColumn("GJI_CH_SMEV_MVD", new Column("ANSWER_INFO", DbType.String, 5000));
           
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_MVD", "ANSWER_INFO");
        }


    }
}
