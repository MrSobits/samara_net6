namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013103100
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013103100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013102802.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять миграцию

            //Database.AddColumn("OVRHL_DICT_WORK_PRICE", new Column("SQUARE_METER_COST", DbType.Decimal));
        }

        public override void Down()
        {
            //Database.RemoveColumn("OVRHL_DICT_WORK_PRICE", "SQUARE_METER_COST");
        }
    }
}