﻿namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014050700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014050700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014041100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_NSO_DISP_VERIFSUBJ",
                new Column("TYPE_VERIF_SUBJ", DbType.Int32),
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "GJI_NSO_DISP_VERIFSUBJ_D", "GJI_DISPOSAL", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_NSO_DISP_VERIFSUBJ");
        }
    }
}