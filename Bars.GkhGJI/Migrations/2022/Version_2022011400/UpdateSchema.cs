namespace Bars.GkhGji.Migrations._2022.Version_2022011400
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022011400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022011200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {           

            Database.AddEntityTable(
              "GJI_DICT_CONTROL_LIST",      
              new Column("DATE_START", DbType.DateTime),
              new Column("DATE_END", DbType.DateTime, ColumnProperty.None),
              new Column("NAME", DbType.String, 1500, ColumnProperty.NotNull),
              new Column("EXTERNAL_ID", DbType.String, 36),
              new Column("CODE", DbType.String, 300),
              new Column("KIND_KND", DbType.Int32, 4, ColumnProperty.NotNull, 0));

            Database.AddEntityTable(
           "GJI_DICT_CONTROL_LIST_QUESTION",         
           new Column("NAME", DbType.String, 3500, ColumnProperty.NotNull),
           new Column("NPD_NAME", DbType.String, 3500, ColumnProperty.NotNull),
           new Column("DESCRIPTION", DbType.String, 1500),
           new Column("EXTERNAL_ID", DbType.String, 36),
           new Column("CODE", DbType.String, 300),
           new RefColumn("LIST_ID", "GJI_DICT_CONTROL_LIST_QUESTION_LIST_ID", "GJI_DICT_CONTROL_LIST", "ID"));

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_CONTROL_LIST_QUESTION");
            Database.RemoveTable("GJI_DICT_CONTROL_LIST");
        }
    }
}