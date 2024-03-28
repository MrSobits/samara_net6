using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.Migrations._2018.Version_2018120700
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018120700")]
    [MigrationDependsOn(typeof(Version_2018120600.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
       
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_MANORG_REQ_RPGU",
                new Column("ANSWER_TEXT", DbType.String, 5000),
                new Column("RPGU_TEXT", DbType.String, 5000),
                new Column("RPGU_REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("RPGU_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 30),
                new Column("RPGU_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 30),
                new RefColumn("LIC_REQUEST_ID", "FK_RPGU_LIC_REQUEST", "GKH_MANORG_LIC_REQUEST", "ID"),
                new RefColumn("ANSWER_FILE_ID", "FK_RPGU_ANSWERFILE_FILEINFO", "B4_FILE_INFO", "ID"),
                new RefColumn("LIC_DOC_FILE_ID", "FK_RPGU_REQFILE_FILEINFO", "B4_FILE_INFO", "ID"));

            this.Database.AddEntityTable("GKH_DICT_QTEST_SETTINGS",
               new Column("ACC_RATE", DbType.Decimal),
               new Column("Q_COUNT", DbType.Int16, ColumnProperty.NotNull),
               new Column("POINTS_PER_QUESTION", DbType.Int16, ColumnProperty.NotNull),
               new Column("DATE_FROM", DbType.DateTime, ColumnProperty.NotNull),
               new Column("DATE_TO", DbType.DateTime),
               new Column("TIME_LIMIT", DbType.Int16, ColumnProperty.NotNull));



        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_DICT_QTEST_SETTINGS");
            this.Database.RemoveTable("GKH_MANORG_REQ_RPGU");
            
        }
    }
}