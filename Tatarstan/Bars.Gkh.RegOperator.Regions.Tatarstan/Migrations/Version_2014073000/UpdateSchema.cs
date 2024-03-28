namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Migrations.Version_20140730
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    ///     Схема миграции
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014073000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Regions.Tatarstan.Migrations.Version_2014062400.UpdateSchema))]
    public sealed class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        ///     Накат миграции
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("REGOP_CONFCONTRIB_DOC", new Column("AMOUNT", DbType.Decimal.WithSize(18, 2), ColumnProperty.Null, null));
        }

        /// <summary>
        ///     Откат миграции
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("REGOP_CONFCONTRIB_DOC", "AMOUNT");
        }
    }
}