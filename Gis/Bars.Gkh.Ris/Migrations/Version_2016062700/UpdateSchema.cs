namespace Bars.Gkh.Ris.Migrations.Version_2016062700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016062700")]
    [MigrationDependsOn(typeof(Version_2016062500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
           this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("RECIPIENT_ENTPR_FIO", DbType.String));
            this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("NON_LIVING_APARTMENT", DbType.String));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "RECIPIENT_ENTPR_FIO");
            this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "NON_LIVING_APARTMENT");
        }
    }
}
