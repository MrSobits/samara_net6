namespace Bars.GkhGji.Migrations._2017.Version_2017072700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017072700")]
    [MigrationDependsOn(typeof(Version_2017071300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_RISK_CATEGORY_UK",
                new Column("NAME", DbType.String, 200, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 100, ColumnProperty.NotNull),
                new Column("RISK_FROM", DbType.Int32,  ColumnProperty.NotNull),
                new Column("RISK_TO",   DbType.Int32,  ColumnProperty.NotNull));
        }

        public override void Down()
        {
           this.Database.RemoveTable("GJI_RISK_CATEGORY_UK");
        }
    }
}