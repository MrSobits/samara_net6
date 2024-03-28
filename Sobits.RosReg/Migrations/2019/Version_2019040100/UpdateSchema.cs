namespace Sobits.RosReg.Migrations._2019.Version_2019040100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Map;

    [Migration("2019040100")]
    [MigrationDependsOn(typeof(_2018.Version_2018120500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName egrnTable = new SchemaQualifiedObjectName
        { Name = ExtractEgrnMap.TableName, Schema = ExtractEgrnMap.SchemaName };


        public override void Up()
        {
         //   this.Database.AddColumn(egrnTable, new Column(nameof(ExtractEgrn.ExtractNumber).ToLower(), DbType.String));


        }

        public override void Down()
        {
           // this.Database.RemoveColumn(egrnTable, nameof(ExtractEgrn.ExtractNumber).ToLower());
        }
    }
}