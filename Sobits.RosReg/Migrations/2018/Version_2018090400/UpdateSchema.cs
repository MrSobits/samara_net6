namespace Sobits.RosReg.Migrations._2018.Version_2018090400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Map;

    [Migration("2018090400")]
    [MigrationDependsOn(typeof(Version_1.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName egrnTable = new SchemaQualifiedObjectName
            { Name = ExtractEgrnMap.TableName, Schema = ExtractEgrnMap.SchemaName };

        private readonly SchemaQualifiedObjectName egrnRightIndTable = new SchemaQualifiedObjectName
            { Name = ExtractEgrnRightIndMap.TableName, Schema = ExtractEgrnRightIndMap.SchemaName };

        private readonly SchemaQualifiedObjectName egrnRightTable = new SchemaQualifiedObjectName
            { Name = ExtractEgrnRightMap.TableName, Schema = ExtractEgrnRightMap.SchemaName };

        public override void Up()
        {
            this.Database.AddTable(
                this.egrnTable,
                new Column(nameof(ExtractEgrn.Id).ToLower(), DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new Column(nameof(ExtractEgrn.CadastralNumber).ToLower(), DbType.String),
                new Column(nameof(ExtractEgrn.Type).ToLower(), DbType.String, ColumnProperty.Null),
                new Column(nameof(ExtractEgrn.Purpose).ToLower(), DbType.String, ColumnProperty.Null),
                new Column(nameof(ExtractEgrn.Address).ToLower(), DbType.String, ColumnProperty.Null),
                new Column(nameof(ExtractEgrn.Area).ToLower(), DbType.Decimal, ColumnProperty.Null),
                new Column(nameof(ExtractEgrn.ExtractDate).ToLower(), DbType.DateTime, ColumnProperty.Null),
                new RefColumn(
                    nameof(ExtractEgrn.ExtractId).ToLower(),
                    $"fk_{ExtractEgrnMap.TableName}_{nameof(ExtractEgrn.ExtractId).ToLower()}_{ExtractMap.TableName}_id",
                    $"{ExtractMap.SchemaName}.{ExtractMap.TableName}",
                    "id"),
                new RefColumn(
                    nameof(ExtractEgrn.RoomId).ToLower(),
                    $"fk_{ExtractEgrnMap.TableName}_{nameof(ExtractEgrn.RoomId).ToLower()}_gkh_room_id",
                    "gkh_room",
                    "id"));

            this.Database.AddTable(
                this.egrnRightTable,
                new Column(nameof(ExtractEgrnRight.Id).ToLower(), DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new Column(nameof(ExtractEgrnRight.Type).ToLower(), DbType.String),
                new Column(nameof(ExtractEgrnRight.Number).ToLower(), DbType.String, ColumnProperty.Null),
                new Column(nameof(ExtractEgrnRight.Share).ToLower(), DbType.String, ColumnProperty.Null),
                new RefColumn(
                    nameof(ExtractEgrnRight.EgrnId).ToLower(),
                    $"fk_{ExtractEgrnRightMap.TableName}_{nameof(ExtractEgrnRight.EgrnId).ToLower()}_{ExtractEgrnMap.TableName}_id",
                    $"{ExtractEgrnMap.SchemaName}.{ExtractEgrnMap.TableName}",
                    "id"));

            this.Database.AddTable(
                this.egrnRightIndTable,
                new Column(nameof(ExtractEgrnRightInd.Id).ToLower(), DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new Column(nameof(ExtractEgrnRightInd.Surname).ToLower(), DbType.String),
                new Column(nameof(ExtractEgrnRightInd.FirstName).ToLower(), DbType.String, ColumnProperty.Null),
                new Column(nameof(ExtractEgrnRightInd.Patronymic).ToLower(), DbType.String, ColumnProperty.Null),
                new Column(nameof(ExtractEgrnRightInd.BirthDate).ToLower(), DbType.DateTime, ColumnProperty.Null),
                new Column(nameof(ExtractEgrnRightInd.BirthPlace).ToLower(), DbType.String, ColumnProperty.Null),
                new Column(nameof(ExtractEgrnRightInd.Snils).ToLower(), DbType.String, ColumnProperty.Null),
                new RefColumn(
                    nameof(ExtractEgrnRightInd.RightId).ToLower(),
                    $"fk_{ExtractEgrnRightIndMap.TableName}_{nameof(ExtractEgrnRightInd.RightId).ToLower()}_{ExtractEgrnRightMap.TableName}_id",
                    $"{ExtractEgrnRightMap.SchemaName}.{ExtractEgrnRightMap.TableName}",
                    "id"));
        }

        public override void Down()
        {
            this.Database.RemoveTable(this.egrnRightIndTable);
            this.Database.RemoveTable(this.egrnRightTable);
            this.Database.RemoveTable(this.egrnTable);
        }
    }
}