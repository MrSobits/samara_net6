namespace Bars.GkhGji.Migrations._2024.Version_2024030112
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030112")]
    [MigrationDependsOn(typeof(Version_2024030111.UpdateSchema))]
    /// Является Version_2019112100 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DISPOSAL", new Column("KIND_KND", DbType.Int32, 4, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DISPOSAL", "KIND_KND");
        }
    }
}