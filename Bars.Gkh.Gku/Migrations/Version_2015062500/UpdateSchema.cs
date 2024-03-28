namespace Bars.Gkh.Gku.Migrations.Version_2015062500
{
    using System;
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015062500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gku.Migrations.Version_2014052400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if(Database.ColumnExists("GKH_GKU_TARIFF","CONTRACTOR_ID"))
                Database.RemoveColumn("GKH_GKU_TARIFF", "CONTRACTOR_ID");
        }

        public override void Down()
        {
            
        }
    }
}