namespace Bars.Gkh.Migrations._2022.Version_2022032400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022032400")]
    
    [MigrationDependsOn(typeof(_2022.Version_2022031700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_PERSON_CERTIFICATE", new RefColumn("STATE_ID", "GKH_PERSON_CERTIFICATE_ST", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "STATE_ID");
        }
    }
}