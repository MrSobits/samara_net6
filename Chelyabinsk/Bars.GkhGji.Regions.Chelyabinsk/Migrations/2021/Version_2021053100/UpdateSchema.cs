﻿namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2021053100
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2021053100")]
    [MigrationDependsOn(typeof(Version_2021041200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_SMEV_EGRN", new Column("CADASTRAL_NUMBER", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_EGRN", "CADASTRAL_NUMBER");
        }
    }
}