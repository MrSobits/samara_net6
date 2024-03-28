namespace Sobits.RosReg.Migrations._2018.Version_2018120500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Map;

    [Migration("2018120500")]
    [MigrationDependsOn(typeof(Version_2018112900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName egrnTable = new SchemaQualifiedObjectName
            { Name = ExtractEgrnMap.TableName, Schema = ExtractEgrnMap.SchemaName };

        
        public override void Up()
        {
            this.Database.AddColumn(egrnTable, new Column(nameof(ExtractEgrn.IsMerged).ToLower(), DbType.Int32, 4, ColumnProperty.NotNull,20));

            
        }

        public override void Down()
        {
            this.Database.RemoveColumn(egrnTable, nameof(ExtractEgrn.IsMerged).ToLower());
        }
    }
}