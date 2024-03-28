namespace Bars.Gkh.Migrations._2023.Version_2023050109
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023050109")]

    [MigrationDependsOn(typeof(Version_2023050108.UpdateSchema))]

    /// Является Version_2018080400 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_DICT_ENERGY_EFFICIENCY_CLASSES",
                new Column("CODE", DbType.String),
                new Column("DESIGNATION", DbType.String, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("DEVIATION_VALUE", DbType.String, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_DICT_ENERGY_EFFICIENCY_CLASSES");
        }
    }
}