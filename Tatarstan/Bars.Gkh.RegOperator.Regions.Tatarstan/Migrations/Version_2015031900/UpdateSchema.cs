namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Migrations.Version_2015031900
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    ///     Схема миграции
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015031900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Regions.Tatarstan.Migrations.Version_20140730.UpdateSchema))]
    public sealed class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        ///     Накат миграции
        /// </summary>
        public override void Up()
        {
            Database.RemoveConstraint("REGOP_CONFCONTRIB_DOC", "FK_CONF_DOC_CONTRIB");
            Database.AddForeignKey("FK_CONF_DOC_CONTRIB", "REGOP_CONFCONTRIB_DOC", "CONFIRMCONTRIB_ID", "REGOP_CONFCONTRIB", "ID");
        }

        /// <summary>
        ///     Откат миграции
        /// </summary>
        public override void Down()
        {
            Database.RemoveConstraint("REGOP_CONFCONTRIB_DOC", "FK_CONF_DOC_CONTRIB");
            Database.AddForeignKey("FK_CONF_DOC_CONTRIB", "REGOP_CONFCONTRIB_DOC", "CONFIRMCONTRIB_ID", "GKH_REALITY_OBJECT", "ID");
        }
    }
}