namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017101600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017101600")]
    [MigrationDependsOn(typeof(Version_2017101000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("REGOP_PERS_ACC_OWNER_INFO", new Column("DOCUMENT_NUMBER", DbType.String, 30));
        }

        public override void Down()
        {
        }
    }
}