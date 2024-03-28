namespace Bars.Gkh.RegOperator.Migrations._2019.Version_2019112900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019112800")]
   
    [MigrationDependsOn(typeof(Version_2019081200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            Database.AddColumn("REGOP_INDIVIDUAL_ACC_OWN", new Column("DOCUMENT_ISSUEDED_ORG", DbType.String, 255));
           
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "DOCUMENT_ISSUEDED_ORG");
           
        }
    }
}