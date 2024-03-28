namespace Sobits.RosReg.Migrations._2020.Version_2020051900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Map;

    [Migration("2019051900")]
    [MigrationDependsOn(typeof(_2019.Version_2019040100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName egrnRightIdTable = new SchemaQualifiedObjectName
        { Name = ExtractEgrnRightIndMap.TableName, Schema = ExtractEgrnMap.SchemaName };


        public override void Up()
        {
            this.Database.AddColumn(this.egrnRightIdTable, new Column(nameof(ExtractEgrnRightInd.DocIndCode).ToLower(), DbType.String, ColumnProperty.Null));
            this.Database.AddColumn(this.egrnRightIdTable, new Column(nameof(ExtractEgrnRightInd.DocIndName).ToLower(), DbType.String, ColumnProperty.Null));
            this.Database.AddColumn(this.egrnRightIdTable, new Column(nameof(ExtractEgrnRightInd.DocIndSerial).ToLower(), DbType.String, ColumnProperty.Null));
            this.Database.AddColumn(this.egrnRightIdTable, new Column(nameof(ExtractEgrnRightInd.DocIndNumber).ToLower(), DbType.String, ColumnProperty.Null));
            this.Database.AddColumn(this.egrnRightIdTable, new Column(nameof(ExtractEgrnRightInd.DocIndDate).ToLower(), DbType.DateTime, ColumnProperty.Null));
            this.Database.AddColumn(this.egrnRightIdTable, new Column(nameof(ExtractEgrnRightInd.DocIndIssue).ToLower(), DbType.String, ColumnProperty.Null));
        }

        public override void Down()
        {
           // this.Database.RemoveColumn(egrnTable, nameof(ExtractEgrn.ExtractNumber).ToLower());
           this.Database.RemoveColumn(this.egrnRightIdTable, nameof(ExtractEgrnRightInd.DocIndCode).ToLower());
           this.Database.RemoveColumn(this.egrnRightIdTable, nameof(ExtractEgrnRightInd.DocIndName).ToLower());
           this.Database.RemoveColumn(this.egrnRightIdTable, nameof(ExtractEgrnRightInd.DocIndSerial).ToLower());
           this.Database.RemoveColumn(this.egrnRightIdTable, nameof(ExtractEgrnRightInd.DocIndNumber).ToLower());
           this.Database.RemoveColumn(this.egrnRightIdTable, nameof(ExtractEgrnRightInd.DocIndDate).ToLower());
           this.Database.RemoveColumn(this.egrnRightIdTable, nameof(ExtractEgrnRightInd.DocIndIssue).ToLower());
        }
    }
}