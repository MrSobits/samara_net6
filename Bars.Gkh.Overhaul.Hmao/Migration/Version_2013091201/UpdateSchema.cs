namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013091201
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091201")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013091200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять эту миграцию

            //Database.AddColumn("OVRHL_COMMON_ESTATE_OBJECT", new Column("WEIGHT", DbType.Int64, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_RO_STRUCT_EL_IN_PRG_3", new Column("POINT", DbType.Decimal, ColumnProperty.NotNull, 0m));
            Database.AddColumn("OVRHL_RO_STRUCT_EL_IN_PRG_3", new Column("INDEX_NUM", DbType.Int64, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            //Database.RemoveColumn("OVRHL_COMMON_ESTATE_OBJECT", "WEIGHT");
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG_3", "POINT");
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG_3", "INDEX_NUM");
        }
    }
}