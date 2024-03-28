namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016031500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016031500
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016031500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016030600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.ChangeColumn("B4_FIAS_ADDRESS", new Column("LETTER", DbType.String, 100));
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
        }
    }
}
