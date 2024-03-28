namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2020040900
{
    using B4.Modules.Ecm7.Framework;
    using Gkh;
    using System.Data;

    [Migration("2020040900")]
    [MigrationDependsOn(typeof(Version_2020011400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_APPCIT_ORDER", new Column("PERSON_PHONE", DbType.String));


        }
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_APPCIT_ORDER", "PERSON_PHONE");

        }
    }
}