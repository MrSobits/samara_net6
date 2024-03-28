namespace Bars.GkhGji.Migrations._2024.Version_2024030100
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030100")]
    [MigrationDependsOn(typeof(Version_2024022700.UpdateSchema))]
    /// Является Version_2018032100 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_INSPECTION_BASIS",
                new Column("CODE", DbType.String, 255, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 1000, ColumnProperty.NotNull));
            this.Database.AddIndex("GJI_INSPECTION_BASIS_CODE", true, "GJI_INSPECTION_BASIS", "CODE");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_INSPECTION_BASIS");
        }
    }
}