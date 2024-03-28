namespace Bars.GkhGji.Migrations._2017.Version_2017110200
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017110200")]
    [MigrationDependsOn(typeof(Version_2017101900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RemoveTable("GJI_APPEAL_METHOD");
            this.Database.RemoveTable("GJI_DICT_APPEAL_METHOD");
        }

        public override void Down()
        {
        }
    }
}
