namespace Bars.Gkh.Migrations._2019.Version_2019060300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2019060300")]
    [MigrationDependsOn(typeof(Version_2019051400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddTable("REPORT_PERSACC_AREAS",
                new Column("ID", DbType.Int32, ColumnProperty.NotNull),
                new Column("ROOM_ID", DbType.Int32),
                new Column("CCLASS_NAME", DbType.String),
                new Column("DATE_ACTUAL", DbType.Date),
                new Column("CPROP_NAME", DbType.String),
                new Column("PARAM_NAME", DbType.String),
                new Column("AREA_VALUE", DbType.Decimal),
                new Column("CDATE_APPLIED", DbType.Date));
            Database.AddIndex("ROOMIDX", false, "REPORT_PERSACC_AREAS", "ROOM_ID");

            Database.AddTable("REPORT_PERSACC_AREASHARE",
                new Column("ID", DbType.Int32, ColumnProperty.NotNull),
                new Column("ACC_ID", DbType.Int32),
                new Column("CCLASS_NAME", DbType.String),
                new Column("DATE_ACTUAL", DbType.Date),
                new Column("CPROP_NAME", DbType.String),
                new Column("PARAM_NAME", DbType.String),
                new Column("AREA_VALUE", DbType.Decimal),
                new Column("CDATE_APPLIED", DbType.Date));
            Database.AddIndex("ACCIDX", false, "REPORT_PERSACC_AREASHARE", "ACC_ID");

            Database.AddTable("REPORT_RO_SPECIAL_ACCOUNT",
                new Column("RO_ID", DbType.Int32),
                new Column("FORM_SP", DbType.String),
                new Column("IN_DATE", DbType.Date),
                new Column("PROT_DATE", DbType.Date),
                new Column("SPOSOB", DbType.Int32),
                new Column("PROT_STATE", DbType.String));
            Database.AddIndex("ROIDX", false, "REPORT_RO_SPECIAL_ACCOUNT", "RO_ID");

            Database.AddTable("REPORT_TEMP_TABLE",
                new Column("PER_ID", DbType.Int32),
                new Column("ROOM_ID", DbType.Int32),
                new Column("ACC_ID", DbType.Int32),
                new Column("SHARE", DbType.Decimal),
                new Column("AREA", DbType.Decimal));
            Database.AddIndex("REPROOM_IDX", false, "REPORT_TEMP_TABLE", "ROOM_ID");
            Database.AddIndex("REPACC_IDX", false, "REPORT_TEMP_TABLE", "ACC_ID");
        }

        public override void Down()
        {
            Database.RemoveTable("REPORT_PERSACC_AREAS");
            Database.RemoveTable("REPORT_PERSACC_AREASHARE");
            Database.RemoveTable("REPORT_RO_SPECIAL_ACCOUNT");
            Database.RemoveTable("REPORT_TEMP_TABLE");
        }
    }
}