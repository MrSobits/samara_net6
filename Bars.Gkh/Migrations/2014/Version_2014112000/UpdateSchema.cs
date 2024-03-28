namespace Bars.Gkh.Migrations.Version_2014112000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2014.Version_2014102800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {

            Database.AddEntityTable("GKH_PERSON",
                    new Column("SURNAME", DbType.String,  100, ColumnProperty.NotNull),
                    new Column("NAME", DbType.String, 100, ColumnProperty.NotNull),
                    new Column("PATRONYMIC", DbType.String, 100),
                    new Column("FULL_NAME", DbType.String, 500, ColumnProperty.NotNull),
                    new Column("PHONE", DbType.String, 200),
                    new Column("EMAIL", DbType.String, 200),
                    new Column("ADDRESS_REG", DbType.String, 2000),
                    new Column("ADDRESS_LIVE", DbType.String, 2000),
                    new Column("INN", DbType.String, 20),
                    new Column("IDENT_TYPE", DbType.Int16, 4, ColumnProperty.NotNull, 10),
                    new Column("IDENT_SERIAL", DbType.String, 10),
                    new Column("IDENT_NUMBER", DbType.String, 10),
                    new Column("IDENT_ISSUEDBY", DbType.String, 2000),
                    new Column("IDENT_ISSUEDDATE", DbType.DateTime));

            Database.AddEntityTable("GKH_PERSON_CERTIFICATE",
                    new RefColumn("PERSON_ID", ColumnProperty.NotNull, "GKH_PERSON_CERTIFICATE_P", "GKH_PERSON", "ID"),
                    new RefColumn("FILE_ID", ColumnProperty.Null, "GKH_PERSON_CERTIFICATE_F", "B4_FILE_INFO", "ID"),
                    new Column("ISSUED_DATE", DbType.DateTime),
                    new Column("CERT_NUMBER", DbType.String, 100),
                    new Column("END_DATE", DbType.DateTime),
                    new Column("TYPE_CANCELATION", DbType.Int16, 4, ColumnProperty.NotNull, 0),
                    new Column("CANCEL_DATE", DbType.DateTime));

            Database.AddEntityTable("GKH_PERSON_DISQUAL",
                    new RefColumn("PERSON_ID", ColumnProperty.NotNull, "GKH_PERSON_DISQUAL_P", "GKH_PERSON", "ID"),
                    new Column("DISQ_TYPE", DbType.Int16, 4, ColumnProperty.NotNull, 10),
                    new Column("DISQ_DATE", DbType.DateTime),
                    new Column("DISQ_END_DATE", DbType.DateTime),
                    new Column("PETITION_DATE", DbType.DateTime),
                    new Column("PETITION_NUMBER", DbType.String, 100),
                    new RefColumn("PETITION_FILE_ID", ColumnProperty.Null, "GKH_PERSON_DISQUAL_F", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable("GKH_PERSON_PLACEWORK",
                    new RefColumn("PERSON_ID", ColumnProperty.NotNull, "GKH_PERSON_PLACEWORK_PE", "GKH_PERSON", "ID"),
                    new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "GKH_PERSON_PLACEWORK_C", "GKH_CONTRAGENT", "ID"),
                    new RefColumn("POSITION_ID", ColumnProperty.Null, "GKH_PERSON_PLACEWORK_PO", "GKH_DICT_POSITION", "ID"),
                    new Column("START_DATE", DbType.DateTime),
                    new Column("END_DATE", DbType.DateTime));
            
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_PERSON_PLACEWORK");
            Database.RemoveTable("GKH_PERSON_DISQUAL");
            Database.RemoveTable("GKH_PERSON_CERTIFICATE");
            Database.RemoveTable("GKH_PERSON");
        }
    }
}