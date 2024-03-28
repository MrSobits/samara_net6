namespace Bars.Gkh.Ris.Migrations.Version_2016050200
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция модуля
    /// </summary>
    [Migration("2016050200")]
    [MigrationDependsOn(typeof(Version_2016021800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //this.Database.AddRisEntityTable("RIS_ORGANIZATION_WORK",
            //    new Column("NAME", DbType.String, 100),
            //    new Column("SERVICE_TYPE_CODE", DbType.String, 20),
            //    new Column("SERVICE_TYPE_GUID", DbType.String, 40),
            //    new Column("REQUIRED_SERVICES", DbType.Binary),
            //    new Column("OKEI", DbType.String, 3),
            //    new Column("STRING_DIMENSION_UNIT", DbType.String, 100));
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveTable("RIS_ORGANIZATION_WORK");
        }
    }
}