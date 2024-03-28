namespace Bars.GkhGji.Migration.Version_2014090900
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014090900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2014090200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GJI_DICT_PROVIDEDDOCUMENT", new Column("NAME", DbType.String, 2000));
            Database.ChangeColumn("GJI_DICT_TASKS_INSPECTION", new Column("NAME", DbType.String, 2000));
            Database.ChangeColumn("GJI_DICT_GOALS_INSPECTION", new Column("NAME", DbType.String, 2000));
        }

        public override void Down()
        {
            
        }
    }
}