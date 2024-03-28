namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013090400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013090400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013082800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять миграцию

            //Database.RemoveEntityTable("OVRHL_RO_STRUCT_EL_WORK");
        }

        public override void Down()
        {
            /*Database.AddEntityTable(
                "OVRHL_RO_STRUCT_EL_WORK",
                new RefColumn("RO_SE_ID", ColumnProperty.NotNull, "RO_SE_WORK_RO_SE", "OVRHL_RO_STRUCT_EL", "ID"),
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "RO_SE_WORK_WORK", "GKH_DICT_WORK", "ID"),
                new Column("LAST_OVERHAUL_YEAR", DbType.Int64, ColumnProperty.NotNull, 0),
                new Column("VOLUME_REPAIR", DbType.Int64, ColumnProperty.NotNull),
                new Column("TYPE_REPAIR", DbType.Int64, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, ColumnProperty.NotNull, 2000));*/
        }
    }
}