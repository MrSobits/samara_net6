namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2017111600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017111600")]
    [MigrationDependsOn(typeof(Version_2017111500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Категория подателей заявления
            Database.AddEntityTable(
                "GJI_CH_RESOLUTION_FIZ",
                 new RefColumn("RESOLUTION_ID", ColumnProperty.NotNull, "GJI_CH_RESOLUTION_FIZ_RESOLUTION_ID", "GJI_RESOLUTION", "ID"),
                 new RefColumn("FLDOCTYPE_ID", ColumnProperty.NotNull, "GJI_CH_RESOLUTION_FIZ_FLDOCTYPE_ID", "GJI_CH_DICT_FLDOC_TYPE", "ID"),
                 new Column("DOC_NUM", DbType.String, 300, ColumnProperty.NotNull),
                 new Column("DOC_SERIAL", DbType.String, 300, ColumnProperty.NotNull),
                 new Column("PAYER_CODE", DbType.String, 300, ColumnProperty.None),
                 new Column("CITIZENSHIP", DbType.Boolean));

            Database.AddEntityTable(
       "GJI_CH_SMEV_EGRN",
       new Column("REQUEST_DATE", DbType.DateTime, ColumnProperty.NotNull),
       new Column("DECLARANT_ID", DbType.String, ColumnProperty.NotNull, 20),
       new Column("DOCUMENT_REF", DbType.String, 500),
       new Column("PERSON_NAME", DbType.String, 500),
       new Column("PERSON_SURNAME", DbType.String, 500),
       new Column("PERSON_PATRONYMIC", DbType.String, 500),
       new Column("DOCUMENT_SERIAL", DbType.String, 500),
       new Column("DOCUMENT_NUMBER", DbType.String, 500),
       new Column("REQUEST_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
       new Column("ANSWER", DbType.String, 500),
       new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
       new Column("MESSAGE_ID", DbType.String, 500),
       new RefColumn("REG_CODE_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_EGRN_REGCODE_ID", "GJI_CH_DICT_REGCODE", "ID"),
       new RefColumn("EGRN_APPLICANT_TYPE_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_EGRN_APPLTYPE_ID", "GJI_CH_DICT_EGRN_APPLICANT_TYPE", "ID"),
       new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_EGRN_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"));

            Database.AddEntityTable(
       "GJI_CH_SMEV_EGRN_FILE",
       new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
       new RefColumn("SMEV_EGRN_ID", ColumnProperty.None, "GJI_CH_SMEVEGRN_SMEV_EGRN_ID", "GJI_CH_SMEV_EGRN", "ID"),
       new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEVEGRN_FILE_INFO_ID", "B4_FILE_INFO", "ID"));



        }
        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_EGRN_FILE");
            Database.RemoveTable("GJI_CH_SMEV_EGRN");
            Database.RemoveTable("GJI_CH_RESOLUTION_FIZ");
        }
    }
}