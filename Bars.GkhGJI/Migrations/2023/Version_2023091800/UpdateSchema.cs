namespace Bars.GkhGji.Migrations._2023.Version_2023091800
{
    using B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023091800")]
    [MigrationDependsOn(typeof(_2023.Version_2023091400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_EMAIL", new Column("SYS_NUMBER", DbType.String));
            Database.AddColumn("GJI_EMAIL", new Column("LIV_ADDRESS", DbType.String,1000));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_EMAIL", "LIV_ADDRESS");
            Database.RemoveColumn("GJI_EMAIL", "SYS_NUMBER");

        }
    }
}