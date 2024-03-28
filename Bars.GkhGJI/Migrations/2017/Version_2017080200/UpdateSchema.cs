namespace Bars.GkhGji.Migrations._2017.Version_2017080200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017080200")]
    [MigrationDependsOn(typeof(Version_2017072700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_DOCUMENT", new Column("LITERAL_NUM", DbType.String));
        }

        public override void Down()
        {
           this.Database.RemoveColumn("GJI_DOCUMENT", "LITERAL_NUM");
        }
    }
}