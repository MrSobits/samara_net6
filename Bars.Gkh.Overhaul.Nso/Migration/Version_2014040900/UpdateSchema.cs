using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2014040900
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014040900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2014032700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_RO_STRUCT_EL_IN_PRG", new Column("LAST_YEAR", DbType.Int32, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_RO_STRUCT_EL_IN_PRG", new Column("VOLUME", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_RO_STRUCT_EL_IN_PRG", new Column("WEAROUT", DbType.Decimal, ColumnProperty.NotNull, 0));

            Database.AddColumn("OVRHL_STAGE1_VERSION", new Column("LAST_YEAR", DbType.Int32, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_STAGE1_VERSION", new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_STAGE1_VERSION", new Column("SUM_SERVICE", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_STAGE1_VERSION", new Column("VOLUME", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_STAGE1_VERSION", new Column("WEAROUT", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG", "LAST_YEAR");
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG", "VOLUME");
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG", "WEAROUT");

            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG", "LAST_YEAR");
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG", "SUM");
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG", "SUM_SERVICE");
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG", "VOLUME");
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG", "WEAROUT");
        }
    }
}