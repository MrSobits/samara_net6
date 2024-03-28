namespace Bars.GkhGji.Migrations._2017.Version_2017041800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    /// <summary>
    /// Миграция 2017041800
    /// </summary>
    [Migration("2017041800")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2017.Version_2017041100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_INSPECTION_LIC_APP", new Column("INSPECTION_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10));
        }


        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_INSPECTION_LIC_APP", "INSPECTION_TYPE");
        }
    }
}