namespace Bars.GkhGji.Migrations._2022.Version_2022053100
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022053100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022053000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_SSTU_TRANSFER", new Column("POSITION", DbType.String, 500));
            Database.AddColumn("GJI_DICT_SSTU_TRANSFER", new Column("ADDRESS", DbType.String, 500));
            Database.AddColumn("GJI_DICT_SSTU_TRANSFER", new Column("FIO", DbType.String, 500));

            ClearTable("GJI_ACTCHECK_CONTROLLIST_ANSWER");

            Database.AddColumn("GJI_ACTCHECK_CONTROLLIST_ANSWER", new RefColumn("QUESTION_ID", ColumnProperty.NotNull, "GJI_ACTCHECK_CONTROLLIST_ANSWER_QUESTION", "GJI_DICT_CONTROL_LIST_QUESTION", "ID"));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_ACTCHECK_CONTROLLIST_ANSWER", "QUESTION_ID");
            Database.RemoveColumn("GJI_DICT_SSTU_TRANSFER", "FIO");
            Database.RemoveColumn("GJI_DICT_SSTU_TRANSFER", "ADDRESS");
            Database.RemoveColumn("GJI_DICT_SSTU_TRANSFER", "POSITION");
        }

        private void ClearTable(string tablename)
        {
            var sql = $"Delete from {tablename}";

            this.Database.ExecuteNonQuery(sql);

        }
    }
}