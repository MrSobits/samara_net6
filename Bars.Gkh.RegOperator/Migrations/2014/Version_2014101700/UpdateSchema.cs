﻿namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014101700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014101700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014101601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_TRANSFER", new Column("IS_AFFECT", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_TRANSFER", "IS_AFFECT");
        }
    }
}
