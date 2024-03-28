namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Migrations.Version_2015051300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    ///     Схема миграции
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015051300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Regions.Tatarstan.Migrations.Version_2015031900.UpdateSchema))]
    public sealed class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("REGOP_CONFCONTRIB_DOC", new Column("AMOUNT", new ColumnType(DbType.Decimal)));
        }

        public override void Down()
        {
            //Нечего откатывать, менялся только размер колонки
        }
    }
}
