namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020100600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2020100600")]
    [MigrationDependsOn(typeof(Version_2020092300.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PRELIMINARY_CHECK", new Column("RESULT_ENUM", DbType.Int32, 4, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PRELIMINARY_CHECK", "RESULT_ENUM");
        }
    }
}


