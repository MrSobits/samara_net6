namespace Bars.Gkh.Migrations._2016.Version_2016011200
{
    /// <summary>
    /// Миграция 12.01.2016
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016011200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015122500.UpdateSchema))]
    public class UpdateSchema: global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            if (this.Database.TableExists("B4_LOG_ENTITY"))
            {
                var oldTimeout = this.Database.CommandTimeout;
                try
                {
                    // очень долгая операция
                    this.Database.CommandTimeout = 3 * 60 * 60;
                    this.Database.AddIndex("B4_LOG_ENTITY_ENTITY_TYPE", false, "B4_LOG_ENTITY", "ENTITY_TYPE");
                    this.Database.AddIndex("B4_LOG_ENTITY_ENTITY_ID", false, "B4_LOG_ENTITY", "ENTITY_ID");
                }
                finally
                {
                    this.Database.CommandTimeout = oldTimeout;
                }
            }
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            if (this.Database.TableExists("B4_LOG_ENTITY"))
            {
                this.Database.RemoveIndex("B4_LOG_ENTITY_ENTITY_TYPE", "B4_LOG_ENTITY");
                this.Database.RemoveIndex("B4_LOG_ENTITY_ENTITY_ID", "B4_LOG_ENTITY");
            }
        }
    }
}
