namespace Bars.GisIntegration.Base.Migrations.Version_2016082900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.GisIntegration.Base.Enums;

    [Migration("2016082900")]
    [MigrationDependsOn(typeof(Version_2016082500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GI_TASK", new Column("TASK_STATE", DbType.Int16, ColumnProperty.NotNull, (int)TaskState.Undefined));
            this.Database.AddColumn("GI_TASK", new Column("MESSAGE", DbType.Binary, ColumnProperty.Null));
            this.Database.AddColumn("GI_TASK_TRIGGER", new Column("TRIGGER_STATE", DbType.Int16, ColumnProperty.NotNull, (int)TriggerState.Undefined));
            this.Database.AddColumn("GI_TASK_TRIGGER", new Column("MESSAGE", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GI_TASK_TRIGGER", "MESSAGE");
            this.Database.RemoveColumn("GI_TASK_TRIGGER", "TRIGGER_STATE");
            this.Database.RemoveColumn("GI_TASK", "MESSAGE");
            this.Database.RemoveColumn("GI_TASK", "TASK_STATE");
        }
    }
}