﻿namespace Bars.Gkh.Migrations.Version_2014112600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014112500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_ORG_FORM", new Column("OKOPF_CODE", DbType.String, 50, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_ORG_FORM", "OKOPF_CODE");
        }
    }
}