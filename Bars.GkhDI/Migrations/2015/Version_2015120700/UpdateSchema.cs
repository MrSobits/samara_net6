namespace Bars.GkhDi.Migrations.Version_2015120700
{
    using System.Data;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    ///     Схема миграции
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015120700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2015111800.UpdateSchema))]
    public sealed class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("DI_RORESORG_SERVICE",
                new RefColumn("GKH_OBJ_RESORG_ID", "REGOP_RO_SRV_RESORG", "GKH_OBJ_RESORG", "ID"),
                new RefColumn("DI_TMP_SRV_ID", "EGOP_RO_SRV_TMP_SRV", "DI_DICT_TEMPL_SERVICE", "ID"),
                new Column("STARTDATE", DbType.DateTime),
                new Column("ENDDATE", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveTable("DI_RORESORG_SERVICE");
        }
    }
}
