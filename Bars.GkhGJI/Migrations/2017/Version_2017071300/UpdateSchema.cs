namespace Bars.GkhGji.Migrations._2017.Version_2017071300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    [Migration("2017071300")]
    [MigrationDependsOn(typeof(Version_2017061300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_INSPECTION", new Column("CONTROL_TYPE", DbType.Int32));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_INSPECTION", "CONTROL_TYPE");
        }
    }
}