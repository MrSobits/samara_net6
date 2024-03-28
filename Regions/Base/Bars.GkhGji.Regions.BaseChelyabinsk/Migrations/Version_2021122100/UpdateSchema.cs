namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021122100
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021122100")]
    [MigrationDependsOn(typeof(Version_2021120900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
           

            Database.AddColumn("GJI_PROTOCOL197_ANNEX", new Column("DOCUMENT_SEND", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_PROTOCOL197_ANNEX", new Column("DOCUMENT_DELIV", DbType.DateTime, ColumnProperty.None));

            Database.AddColumn("GJI_DISPOSAL_ANNEX", new Column("DOCUMENT_SEND", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_DISPOSAL_ANNEX", new Column("DOCUMENT_DELIV", DbType.DateTime, ColumnProperty.None));

            Database.AddColumn("GJI_ACTCHECK_ANNEX", new Column("DOCUMENT_SEND", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_ACTCHECK_ANNEX", new Column("DOCUMENT_DELIV", DbType.DateTime, ColumnProperty.None));

            Database.AddColumn("GJI_ACTCHECK_DEFINITION", new Column("DOCUMENT_SEND", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_ACTCHECK_DEFINITION", new Column("DOCUMENT_DELIV", DbType.DateTime, ColumnProperty.None));

            Database.AddColumn("GJI_PRESCRIPTION_ANNEX", new Column("DOCUMENT_SEND", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_PRESCRIPTION_ANNEX", new Column("DOCUMENT_DELIV", DbType.DateTime, ColumnProperty.None));

            Database.AddColumn("GJI_PROTOCOL_ANNEX", new Column("DOCUMENT_SEND", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_PROTOCOL_ANNEX", new Column("DOCUMENT_DELIV", DbType.DateTime, ColumnProperty.None));

            Database.AddColumn("GJI_RESOLUTION_ANNEX", new Column("DOCUMENT_SEND", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_RESOLUTION_ANNEX", new Column("DOCUMENT_DELIV", DbType.DateTime, ColumnProperty.None));
        }
                       
        public override void Down()
        {
            Database.RemoveColumn("GJI_PRESCRIPTION_ANNEX", "DOCUMENT_DELIV");
            Database.RemoveColumn("GJI_PRESCRIPTION_ANNEX", "DOCUMENT_SEND");

            Database.RemoveColumn("GJI_PRESCRIPTION_ANNEX", "DOCUMENT_DELIV");
            Database.RemoveColumn("GJI_PRESCRIPTION_ANNEX", "DOCUMENT_SEND");

            Database.RemoveColumn("GJI_PROTOCOL_ANNEX", "DOCUMENT_DELIV");
            Database.RemoveColumn("GJI_PROTOCOL_ANNEX", "DOCUMENT_SEND");

            Database.RemoveColumn("GJI_RESOLUTION_ANNEX", "DOCUMENT_DELIV");
            Database.RemoveColumn("GJI_RESOLUTION_ANNEX", "DOCUMENT_SEND");

            Database.RemoveColumn("GJI_DISPOSAL_ANNEX", "DOCUMENT_DELIV");
            Database.RemoveColumn("GJI_DISPOSAL_ANNEX", "DOCUMENT_SEND");

            Database.RemoveColumn("GJI_PROTOCOL197_ANNEX", "DOCUMENT_DELIV");
            Database.RemoveColumn("GJI_PROTOCOL197_ANNEX", "DOCUMENT_SEND");

            Database.RemoveColumn("GJI_ACTCHECK_ANNEX", "DOCUMENT_DELIV");
            Database.RemoveColumn("GJI_ACTCHECK_ANNEX", "DOCUMENT_SEND");
        }

    }
}