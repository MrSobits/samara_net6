namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013102300
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013102200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль, не удалять миграцию
            /*Database.AddEntityTable(
                "REAL_EST_TYPE_COMM_PARAM",
                new Column("MIN", DbType.String, 500, ColumnProperty.NotNull),
                new Column("MAX", DbType.String, 500, ColumnProperty.NotNull),
                new RefColumn("REAL_EST_TYPE_ID", ColumnProperty.NotNull, "COMM_PAR_REAL_EST_TYPE", "OVRHL_REAL_ESTATE_TYPE", "ID"),
                new RefColumn("COMMON_PARAM_ID", ColumnProperty.NotNull, "COMMON_PARAM", "OVRHL_DICT_COMMON_PARAM", "ID"));

            Database.AddEntityTable(
                "REAL_EST_TYPE_STRUCT_EL",
                new RefColumn("REAL_EST_TYPE_ID", ColumnProperty.NotNull, "STR_EL_REAL_EST_TYPE", "OVRHL_REAL_ESTATE_TYPE", "ID"),
                new RefColumn("STRUCT_EL_ID", ColumnProperty.NotNull, "EST_TYPE_STRUCT_EL", "OVRHL_STRUCT_EL", "ID"));*/
        }

        public override void Down()
        {
            /*Database.RemoveEntityTable("REAL_EST_TYPE_COMM_PARAM");
            Database.RemoveEntityTable("REAL_EST_TYPE_STRUCT_EL");*/
        }
    }
}