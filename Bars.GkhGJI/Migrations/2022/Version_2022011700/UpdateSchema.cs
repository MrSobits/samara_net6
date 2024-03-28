namespace Bars.GkhGji.Migrations._2022.Version_2022011700
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022011700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022011400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {           

            Database.AddEntityTable(
              "GJI_DICT_DECISION_REASON",           
              new Column("NAME", DbType.String, 1500, ColumnProperty.NotNull),
              new Column("EXTERNAL_ID", DbType.String, 36),
              new Column("CODE", DbType.String, 300));

            Database.AddEntityTable(
             "GJI_DECISION_DEC_REASON",
             new Column("DESCRIPTION", DbType.String, 2500, ColumnProperty.None),
             new RefColumn("DECISION_ID", "GJI_DEC_DEC_REASON_DEC_ID", "GJI_DICISION", "ID"),
             new RefColumn("DEC_REASON_ID", "GJI_DEC_DEC_REASON_DICT_ID", "GJI_DICT_DECISION_REASON", "ID"));

            Database.AddEntityTable(
           "GJI_DECISION_CON_LIST",
           new Column("DESCRIPTION", DbType.String, 2500, ColumnProperty.None),
           new RefColumn("DECISION_ID", "GJI_DECISION_CON_LIST_DEC_ID", "GJI_DICISION", "ID"),
           new RefColumn("CONTROL_LIST_ID", "GJI_DECISION_CON_LIST_CLIST_ID", "GJI_DICT_CONTROL_LIST", "ID"));


        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DECISION_CON_LIST");
            Database.RemoveTable("GJI_DECISION_DEC_REASON");
            Database.RemoveTable("GJI_DICT_DECISION_REASON");
        }
    }
}