namespace Bars.Gkh.Migrations._2017.Version_2017021800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Миграция Gkh.2017021800
    /// </summary>
    [Migration("2017021800")]
    [MigrationDependsOn(typeof(Version_2017020100.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017012500.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017012501.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_PERSON_REQUEST_EXAM", new Column("SUPPLY_METHOD", DbType.Int32, ColumnProperty.NotNull, 0));
            this.Database.AddColumn("GKH_PERSON_REQUEST_EXAM", new Column("REQUEST_TIME", DbType.String, 255));
            this.Database.AddColumn("GKH_PERSON_REQUEST_EXAM", new Column("NOTIF_NUM", DbType.String, 255));
            this.Database.AddColumn("GKH_PERSON_REQUEST_EXAM", new Column("NOTIF_DATE", DbType.DateTime));
            this.Database.AddColumn("GKH_PERSON_REQUEST_EXAM", new Column("IS_DENIED", DbType.Boolean));
            this.Database.AddColumn("GKH_PERSON_REQUEST_EXAM", new Column("RESULT_NOTIF_NUM", DbType.String, 255));
            this.Database.AddColumn("GKH_PERSON_REQUEST_EXAM", new Column("RESULT_NOTIF_DATE", DbType.DateTime));
            this.Database.AddColumn("GKH_PERSON_REQUEST_EXAM", new Column("MAILING_DATE", DbType.DateTime));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_PERSON_REQUEST_EXAM", "SUPPLY_METHOD");
            this.Database.RemoveColumn("GKH_PERSON_REQUEST_EXAM", "REQUEST_TIME");
            this.Database.RemoveColumn("GKH_PERSON_REQUEST_EXAM", "NOTIF_NUM");
            this.Database.RemoveColumn("GKH_PERSON_REQUEST_EXAM", "NOTIF_DATE");
            this.Database.RemoveColumn("GKH_PERSON_REQUEST_EXAM", "IS_DENIED");
            this.Database.RemoveColumn("GKH_PERSON_REQUEST_EXAM", "RESULT_NOTIF_NUM");
            this.Database.RemoveColumn("GKH_PERSON_REQUEST_EXAM", "RESULT_NOTIF_DATE");
            this.Database.RemoveColumn("GKH_PERSON_REQUEST_EXAM", "MAILING_DATE");
        }
    }
}