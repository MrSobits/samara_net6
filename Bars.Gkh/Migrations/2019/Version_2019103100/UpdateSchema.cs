namespace Bars.Gkh.Migrations._2019.Version_2019103100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019103100")]
    [MigrationDependsOn(typeof(Version_2019103000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_DICT_LIVING_SQUARE_COST",
                new Column("COST", DbType.Decimal),
                new Column("YEAR",DbType.Int32),
                new RefColumn("MO_ID", ColumnProperty.NotNull, "FK_LSC_MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID")

                );
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_DICT_LIVING_SQUARE_COST");
        }
    }
}