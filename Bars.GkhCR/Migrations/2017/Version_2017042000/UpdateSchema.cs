namespace Bars.GkhCr.Migrations._2017.Version_2017042000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2017042000")]
    [MigrationDependsOn(typeof(Version_2017032200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("CR_OBJ_BUILD_CONTRACT", "TERMINATION_DATE", DbType.DateTime);
            this.Database.AddColumn("CR_OBJ_BUILD_CONTRACT", "TERMINATION_REASON", DbType.String);
            this.Database.AddRefColumn("CR_OBJ_BUILD_CONTRACT",
                new FileColumn("TERMINATION_DOCUMENT_FILE_ID", "CR_OBJ_BUILD_CONTRACT_TERMINATION_DOCUMENT_FILE"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "TERMINATION_DATE");
            this.Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "TERMINATION_REASON");
            this.Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "TERMINATION_DOCUMENT_FILE_ID");
        }
    }
}