namespace Bars.GkhGji.Migrations._2022.Version_2022053000
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022053000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022052700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("CIT_TYPE_OF_FEEDBACK",
                 new RefColumn("APPCIT_ID", "GJI_APPCIT_DECISION_TOF", "GJI_APPEAL_CITIZENS", "ID"),
                 new RefColumn("CIT_TOF_ID", "GJI_APPCIT_TOF", "GJI_DICT_TYPE_OF_FEEDBACK", "ID"),
                 new RefColumn("FILE_TOF_ID", "GJI_APPCIT_FILE_TOF", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("CIT_TYPE_OF_FEEDBACK");
        }
    }
}