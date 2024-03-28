namespace Sobits.RosReg.Migrations._2020.Version_2020052700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Map;

    [Migration("2019052700")]
    [MigrationDependsOn(typeof(_2020.Version_2020051900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName egrnTable = new SchemaQualifiedObjectName
            { Name = ExtractEgrnMap.TableName, Schema = ExtractEgrnMap.SchemaName };


        public override void Up()
        {
            this.Database.AddIndex("extractegrn_id_index",false,this.egrnTable,"id");
            this.Database.AddIndex("extractegrn_address_index",false,this.egrnTable,"address");
            this.Database.AddIndex("extractegrn_cadastralnumber_index",false,this.egrnTable,"cadastralnumber");
        }

        public override void Down()
        {
            this.Database.RemoveIndex("extractegrn_id_index",this.egrnTable);
            this.Database.RemoveIndex("extractegrn_address_index",this.egrnTable);
            this.Database.RemoveIndex("extractegrn_cadastralnumber_index",this.egrnTable);
        }
    }
}