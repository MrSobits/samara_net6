namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013120700
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013120700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013120600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять эту миграцию

            /*Database.AddEntityTable("OVRHL_DICT_PAYSIZE",
                new Column("PAYMENT_SIZE", DbType.Decimal, ColumnProperty.NotNull),
                new Column("DATE_START_PERIOD", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END_PERIOD", DbType.DateTime));

            Database.AddEntityTable("OVRHL_PAYSIZE_MU_RECORD",
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "OV_PAYSIZE_MUREC_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("PAYSIZECR_ID", ColumnProperty.NotNull, "OV_PAYSIZE_MUREC_PS", "OVRHL_DICT_PAYSIZE", "ID"));*/
        }

        public override void Down()
        {
            /*Database.RemoveEntityTable("OVRHL_PAYSIZE_MU_RECORD");
            Database.RemoveEntityTable("OVRHL_DICT_PAYSIZE");*/
        }
    }
}