namespace Bars.GkhGji.Migrations._2017.Version_2017101800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2017101800
    /// </summary>
    [Migration("2017101800")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2017.Version_2017011100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.ChangeColumn("GJI_DICT_FEATUREVIOL", new Column("NAME", DbType.String, 900, ColumnProperty.NotNull));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.ChangeColumn("GJI_DICT_FEATUREVIOL", new Column("NAME", DbType.String, 300, ColumnProperty.NotNull));
        }
    }
}