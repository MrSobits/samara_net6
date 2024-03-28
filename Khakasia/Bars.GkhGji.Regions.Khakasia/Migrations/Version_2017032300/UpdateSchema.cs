namespace Bars.GkhGji.Regions.Khakasia.Migrations.Version_2017032300
{
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция Version_2017032300
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017032300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Khakasia.Migrations.Version_2014120500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Применить
        /// </summary>
        public override void Up()
        {
            this.Database.AlterColumnSetNullable("GJI_APPCIT_EXECUTANT", "PERFOM_DATE", true);
        }

        /// <summary>
        /// Откатить 
        /// </summary>
        public override void Down()
        {
        }
    }
}