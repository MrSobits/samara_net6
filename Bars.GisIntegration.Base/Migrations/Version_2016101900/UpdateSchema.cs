namespace Bars.GisIntegration.Base.Migrations.Version_2016101900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2016101900")]
    [MigrationDependsOn(typeof(Version_2016101400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GI_CONTRAGENT", new Column("ACCREDITATION_RECORD_NUMBER", DbType.String));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GI_CONTRAGENT", "ACCREDITATION_RECORD_NUMBER");
        }
    }
}