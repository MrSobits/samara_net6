﻿namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013123000
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013123000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013122400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_STAGE1_VERSION", new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_STAGE1_VERSION", new Column("SUM_SERVICE", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_STAGE1_VERSION", new Column("VOLUME", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_STAGE1_VERSION", "SUM");
            Database.RemoveColumn("OVRHL_STAGE1_VERSION", "SUM_SERVICE");
            Database.RemoveColumn("OVRHL_STAGE1_VERSION", "VOLUME");
        }
    }
}