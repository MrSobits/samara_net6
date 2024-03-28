namespace Bars.GkhGji.Migrations._2024.Version_2024030101
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030101")]
    [MigrationDependsOn(typeof(Version_2024030100.UpdateSchema))]
    /// Является Version_2018040600 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_INSPECTION_JURPERSON", new Column("PLAN_NUMBER", DbType.Int32, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_INSPECTION_JURPERSON", "PLAN_NUMBER");
        }
    }
}