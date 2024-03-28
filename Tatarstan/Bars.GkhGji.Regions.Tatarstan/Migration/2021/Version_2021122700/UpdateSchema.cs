namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021122700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2021122700")]
    [MigrationDependsOn(typeof(Version_2021122402.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName documentTable =
            new SchemaQualifiedObjectName { Name = "GJI_DOCUMENT_PREVENTIVE_ACTION_TASK" };
        
        private readonly SchemaQualifiedObjectName plannedActionTable =
            new SchemaQualifiedObjectName { Name = "PREVENTIVE_ACTION_TASK_PLANNED_ACTION" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable
            (
                this.documentTable.Name,
                "GJI_DOCUMENT",
                this.documentTable.Name + "_GJI_DOCUMENT",
                new Column("ACTION_TYPE", DbType.Int32),
                new Column("VISIT_TYPE", DbType.Int32),
                new Column("COUNSELING_TYPE", DbType.Int32),
                new Column("ACTION_DATE", DbType.Date),
                new Column("ACTION_START_TIME", DbType.DateTime),
                new Column("STRUCTURAL_SUBDIVISION", DbType.String.WithSize(500)),
                new Column("NOTIFICATION_DATE", DbType.Date),
                new Column("OUTGOING_LETTER_DATE", DbType.Date),
                new Column("NOTIFICATION_SENT", DbType.Int32),
                new Column("NOTIFICATION_TYPE", DbType.Int32),
                new Column("NOTIFICATION_DOCUMENT_NUMBER", DbType.String.WithSize(25)),
                new Column("OUTGOING_LETTER_NUMBER", DbType.String.WithSize(25)),
                new Column("NOTIFICATION_RECIEVED", DbType.Int32),
                new Column("PARTICIPATION_REJECTION", DbType.Int32, ColumnProperty.NotNull, (int) YesNo.No),
                new RefColumn("EXECUTOR_ID", documentTable.Name + "_EXECUTOR", "GKH_DICT_INSPECTOR", "ID"),
                new RefColumn("TASKING_INSPECTOR_ID", documentTable.Name + "_TASKING_INSPECTOR", "GKH_DICT_INSPECTOR", "ID"),
                new RefColumn("ACTION_LOCATION_ID", documentTable.Name + "_ACTION_LOCATION", "B4_FIAS_ADDRESS", "ID")
            );

            this.Database.AddEntityTable
            (
                plannedActionTable.Name,
                new Column("ACTION", DbType.String.WithSize(1000)),
                new Column("COMMENTARY", DbType.String.WithSize(1000)),
                new RefColumn("TASK_ID", ColumnProperty.NotNull, this.plannedActionTable.Name + "_PREVENTIVE_ACTION_TASK", "GJI_DOCUMENT_PREVENTIVE_ACTION_TASK", "ID")
            );
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.plannedActionTable);
            this.Database.RemoveTable(this.documentTable);
        }
    }
}