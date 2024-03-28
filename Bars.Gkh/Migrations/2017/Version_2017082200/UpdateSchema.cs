namespace Bars.Gkh.Migrations._2017.Version_2017082200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017082200")]
    [MigrationDependsOn(typeof(Version_2017080900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_DICT_UNITMEASURE", new Column("OKEI_CODE", DbType.String));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_DICT_UNITMEASURE", "OKEI_CODE");
        }
    }
}