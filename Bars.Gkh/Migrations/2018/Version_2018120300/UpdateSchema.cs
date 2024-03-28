using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.Migrations._2018.Version_2018120300
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018120300")]
    [MigrationDependsOn(typeof(Version_2018100400.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
       
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_DICT_QTEST_QUESTIONS",
                new Column("CODE", DbType.String, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, ColumnProperty.Null),
                new Column("QUESTION_TEXT", DbType.String, ColumnProperty.NotNull),
                new Column("IS_ACTUAL", DbType.Int32, 4, ColumnProperty.NotNull, 10));

            this.Database.AddEntityTable("GKH_DICT_QTEST_QUANSWERS",
              new Column("CODE", DbType.String, ColumnProperty.NotNull),
              new Column("ANSWER", DbType.String, ColumnProperty.NotNull),
              new Column("IS_CORRECT", DbType.Int32, 4, ColumnProperty.NotNull, 20),
               new RefColumn("QUESTION_ID", "FK_QTEST_QANSWERS_ANSWERS", "GKH_DICT_QTEST_QUESTIONS", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_DICT_QTEST_QUANSWERS");
            this.Database.RemoveTable("GKH_DICT_QTEST_QUESTIONS");
        }
    }
}