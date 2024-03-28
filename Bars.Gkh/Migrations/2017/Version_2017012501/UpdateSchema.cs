namespace Bars.Gkh.Migrations._2017.Version_2017012501
{    
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Добавить связь между журналом импортов и задачей сервера вычислений, которая обрабатывала импорт
    /// </summary>
    [Migration("2017012501")]
    [MigrationDependsOn(typeof(Version_2017011100.UpdateSchema))] // Логической зависимости нет. Просто, предыдущая по порядку миграция.
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddRefColumn("GKH_LOG_IMPORT", new RefColumn("TASK_ID", "GKH_LOG_IMPORT_TASK", "B4_TASK_ENTRY", "ID"));
        }

        /// <summary>
        /// Отменить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_LOG_IMPORT", "TASK_ID");
        }
    }
}
