﻿namespace Bars.Gkh.Migrations.Version_2014011300
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014011300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013122602.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //ViewManager.Drop(Database, "Gkh");
            //ViewManager.Create(Database, "Gkh");
        }

        public override void Down()
        {
            ViewManager.Drop(Database, "Gkh");
        }
    }
}