namespace Bars.GkhDi.Migrations.Version_2013070200
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013070200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013062700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_DICT_GROUP_WORK_PPR", new Column("IS_NOT_ACTUAL", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("DI_DICT_GROUP_WORK_PPR", new Column("EXTERNAL_ID", DbType.String, 36));

            Database.AddColumn("DI_DICT_WORK_TO", new Column("IS_NOT_ACTUAL", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("DI_DICT_WORK_TO", new Column("EXTERNAL_ID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_DICT_GROUP_WORK_PPR", "IS_NOT_ACTUAL");
            Database.RemoveColumn("DI_DICT_GROUP_WORK_PPR", "EXTERNAL_ID");

            Database.RemoveColumn("DI_DICT_WORK_TO", "IS_NOT_ACTUAL");
            Database.RemoveColumn("DI_DICT_WORK_TO", "EXTERNAL_ID");
        }
    }
}