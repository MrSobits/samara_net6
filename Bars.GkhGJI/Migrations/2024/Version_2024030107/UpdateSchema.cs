namespace Bars.GkhGji.Migrations._2024.Version_2024030107
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030107")]
    [MigrationDependsOn(typeof(Version_2024030106.UpdateSchema))]
    /// Является Version_2019052800 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RemoveTable("GJI_CATEGORY_RISK_UK");
        }

        public override void Down()
        {
            // ничего
        }
    }
}