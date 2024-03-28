namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2014011500
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014011500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2014011400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять

            //Database.AddColumn("OVRHL_COMMON_ESTATE_OBJECT", new Column("IS_MAIN", DbType.Boolean, ColumnProperty.NotNull, false));
            //Database.AddColumn("OVRHL_REAL_ESTATE_TYPE", new Column("MARG_REPAIR_COST", DbType.Decimal));
        }

        public override void Down()
        {
            //Database.RemoveColumn("OVRHL_COMMON_ESTATE_OBJECT", "IS_MAIN");
            //Database.RemoveColumn("OVRHL_REAL_ESTATE_TYPE", "MARG_REPAIR_COST");
        }
    }
}