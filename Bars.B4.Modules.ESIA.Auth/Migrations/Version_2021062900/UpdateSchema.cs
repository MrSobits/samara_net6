namespace Bars.B4.Modules.ESIA.Auth.Migrations.Version_2021062900
{
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2021062900")]
    [MigrationDependsOn(typeof(Version_1.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            var sql = @"ALTER TABLE esia_operator ALTER org_name TYPE character varying(300);
                        ALTER TABLE esia_operator ALTER org_postion TYPE character varying(300);
                        ALTER TABLE esia_operator ALTER org_shortname TYPE character varying(300);
                        ALTER TABLE esia_operator ALTER org_addresses TYPE character varying(500);
                        ALTER TABLE esia_operator ALTER org_legalform TYPE character varying(200);
                        ";
        }
    }
}