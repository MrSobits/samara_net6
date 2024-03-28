namespace Sobits.RosReg.Migrations._2022.Version_2022122900
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Map;

    using ForeignKeyConstraint = Bars.B4.Modules.Ecm7.Framework.ForeignKeyConstraint;

    [Migration("2022122900")]
    [MigrationDependsOn(typeof(_2022.Version_2022112300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
    

        private readonly SchemaQualifiedObjectName egrnTable = new SchemaQualifiedObjectName
            { Name = ExtractEgrnMap.TableName, Schema = ExtractEgrnMap.SchemaName };

        public override void Up()
        {            
            this.Database.ExecuteNonQuery(@"alter table rosreg.extractegrn drop constraint fk_extractegrn_room_id;");
        }

        public override void Down()
        {
          
        }
    }
}