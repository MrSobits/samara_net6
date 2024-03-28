namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013091301
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091301")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013091300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять эту миграцию

            /*Database.AddEntityTable("OVRHL_DICT_JOB", 
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "OVRHL_DICT_JOB_WORK", "GKH_DICT_WORK", "ID"));

            Database.RemoveRefColumn("OVRHL_DICT_WORK_PRICE", "WORK_ID");
            Database.RemoveColumn("OVRHL_DICT_WORK_PRICE", "NAME");

            Database.AddRefColumn("OVRHL_DICT_WORK_PRICE", new RefColumn("JOB_ID", "WRK_PRC_JOB", "OVRHL_DICT_JOB", "ID"));

            Database.RemoveRefColumn("OVRHL_STRUCT_EL_WORK", "WORK_ID");
            Database.AddRefColumn("OVRHL_STRUCT_EL_WORK", new RefColumn("JOB_ID", "STR_EL_WRK_JOB", "OVRHL_DICT_JOB", "ID"));*/
        }

        public override void Down()
        {
            /*Database.RemoveEntityTable("OVRHL_DICT_JOB");

            Database.RemoveRefColumn("OVRHL_DICT_WORK_PRICE", "JOB_ID");

            Database.AddColumn("OVRHL_DICT_WORK_PRICE", new Column("NAME", DbType.String, 300, ColumnProperty.NotNull, string.Empty));
            Database.AddRefColumn("OVRHL_DICT_WORK_PRICE", new RefColumn("WORK_ID", "WRK_PRC_WORK", "GKH_DICT_WORK", "ID"));

            Database.RemoveRefColumn("OVRHL_STRUCT_EL_WORK", "JOB_ID");
            Database.AddRefColumn("OVRHL_STRUCT_EL_WORK", new RefColumn("WORK_ID", "STR_EL_WRK_WRK", "GKH_DICT_WORK", "ID"));*/
        }
    }
}