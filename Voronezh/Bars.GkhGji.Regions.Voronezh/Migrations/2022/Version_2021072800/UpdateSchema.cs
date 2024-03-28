namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021072800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021072800")]
    [MigrationDependsOn(typeof(Version_2021052600.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("KIND_KND", DbType.Int32, 4, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "KIND_KND");
        }


    }
}
