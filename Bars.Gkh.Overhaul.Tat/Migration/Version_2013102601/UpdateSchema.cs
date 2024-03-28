namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013102601
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013102600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //перенесено в базовый модуль, не удалять миграцию
            /*Database.RemoveRefColumn("REAL_EST_TYPE_COMM_PARAM", "COMMON_PARAM_ID");
            Database.AddColumn("REAL_EST_TYPE_COMM_PARAM",  new Column("COMMON_PARAM_CODE", DbType.String, 500, ColumnProperty.Null));*/
        }

        public override void Down()
        {
            /*Database.AddRefColumn("REAL_EST_TYPE_COMM_PARAM",
                new RefColumn("COMMON_PARAM_ID", ColumnProperty.NotNull, "COMMON_PARAM", "OVRHL_DICT_COMMON_PARAM", "ID"));
            Database.RemoveColumn("REAL_EST_TYPE_COMM_PARAM", "COMMON_PARAM_CODE");*/
        }
    }
}