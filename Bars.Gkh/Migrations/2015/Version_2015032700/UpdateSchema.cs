namespace Bars.Gkh.Migrations.Version_2015032700
{
    using Bars.Gkh.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015032700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2015032300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        #region Overrides of Migration

        public override void Up()
        {
            if (Database.ColumnExists("b4_task_entry", "task_code"))
            {
                Database.AlterColumnSetNullable("b4_task_entry", "executor_code", true);
            }
        }

        public override void Down()
        {
        }

        #endregion
    }
}