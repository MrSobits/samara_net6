namespace Bars.Gkh.Migrations._2023.Version_2023050107
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023050107")]

    [MigrationDependsOn(typeof(Version_2023050106.UpdateSchema))]

    /// Является Version_2018041800 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("GKH_FORMAT_DATA_EXPORT_TASK",
                new RefColumn("USER_ID", "GKH_FORMAT_DATA_EXPORT_TASK_USER", "B4_USER", "ID"));
        }
    }
}