namespace Bars.Gkh.Migrations._2018.Version_2018011900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018011900")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2017.Version_2017121902.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("CLW_RESTRUCT_DEBT", new Column("REASON", DbType.String, 500));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CLW_RESTRUCT_DEBT", "REASON");
        }
    }
}