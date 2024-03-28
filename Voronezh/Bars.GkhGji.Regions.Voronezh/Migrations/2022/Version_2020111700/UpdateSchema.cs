namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020111700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020111700")]
    [MigrationDependsOn(typeof(Version_2020111100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //-----Тип объекта запроса ЕГРН
            Database.AddEntityTable(
                "GJI_CH_DICT_EGRN_OBJECT_TYPE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 300, ColumnProperty.None),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull));
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            Database.ExecuteNonQuery($@"insert into public.GJI_CH_DICT_EGRN_OBJECT_TYPE (CODE, NAME, object_version, object_create_date, object_edit_date) VALUES
('002001001000', 'Земельный участок', 0, '{date}'::date, '{date}'::date),
('002001002000', 'Здание', 0, '{date}'::date, '{date}'::date),
('002001003000', 'Помещение', 0, '{date}'::date, '{date}'::date),
('002001004000', 'Сооружение', 0, '{date}'::date, '{date}'::date),
('002001005000', 'Объект незавершенного строительства', 0, '{date}'::date, '{date}'::date),
('002001006000', 'Предприятие как имущественный комплекс', 0, '{date}'::date, '{date}'::date),
('002001008000', 'Единый недвижимый комплекс', 0, '{date}'::date, '{date}'::date),
('002001009000', 'Машино-место', 0, '{date}'::date, '{date}'::date),
('002001010000', 'Иной объект недвижимости', 0, '{date}'::date, '{date}'::date)");

            //-----Справочник кодов регионов
            Database.AddEntityTable(
                "GJI_CH_DICT_REGCODE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_CH_REGCODE_NAME", false, "GJI_CH_DICT_REGCODE", "NAME");
            Database.AddIndex("IND_GJI_CH_REGCODE__CODE", false, "GJI_CH_DICT_REGCODE", "CODE");
            Database.ExecuteNonQuery($@"insert into public.GJI_CH_DICT_REGCODE (CODE, NAME, object_version, object_create_date, object_edit_date) VALUES
('36', 'Воронежская область', 0, '{date}'::date, '{date}'::date)");

            //-----Категория подателей заявления
            Database.AddEntityTable(
                "GJI_CH_DICT_EGRN_APPLICANT_TYPE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 300, ColumnProperty.None),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull));
            Database.ExecuteNonQuery($@"insert into public.GJI_CH_DICT_EGRN_APPLICANT_TYPE (CODE, NAME, object_version, object_create_date, object_edit_date) VALUES
('357013000000', 'Органы государственной власти субъектов Российской Федерации', 0, '{date}'::date, '{date}'::date),
('357014000000', 'Органы местного самоуправления', 0, '{date}'::date, '{date}'::date),
('357020000000', 'Руководители (должностные лица) федеральных государственных органов, перечень которых определяется Президентом Российской Федерации, и высшие должностные лица субъектов Российской Федерации (руководителям высших исполнительных органов государственной власти субъектов Российской Федерации)', 0, '{date}'::date, '{date}'::date),
('357023000000', 'Иные определенные федеральным законом органы и организации, имеющие право на бесплатное получение информации', 0, '{date}'::date, '{date}'::date),
('357099000000', 'Иное лицо', 0, '{date}'::date, '{date}'::date)");

            ////-----Тип документа ЕГРН
            //Database.AddEntityTable(
            //   "GJI_CH_DICT_EGRN_DOC_TYPE",
            //   new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
            //   new Column("DESCRIPTION", DbType.String, 300, ColumnProperty.None),
            //   new Column("CODE", DbType.String, 300, ColumnProperty.NotNull));

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
               new Column("REQUEST_DATA_TYPE", System.Data.DbType.Int32, ColumnProperty.None),
               new Column("QUALITY_PHONE", DbType.String, ColumnProperty.None),
               new RefColumn("REG_CODE_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_EGRN_REGCODE_ID", "GJI_CH_DICT_REGCODE", "ID"),
               new RefColumn("EGRN_APPLICANT_TYPE_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_EGRN_APPLTYPE_ID", "GJI_CH_DICT_EGRN_APPLICANT_TYPE", "ID"),
               new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_EGRN_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
               new RefColumn("RO_ID", ColumnProperty.None, "GJI_CH_SMEV_EGRN_RO_ID", "GKH_REALITY_OBJECT", "ID"),
               new RefColumn("ROOM_ID", ColumnProperty.None, "GJI_CH_SMEV_EGRN_ROOM_ID", "GKH_ROOM", "ID"),
               new RefColumn("EGRN_OBJECT_TYPE_ID", ColumnProperty.None, "GJI_CH_SMEV_EGRN_OBJTYPE_ID", "GJI_CH_DICT_EGRN_OBJECT_TYPE", "ID"));

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
            //Database.RemoveTable("GJI_CH_DICT_EGRN_DOC_TYPE");
            Database.RemoveTable("GJI_CH_DICT_EGRN_APPLICANT_TYPE");
            Database.RemoveTable("GJI_CH_DICT_EGRN_OBJECT_TYPE");
            Database.RemoveTable("GJI_CH_DICT_REGCODE");
        }
    }
}


