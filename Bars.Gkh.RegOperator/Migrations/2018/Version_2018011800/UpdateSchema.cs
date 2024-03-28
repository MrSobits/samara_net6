namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018011800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018011800")]
    [MigrationDependsOn(typeof(Version_2018011160.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_BANK_DOC_IMPORT", new Column("REPORT_DATE", DbType.DateTime));
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_BANK_DOC_IMPORT", "REPORT_DATE");
        }
    }
}