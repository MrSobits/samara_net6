namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022011300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022011300")]
    [MigrationDependsOn(typeof(Version_2022011200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName preventiveActionTaskItemTable =
            new SchemaQualifiedObjectName { Name = "PREVENTIVE_ACTION_TASK_ITEM", Schema = "PUBLIC" };

        private readonly string gjiDictPreventiveActionItemsOldTableName = "GJI_DICT_PREVENTIVE_ITEMS";
        private readonly string gjiDictPreventiveActionItemsNewTableName = "GJI_DICT_PREVENTIVE_ACTION_ITEMS";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.RenameTableAndSequence(this.gjiDictPreventiveActionItemsOldTableName,
                this.gjiDictPreventiveActionItemsNewTableName);
            
            this.Database.AddEntityTable(this.preventiveActionTaskItemTable.Name,
                new RefColumn("TASK_ID",
                    this.preventiveActionTaskItemTable.Name + "_TASK",
                    "GJI_DOCUMENT_PREVENTIVE_ACTION_TASK",
                    "ID"),
                new RefColumn("ITEM_ID",
                    this.preventiveActionTaskItemTable.Name + "_ITEM",
                    this.gjiDictPreventiveActionItemsNewTableName,
                    "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.preventiveActionTaskItemTable);
            this.Database.RenameTableAndSequence(this.gjiDictPreventiveActionItemsNewTableName,
                this.gjiDictPreventiveActionItemsOldTableName);
        }
    }
}