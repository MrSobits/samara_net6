namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013090500
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013090500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013090400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять миграцию

            /*Database.AddColumn("OVRHL_RO_STRUCT_EL", new Column("WEAROUT", DbType.Decimal, ColumnProperty.NotNull, 0));

            Database.RemoveColumn("OVRHL_RO_STRUCT_EL", "FORMULA_PARAMS");
            Database.AddColumn("OVRHL_STRUCT_EL_GROUP", new Column("FORMULA_PARAMS_BIN", DbType.Binary, ColumnProperty.Null));*/
        }

        public override void Down()
        {
            /*Database.RemoveColumn("OVRHL_RO_STRUCT_EL", "WEAROUT");

            Database.RemoveColumn("OVRHL_RO_STRUCT_EL", "FORMULA_PARAMS_BIN");
            Database.AddColumn(
                "OVRHL_STRUCT_EL_GROUP", new Column("FORMULA_PARAMS", DbType.String, 4000, ColumnProperty.Null));*/
        }
    }
}