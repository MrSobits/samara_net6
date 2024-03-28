namespace Bars.B4.Modules.FIAS.Migrations.Version_1
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddPersistentObjectTable("B4_FIAS",
                new Column("CODERECORD", DbType.String, 50),
                new Column("TYPERECORD", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("AOLEVEL", DbType.Int32, 4, ColumnProperty.NotNull, 7),
                new Column("ACTSTATUS", DbType.Int32, 4, ColumnProperty.NotNull, 0),
                new Column("CENTSTATUS", DbType.Int32, 4, ColumnProperty.NotNull, 0),
                new Column("OPERSTATUS", DbType.Int32, 4, ColumnProperty.NotNull, 1),
                new Column("STARTDATE", DbType.DateTime),
                new Column("ENDDATE", DbType.DateTime),
                new Column("AOGUID", DbType.String, 36),
                new Column("AOID", DbType.String, 36),
                new Column("PARENTGUID", DbType.String, 36),
                new Column("PREVID", DbType.String, 36),
                new Column("NEXTID", DbType.String, 36),
                new Column("FORMALNAME", DbType.String, 120),
                new Column("OFFNAME", DbType.String, 120),
                new Column("SHORTNAME", DbType.String, 10),
                new Column("REGIONCODE", DbType.String, 5),
                new Column("AUTOCODE", DbType.String, 5),
                new Column("AREACODE", DbType.String, 5),
                new Column("CITYCODE", DbType.String, 5),
                new Column("CTARCODE", DbType.String, 5),
                new Column("PLACECODE", DbType.String, 5),
                new Column("STREETCODE", DbType.String, 5),
                new Column("EXTRCODE", DbType.String, 5),
                new Column("SEXTCODE", DbType.String, 5),
                new Column("POSTALCODE", DbType.String, 6),
                new Column("IFNSFL", DbType.String, 4),
                new Column("TERRIFNSFL", DbType.String, 4),
                new Column("IFNSUL", DbType.String, 4),
                new Column("TERRIFNSUL", DbType.String, 4),
                new Column("OKATO", DbType.String, 11),
                new Column("OKTMO", DbType.String, 8),
                new Column("UPDATEDATE", DbType.DateTime),
                new Column("NORMDOC", DbType.String, 36),
                new Column("KLADRCODE", DbType.String, 17),
                new Column("KLADRPLAINCODE", DbType.String, 15),
                new Column("KLADRPCURRSTATUS", DbType.Int32, 4, ColumnProperty.NotNull, 0));

            Database.AddIndex("B4_FIAS_AOID", false, "B4_FIAS", "AOID");
            Database.AddIndex("B4_FIAS_PARENT", false, "B4_FIAS", "PARENTGUID");
            Database.AddIndex("B4_FIAS_FNAME", false, "B4_FIAS", "FORMALNAME");
            Database.AddIndex("B4_FIAS_ONAME", false, "B4_FIAS", "OFFNAME");
            Database.AddIndex("B4_FIAS_SNAME", false, "B4_FIAS", "SHORTNAME");
            Database.AddIndex("B4_FIAS_REGIONCODE", false, "B4_FIAS", "REGIONCODE");
            Database.AddIndex("B4_FIAS_AUTOCODE", false, "B4_FIAS", "AUTOCODE");
            Database.AddIndex("B4_FIAS_AREACODE", false, "B4_FIAS", "AREACODE");
            Database.AddIndex("B4_FIAS_CITYCODE", false, "B4_FIAS", "CITYCODE");
            Database.AddIndex("B4_FIAS_CTARCODE", false, "B4_FIAS", "CTARCODE");
            Database.AddIndex("B4_FIAS_PLACECODE", false, "B4_FIAS", "PLACECODE");
            Database.AddIndex("B4_FIAS_STREETCODE", false, "B4_FIAS", "STREETCODE");
            Database.AddIndex("B4_FIAS_EXTRCODE", false, "B4_FIAS", "EXTRCODE");
            Database.AddIndex("B4_FIAS_SEXTCODE", false, "B4_FIAS", "SEXTCODE");


            Database.AddEntityTable("B4_FIAS_ADDRESS",
                new Column("POST_CODE", DbType.String, 6),
                new Column("ADDRESS_NAME", DbType.String, 1000),
                new Column("PLACE_ADDRESS_NAME", DbType.String, 1000),
                new Column("PLACE_NAME", DbType.String, 100),
                new Column("PLACE_GUID", DbType.String, 36),
                new Column("PLACE_CODE", DbType.String, 30),
                new Column("STREET_NAME", DbType.String, 50),
                new Column("STREET_GUID", DbType.String, 36),
                new Column("STREET_CODE", DbType.String, 30),
                new Column("HOUSE", DbType.String, 10),
                new Column("HOUSING", DbType.String, 10),
                new Column("BUILDING", DbType.String, 10),
                new Column("FLAT", DbType.String, 10),
                new Column("COORDINATE", DbType.String, 50)
            );

            Database.AddIndex("B4_B4_FIAS_ADDRESS_ANAME", false, "B4_FIAS_ADDRESS", "ADDRESS_NAME");
            Database.AddIndex("B4_B4_FIAS_ADDRESS_PNAME", false, "B4_FIAS_ADDRESS", "PLACE_ADDRESS_NAME");
            Database.AddIndex("B4_B4_FIAS_ADDRESS_PGUID", false, "B4_FIAS_ADDRESS", "PLACE_GUID");
            Database.AddIndex("B4_B4_FIAS_ADDRESS_PCODE", false, "B4_FIAS_ADDRESS", "PLACE_CODE");
            Database.AddIndex("B4_B4_FIAS_ADDRESS_SGUID", false, "B4_FIAS_ADDRESS", "STREET_GUID");
            Database.AddIndex("B4_B4_FIAS_ADDRESS_SCODE", false, "B4_FIAS_ADDRESS", "STREET_CODE");
        }

        public override void Down()
        {
            Database.RemoveTable("B4_FIAS");
            Database.RemoveTable("B4_FIAS_ADDRESS");
        }
    }
}