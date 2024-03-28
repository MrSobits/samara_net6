namespace Bars.GkhGji.Migrations._2024.Version_2024030102
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030102")]
    [MigrationDependsOn(typeof(Version_2024030101.UpdateSchema))]
    /// Является Version_2018042300 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_APPEAL_CITIZENS",
                new Column("IS_PRELIMENTARY_CHECK", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_APPEAL_CITIZENS", "IS_PRELIMENTARY_CHECK");
        }
    }
}