namespace Bars.GkhDi.Migrations.Version_2016031500
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция номер [2016031500]
    /// </summary>
    [Migration("2016031500")]
    [MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2016021000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.ChangeDefaultValue("DI_DISINFO_DOC_RO", "HAS_GEN_MEET_OWNERS", 30);
            this.Database.ExecuteNonQuery("update DI_DISINFO_DOC_RO set HAS_GEN_MEET_OWNERS = 30 where HAS_GEN_MEET_OWNERS = 0");
        }
    }
}
