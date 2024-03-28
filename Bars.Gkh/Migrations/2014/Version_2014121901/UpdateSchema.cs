namespace Bars.Gkh.Migrations.Version_2014121901
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014121900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_PERSON_CERTIFICATE", "BLANK_NUMBER", DbType.String, 100);
            Database.AddColumn("GKH_PERSON_CERTIFICATE", "HAS_DUPLICATE", DbType.Boolean, ColumnProperty.NotNull, false);
            Database.AddColumn("GKH_PERSON_CERTIFICATE", "DUPLICATE_NUMBER", DbType.String, 100);
            Database.AddColumn("GKH_PERSON_CERTIFICATE", "DUPLICATE_ISSUED_DATE", DbType.Date);
            Database.AddRefColumn("GKH_PERSON_CERTIFICATE", new RefColumn("DUPLICATE_FILE_ID", "DUPLICATE_FILE", "B4_FILE_INFO", "ID"));

            Database.AddColumn("GKH_PERSON_CERTIFICATE", "HAS_CANCELLED", DbType.Boolean, ColumnProperty.NotNull, false);
            Database.AddColumn("GKH_PERSON_CERTIFICATE", "CANCEL_NUMBER", DbType.String, 100);
            Database.AddColumn("GKH_PERSON_CERTIFICATE", "CANCEL_PROTOCOL_DATE", DbType.Date);
            Database.AddRefColumn("GKH_PERSON_CERTIFICATE", new RefColumn("CANCEL_FILE_ID", "CANCEL_FILE", "B4_FILE_INFO", "ID"));

            Database.AddColumn("GKH_PERSON_CERTIFICATE", "HAS_RENEWED", DbType.Boolean, ColumnProperty.NotNull, false);
            Database.AddColumn("GKH_PERSON_CERTIFICATE", "COURT_NAME", DbType.String, 1000);
            Database.AddColumn("GKH_PERSON_CERTIFICATE", "COURT_ACT_NUMBER", DbType.String, 100);
            Database.AddColumn("GKH_PERSON_CERTIFICATE", "COURT_ACT_DATE", DbType.Date);
            Database.AddRefColumn("GKH_PERSON_CERTIFICATE", new RefColumn("ACT_FILE_ID", "ACT_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "BLANK_NUMBER");
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "HAS_DUPLICATE");
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "DUPLICATE_NUMBER");
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "DUPLICATE_ISSUED_DATE");
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "DUPLICATE_FILE_ID");

            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "HAS_CANCELLED");
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "CANCEL_NUMBER");
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "CANCEL_PROTOCOL_DATE");
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "CANCEL_FILE_ID");

            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "HAS_RENEWED");
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "COURT_NAME");
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "COURT_ACT_NUMBER");
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "COURT_ACT_DATE");
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "ACT_FILE_ID");
        }
    }
}