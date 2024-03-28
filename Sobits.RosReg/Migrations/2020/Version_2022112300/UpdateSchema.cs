namespace Sobits.RosReg.Migrations._2022.Version_2022112300
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

    [Migration("2022112300")]
    [MigrationDependsOn(typeof(_2020.Version_20201023.UpdateSchema))]
    public class UpdateSchema : Migration
    {
    

        private readonly SchemaQualifiedObjectName egrnTable = new SchemaQualifiedObjectName
            { Name = ExtractEgrnMap.TableName, Schema = ExtractEgrnMap.SchemaName };

        public override void Up()
        {
            this.Database.AddColumn(egrnTable, new RefColumn("ROOM_ID", ColumnProperty.None, "EXTRACTEGRN_ROOM_ID", "GKH_ROOM", "ID"));
            this.Database.ExecuteNonQuery(@"update rosreg.extractegrn set ROOM_ID = roomid where roomid is not null");
        }

        public override void Down()
        {
            this.Database.RemoveColumn(egrnTable, "ROOM_ID");
        }
    }
}