namespace Bars.GkhGji.Migrations.Version_2013121800
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013100300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_GOALS_INSPECTION", "CODE", DbType.String, 100);
            Database.AddColumn("GJI_DICT_LEGFOUND_INSPECT", "CODE", DbType.String, 100);
            Database.AddColumn("GJI_DICT_KIND_INSPECTION", "CODE", DbType.String, 100);
            Database.AddColumn("GJI_DICT_TASKS_INSPECTION", "CODE", DbType.String, 100);
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_GOALS_INSPECTION", "CODE");
            Database.RemoveColumn("GJI_DICT_LEGFOUND_INSPECT", "CODE");
            Database.RemoveColumn("GJI_DICT_KIND_INSPECTION", "CODE");
            Database.RemoveColumn("GJI_DICT_TASKS_INSPECTION", "CODE");
        }
    }
}