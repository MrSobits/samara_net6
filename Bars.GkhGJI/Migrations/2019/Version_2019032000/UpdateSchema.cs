namespace Bars.GkhGji.Migrations._2019.Version_2019032000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019032000")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2018.Version_2018122100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {


            this.Database.AddEntityTable("GJI_DICT_OSP",
               new Column("BANK_ACCOUNT", DbType.String, 30),
               new Column("KBK", DbType.String, 30),
               new Column("SHORT_NAME", DbType.String, 300),
               new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
               new Column("STREET", DbType.String, 100),
               new Column("TOWN", DbType.String, 100),
               new Column("CREDITORG_ID", DbType.Int64, 22),
               new Column("MUNICIPALITY_ID", DbType.Int64, 22));

            Database.AddForeignKey("FK_GJI_OSP_MCP", "GJI_DICT_OSP", "MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");
            Database.AddForeignKey("FK_GJI_OSP_CREDITORG", "GJI_DICT_OSP", "CREDITORG_ID", "OVRHL_CREDIT_ORG", "ID");

            Database.AddColumn("GJI_RESOLUTION", new Column("POST_UIN", DbType.String, 50));
            Database.AddColumn("GJI_RESOLUTION", new Column("OSP_ID", DbType.Int64, 22));
            Database.AddForeignKey("RESOL_OSP", "GJI_RESOLUTION", "OSP_ID", "CLW_JUR_INSTITUTION", "ID");
            Database.AddColumn("GJI_RESOLUTION", new Column("COMMENT", DbType.String, 500));
            Database.AddColumn("GJI_RESOLUTION", new Column("EXECUTESSP_NUMBER", DbType.String, 250));
            Database.AddColumn("GJI_RESOLUTION", new Column("EXECUTESTART_DATE", DbType.DateTime));
            Database.AddColumn("GJI_RESOLUTION", new Column("END_EXECUTE_DATE", DbType.DateTime));
            Database.AddColumn("GJI_RESOLUTION", new Column("EXECUTEDOCARRIVE_DATE", DbType.DateTime));
            Database.AddColumn("GJI_RESOLUTION", new Column("OSP_DECISION", DbType.Int32, 4));
            Database.AddColumn("GJI_RESOLUTION", new Column("DECISION_DATE", DbType.DateTime, ColumnProperty.Null));
            Database.AddColumn("GJI_RESOLUTION", new Column("DECISION_ENTRY_DATE", DbType.DateTime, ColumnProperty.Null));
            Database.AddColumn("GJI_RESOLUTION", new Column("DECISION_NUMBER", DbType.String, 50));
            Database.AddColumn("GJI_RESOLUTION", new Column("VIOLATION", DbType.String, 800));
            Database.AddColumn("GJI_RESOLUTION", new RefColumn("JUDICAL_OFFICE_ID", ColumnProperty.None, "FK_GJI_RESOLUTION_JUDICAL_OFFICE", "CLW_JUR_INSTITUTION", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_EGRIP", "JUDICAL_OFFICE_ID");
            Database.RemoveColumn("GJI_CH_SMEV_EGRIP", "VIOLATION");
            Database.RemoveColumn("GJI_CH_SMEV_EGRIP", "DECISION_NUMBER");
            Database.RemoveColumn("GJI_CH_SMEV_EGRIP", "DECISION_ENTRY_DATE");
            Database.RemoveColumn("GJI_CH_SMEV_EGRIP", "DECISION_DATE");
            Database.RemoveColumn("GJI_RESOLUTION", "OSP_DECISION");
            Database.RemoveColumn("GJI_RESOLUTION", "EXECUTEDOCARRIVE_DATE");
            Database.RemoveColumn("GJI_RESOLUTION", "END_EXECUTE_DATE");
            Database.RemoveColumn("GJI_RESOLUTION", "EEXECUTESTART_DATE");
            Database.RemoveColumn("GJI_RESOLUTION", "EXECUTESSP_NUMBER");
            Database.RemoveColumn("GJI_RESOLUTION", "COMMENT");
            Database.RemoveColumn("GJI_RESOLUTION", "OSP_ID");
            Database.RemoveColumn("GJI_RESOLUTION", "POST_UIN");
            Database.RemoveTable("GJI_DICT_OSP");

        }

    }
}