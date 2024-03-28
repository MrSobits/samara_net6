namespace Bars.Gkh.Migrations.Version_2015111000
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015111000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2015110500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Накат миграции
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("SNILS", DbType.String, 50));
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "SNILS");
        }
    }
}