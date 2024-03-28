namespace Bars.Gkh.Regions.Voronezh.Migrations._2018.Version_2018022100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018022100")]
    [MigrationDependsOn(typeof(_2016.Version_1.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            var baseTableName = new SchemaQualifiedObjectName { Name = "DATEAREAOWNER", Schema = "IMPORT" };
            //смена руками с varchar(255) на text после миграции -_-
            this.Database.AddTable(baseTableName,
                new Column("ID_Object", DbType.String),
                new Column("CadastralNumber", DbType.String),
                new Column("ObjectType", DbType.String),
                new Column("ObjectTypeText", DbType.String),
                new Column("ObjectTypeName", DbType.String),
                new Column("AssignationCode", DbType.String),
                new Column("AssignationCodeText", DbType.String),
                new Column("Area", DbType.String),
                new Column("AreaText", DbType.String),
                new Column("AreaUnit", DbType.String),
                new Column("Floor", DbType.String),
                new Column("ID_Address", DbType.String),
                new Column("AddressContent", DbType.String),
                new Column("RegionCode", DbType.String),
                new Column("RegionName", DbType.String),
                new Column("OKATO", DbType.String),
                new Column("KLADR", DbType.String),
                new Column("CityName", DbType.String),
                new Column("StreetName", DbType.String),
                new Column("Level1Name", DbType.String),
                new Column("ApartmentName", DbType.String),
                new Column("ID_Subject", DbType.String),
                new Column("Code_SP", DbType.String),
                new Column("PersonContent", DbType.String),
                new Column("Surname", DbType.String),
                new Column("FirstName", DbType.String),
                new Column("Patronymic", DbType.String),
                new Column("DateBirth", DbType.String),
                new Column("Citizenship", DbType.String),
                new Column("Sex", DbType.String),
                new Column("PassportContent", DbType.String),
                new Column("TypeDocument", DbType.String),
                new Column("DocumentName", DbType.String),
                new Column("Series", DbType.String),
                new Column("Number", DbType.String),
                new Column("IssueDate", DbType.String),
                new Column("IssueOrgan", DbType.String),
                new Column("ID_AddressL", DbType.String),
                new Column("AddressContentL", DbType.String),
                new Column("RegionCodeL", DbType.String),
                new Column("RegionNameL", DbType.String),
                new Column("OKATOL", DbType.String),
                new Column("KLADRL", DbType.String),
                new Column("SNILS", DbType.String),
                new Column("ID_Record", DbType.String),
                new Column("RegNumber", DbType.String),
                new Column("Type", DbType.String),
                new Column("RegName", DbType.String),
                new Column("RegDate", DbType.String),
                new Column("RestorCourt", DbType.String),
                new Column("ShareText", DbType.String),
                new Column("IsProcessed", DbType.Boolean),
                new Column("IsImported", DbType.Boolean),
                new Column("IsProcessedWithCollision", DbType.Boolean));
        }

        public override void Down()
        {
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = "DATEAREAOWNER", Schema = "IMPORT" });
        }
    }
}