namespace Bars.Gkh.Migrations.Version_2014121900
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014121700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_PERSON_REQUEST_EXAM",
                new Column("REQUEST_NUM", DbType.String, 100),
                new Column("REQUEST_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("EXAM_DATE", DbType.DateTime),
                new Column("EXAM_TIME", DbType.String, 100),
                new Column("COR_ANSWER_PERCENT", DbType.Decimal),
                new Column("PROTOCOL_NUM", DbType.String, 100),
                new Column("PROTOCOL_DATE", DbType.DateTime),
                new RefColumn("PERSON_ID", ColumnProperty.NotNull, "GKH_PERS_REQ_EX_PER", "GKH_PERSON", "ID"),
                new RefColumn("REQUEST_FILE_ID", "GKH_PERS_REQ_EX_RF", "B4_FILE_INFO", "ID"),
                new RefColumn("PROTOCOL_FILE_ID", "GKH_PERS_REQ_EX_PF", "B4_FILE_INFO", "ID"),
                new RefColumn("STATE_ID", "GKH_PERS_REQ_EX_ST", "B4_STATE", "ID"));

            Database.AddRefColumn("GKH_PERSON_CERTIFICATE", new RefColumn("REQUEST_EXAM_ID", "GKH_PERS_CERT_REQ_EX", "GKH_PERSON_REQUEST_EXAM", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "REQUEST_EXAM_ID");

            Database.RemoveTable("GKH_PERSON_REQUEST_EXAM");
        }
    }
}