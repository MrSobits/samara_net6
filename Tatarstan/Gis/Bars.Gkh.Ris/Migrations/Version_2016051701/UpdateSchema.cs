namespace Bars.Gkh.Ris.Migrations.Version_2016051701
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция модуля
    /// </summary>
    [Migration("2016051701")]
    [MigrationDependsOn(typeof(Version_2016051100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //if (!this.Database.TableExists("RIS_ACCOUNT_RELATIONS"))
            //{
            //    this.Database.AddEntityTable("RIS_ACCOUNT_RELATIONS", new[]
            //    {
            //        new RefColumn("ACCOUNT_ID", "RIS_ACCOUNT_RELATIONS_ACCOUNT", "RIS_ACCOUNT", "ID"),
            //        new RefColumn("HOUSE_ID", "RIS_ACCOUNT_RELATIONS_HOUSE", "RIS_HOUSE", "ID"),
            //        new RefColumn("RESIDENTIAL_PREMISE_ID", "RIS_ACCOUNT_RELATIONS_RESIDENTIAL_PREMISE", "RIS_RESIDENTIALPREMISES", "ID"),
            //        new RefColumn("NONRESIDENTIAL_PREMISE_ID", "RIS_ACCOUNT_RELATIONS_NONRESIDENTIAL_PREMISE", "RIS_NONRESIDENTIALPREMISES", "ID"),
            //        new RefColumn("LIVING_ROOM_ID", "RIS_ACCOUNT_RELATIONS_NONRESIDENTIAL_PREMISE", "RIS_LIVINGROOM", "ID"),
            //        new RefColumn("RIS_CONTRAGENT_ID", "RIS_ACCOUNT_RELATIONS_RIS_CONTRAGENT_ID", "RIS_CONTRAGENT", "ID"),
            //        new Column("EXTERNAL_SYSTEM_NAME", DbType.String),
            //        new Column("GUID", DbType.String),
            //        new Column("OPERATION", DbType.Int16),
            //        new Column("EXTERNAL_ID", DbType.Int64)
            //    });
            //}
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //if (this.Database.TableExists("RIS_ACCOUNT_RELATIONS"))
            //{
            //    this.Database.RemoveTable("RIS_ACCOUNT_RELATIONS");
            //}
        }
    }
}