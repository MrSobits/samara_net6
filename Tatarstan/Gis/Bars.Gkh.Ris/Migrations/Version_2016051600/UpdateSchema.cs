namespace Bars.Gkh.Ris.Migrations.Version_2016051600
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция модуля
    /// </summary>
    [Migration("2016051600")]
    [MigrationDependsOn(typeof(Version_2015122600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //this.Database.RemoveColumn("RIS_SUBSIDARY", "SOURCEDATE");
            //this.Database.AddColumn("RIS_SUBSIDARY", new Column("SOURCEDATE", DbType.Date));
            //this.Database.AddRefColumn("RIS_SUBSIDARY", new RefColumn("PARENT_ID", "RIS_PARENT_CONTRAGENT_ID", "RIS_CONTRAGENT", "ID"));
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveColumn("RIS_SUBSIDARY", "PARENT_ID");
            //this.Database.RemoveColumn("RIS_SUBSIDARY", "SOURCEDATE");
            //this.Database.AddColumn("RIS_SUBSIDARY", new Column("SOURCEDATE", DbType.String, 50));
        }
    }
}
