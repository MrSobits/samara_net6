namespace Bars.GkhDi.Regions.Tatarstan.Migrations.Version_2014031200
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Regions.Tatarstan.Migrations.Version_2014022000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // через Dialect, т.к. ORACLE не может провести modify на изменение размера колонки, если в нем содержатся данные 

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ChangeColumn("DI_DISINFO_REALOBJ", new Column("EXECUTION_WORK", DbType.String, 3000));
                Database.ChangeColumn("DI_DISINFO_REALOBJ", new Column("EXECUTION_OBLIGATION", DbType.String, 3000));
                Database.ChangeColumn("DI_DISINFO_REALOBJ", new Column("DESCR_CATREP_SERV", DbType.String, 3000));
                Database.ChangeColumn("DI_DISINFO_REALOBJ", new Column("DESCR_CATREP_TARIF", DbType.String, 3000));
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ add (TEMPCOLUMN VARCHAR2(3000))");
                Database.ExecuteNonQuery("update DI_DISINFO_REALOBJ set TEMPCOLUMN = EXECUTION_WORK");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ drop column EXECUTION_WORK");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ rename column TEMPCOLUMN to EXECUTION_WORK");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ add (TEMPCOLUMN VARCHAR2(3000))");
                Database.ExecuteNonQuery("update DI_DISINFO_REALOBJ set TEMPCOLUMN = EXECUTION_OBLIGATION");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ drop column EXECUTION_OBLIGATION");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ rename column TEMPCOLUMN to EXECUTION_OBLIGATION");

                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ add (TEMPCOLUMN VARCHAR2(3000))");
                Database.ExecuteNonQuery("update DI_DISINFO_REALOBJ set TEMPCOLUMN = DESCR_CATREP_SERV");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ drop column DESCR_CATREP_SERV");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ rename column TEMPCOLUMN to DESCR_CATREP_SERV");

                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ add (TEMPCOLUMN VARCHAR2(3000))");
                Database.ExecuteNonQuery("update DI_DISINFO_REALOBJ set TEMPCOLUMN = DESCR_CATREP_TARIF");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ drop column DESCR_CATREP_TARIF");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ rename column TEMPCOLUMN to DESCR_CATREP_TARIF");
            }

            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("FILE_EXECUTION_WORK", DbType.Int64, 22));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("FILE_ECXECUTION_OBLIG", DbType.Int64, 22));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("FILE_SERV_CAT_REPORT", DbType.Int64, 22));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("FILE_TARIF_CAT_REPORT", DbType.Int64, 22));

            Database.AddIndex("IND_FILE_EXECUTION_WORK", false, "DI_DISINFO_REALOBJ", "FILE_EXECUTION_WORK");
            Database.AddIndex("IND_FILE_ECXECUTION_OBLIG", false, "DI_DISINFO_REALOBJ", "FILE_ECXECUTION_OBLIG");
            Database.AddIndex("IND_FILE_SERV_CAT_REPORT", false, "DI_DISINFO_REALOBJ", "FILE_SERV_CAT_REPORT");
            Database.AddIndex("IND_FILE_TARIF_CAT_REPORT", false, "DI_DISINFO_REALOBJ", "FILE_TARIF_CAT_REPORT");
            Database.AddForeignKey("FK_FILE_EXECUTION_WORK", "DI_DISINFO_REALOBJ", "FILE_EXECUTION_WORK", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_FILE_ECXECUTION_OBLIG", "DI_DISINFO_REALOBJ", "FILE_ECXECUTION_OBLIG", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_FILE_SERV_CAT_REPORT", "DI_DISINFO_REALOBJ", "FILE_SERV_CAT_REPORT", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_FILE_TARIF_CAT_REPORT", "DI_DISINFO_REALOBJ", "FILE_TARIF_CAT_REPORT", "B4_FILE_INFO", "ID");
        }

        public override void Down()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ChangeColumn("DI_DISINFO_REALOBJ", new Column("EXECUTION_WORK", DbType.String, 500));
                Database.ChangeColumn("DI_DISINFO_REALOBJ", new Column("EXECUTION_OBLIGATION", DbType.String, 500));
                Database.ChangeColumn("DI_DISINFO_REALOBJ", new Column("DESCR_CATREP_SERV", DbType.String, 500));
                Database.ChangeColumn("DI_DISINFO_REALOBJ", new Column("DESCR_CATREP_TARIF", DbType.String, 500));
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ add (TEMPCOLUMN VARCHAR2(500))");
                Database.ExecuteNonQuery("update DI_DISINFO_REALOBJ set TEMPCOLUMN = EXECUTION_WORK");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ drop column EXECUTION_WORK");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ rename column TEMPCOLUMN to EXECUTION_WORK");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ add (TEMPCOLUMN VARCHAR2(500))");
                Database.ExecuteNonQuery("update DI_DISINFO_REALOBJ set TEMPCOLUMN = EXECUTION_OBLIGATION");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ drop column EXECUTION_OBLIGATION");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ rename column TEMPCOLUMN to EXECUTION_OBLIGATION");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ add (TEMPCOLUMN VARCHAR2(500))");
                Database.ExecuteNonQuery("update DI_DISINFO_REALOBJ set TEMPCOLUMN = DESCR_CATREP_SERV");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ drop column DESCR_CATREP_SERV");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ rename column TEMPCOLUMN to DESCR_CATREP_SERV");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ add (TEMPCOLUMN VARCHAR2(500))");
                Database.ExecuteNonQuery("update DI_DISINFO_REALOBJ set TEMPCOLUMN = DESCR_CATREP_TARIF");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ drop column DESCR_CATREP_TARIF");
                Database.ExecuteNonQuery("alter table DI_DISINFO_REALOBJ rename column TEMPCOLUMN to DESCR_CATREP_TARIF");
            }

            Database.RemoveConstraint("DI_DISINFO_REALOBJ", "FK_FILE_EXECUTION_WORK");
            Database.RemoveConstraint("DI_DISINFO_REALOBJ", "FK_FILE_ECXECUTION_OBLIG");
            Database.RemoveConstraint("DI_DISINFO_REALOBJ", "FK_FILE_SERV_CAT_REPORT");
            Database.RemoveConstraint("DI_DISINFO_REALOBJ", "FK_FILE_TARIF_CAT_REPORT");

            Database.RemoveColumn("DI_DISINFO_REALOBJ", "FILE_EXECUTION_WORK");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "FILE_ECXECUTION_OBLIG");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "FILE_SERV_CAT_REPORT");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "FILE_TARIF_CAT_REPORT");
        }
    }
}