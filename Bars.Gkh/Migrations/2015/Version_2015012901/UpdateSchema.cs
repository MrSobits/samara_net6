﻿using System.Data;

namespace Bars.Gkh.Migrations.Version_2015012901
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015012901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015012900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("VTSCP_CODE", DbType.String, 50));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "VTSCP_CODE");
        }
    }
}