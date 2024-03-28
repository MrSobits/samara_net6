namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2024030501
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030501")]
    [MigrationDependsOn(typeof(Version_2024030500.UpdateSchema))]
    // Является Version_2019062600 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RenameTable("OVHL_DPKR_CORRECT_ST2", "OVRHL_DPKR_CORRECT_ST2");
        }

        public override void Down()
        {
            this.Database.RenameTable("OVRHL_DPKR_CORRECT_ST2", "OVHL_DPKR_CORRECT_ST2");
        }
    }
}