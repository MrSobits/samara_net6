namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2022053100
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022053100")]
    [MigrationDependsOn(typeof(Version_2022032400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_APPCIT_ANSWER_EXECUTION_TYPE",
                 new RefColumn("APPCIT_ANSWER_ID", "GJI_APPCIT_ANSWER_EXECUTION_ANSWER", "GJI_APPCIT_ANSWER", "ID"),
                 new RefColumn("AE_TYPE_ID", "GJI_APPCIT_ANSWER_EXECUTION_DICT_EXECTYPE", "GJI_DICT_APPEAL_EXECUTION_TYPE", "ID"));

        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_APPCIT_ANSWER_EXECUTION_TYPE");
        }
    }
}