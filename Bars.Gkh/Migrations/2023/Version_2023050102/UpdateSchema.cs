namespace Bars.Gkh.Migrations._2023.Version_2023050102
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023050102")]
    
    [MigrationDependsOn(typeof(Version_2023050101.UpdateSchema))]

    /// Является Version_2018030500 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("GKH_DICT_CATEGORY_CONSUMERS_EQUAL_POPULATION", new Column("NAME", DbType.String, 300, ColumnProperty.NotNull));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.ChangeColumn("GKH_DICT_CATEGORY_CONSUMERS_EQUAL_POPULATION", new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));
        }
    }
}