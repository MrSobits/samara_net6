namespace Bars.GkhGji.Migrations.Version_2013071800
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013071800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013071100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ExecuteNonQuery(@"update gji_appeal_citizens ac set file_id=null where not exists (select f.id from b4_file_info f where f.id=ac.file_id) and ac.file_id is not null");
            Database.AddForeignKey("FK_GJI_APPEAL_CIT_FILE", "GJI_APPEAL_CITIZENS", "FILE_ID", "B4_FILE_INFO", "ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_APPEAL_CITIZENS", "FK_GJI_APPEAL_CIT_FILE");
        }
    }
}