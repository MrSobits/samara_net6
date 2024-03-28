namespace Bars.Gkh.Migrations._2017.Version_2017021000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017021000")]
    [MigrationDependsOn(typeof(Version_2017020100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("OVRHL_STRUCT_EL", new Column("REFORM_CODE", DbType.String, 300));
            this.Database.AddColumn("GKH_REALITY_OBJECT", new Column("CULT_HER_ASSIGN_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_STRUCT_EL", "REFORM_CODE");
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "CULT_HER_ASSIGN_DATE");
        }
    }
}