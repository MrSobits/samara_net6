namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019112200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2019112200")]
    [MigrationDependsOn(typeof(Version_2019112100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("GJI_DICT_MANDATORY_REQS", new Column("mandratory_req_content", DbType.String.WithSize(600)));
        }
    }
}
