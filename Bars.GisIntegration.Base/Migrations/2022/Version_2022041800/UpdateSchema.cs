namespace Bars.GisIntegration.Base.Migrations._2022.Version_2022041800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022041800")]
    [MigrationDependsOn(typeof(Version_2020100100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private string table = "GI_TASK";
        private RefColumn oldColumn = new RefColumn("DISPOSAL_ID", "GKH_RIS_TASK_DISPOSAL", "GJI_DISPOSAL", "Id");
        private RefColumn newColumn = new RefColumn("DOCUMENT_ID", "GKH_RIS_TASK_DOCUMENT", "GJI_DOCUMENT", "Id");

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.RemoveConstraint(this.table, this.oldColumn.IndexAndForeignKeyName);
            this.Database.RemoveIndex(this.table, this.oldColumn.IndexAndForeignKeyName);

            this.Database.RenameColumn(this.table, this.oldColumn.Name, this.newColumn.Name);

            this.Database.AddIndex(this.newColumn.IndexAndForeignKeyName, false, this.table, this.newColumn.Name);
            this.Database.AddForeignKey(this.newColumn.IndexAndForeignKeyName, this.table, this.newColumn.Name, this.newColumn.PrimaryTable, this.newColumn.PrimaryColumn);
        }

        /// <inheritdoc />
        public override void Down()
        {
            var sql = $@"
                UPDATE {this.table} gt SET {this.newColumn.Name} = NULL
                WHERE {this.newColumn.Name} NOTNULL AND NOT EXISTS 
                    (SELECT 1 FROM {this.oldColumn.PrimaryTable} gd WHERE gd.Id = gt.{this.newColumn.Name})";
            this.Database.ExecuteNonQuery(sql);

            this.Database.RemoveConstraint(this.table, this.newColumn.IndexAndForeignKeyName);
            this.Database.RemoveIndex(this.table, this.newColumn.IndexAndForeignKeyName);

            this.Database.RenameColumn(this.table, this.newColumn.Name, this.oldColumn.Name);

            this.Database.AddIndex(this.oldColumn.IndexAndForeignKeyName, false, this.table, this.oldColumn.Name);
            this.Database.AddForeignKey(this.oldColumn.IndexAndForeignKeyName, this.table, this.oldColumn.Name, this.oldColumn.PrimaryTable, this.oldColumn.PrimaryColumn);
        }
    }
}