﻿namespace Bars.Gkh1468.Migrations.Version_2013120400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013120400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2013120300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GKH_OKI_PASSPORT", "STATE_ID");
            Database.RemoveColumn("GKH_HOUSE_PASSPORT", "STATE_ID");
        }

        public override void Down()
        {
            Database.AddRefColumn(
                "GKH_OKI_PASSPORT",
                new RefColumn("STATE_ID", ColumnProperty.Null, "OKI_PROV_PASP_STATE", "B4_STATE", "ID"));
            Database.AddRefColumn(
                "GKH_HOUSE_PASSPORT",
                new RefColumn("STATE_ID", ColumnProperty.Null, "HS_PASP_STATE", "B4_STATE", "ID"));
        }
    }
}