namespace Sobits.RosReg.Migrations._2023.Version_2023021500
{
    using Bars.B4.Modules.Ecm7.Framework;

    using Sobits.RosReg.Map;
    using System.Data;

    [Migration("2023021500")]
    [MigrationDependsOn(typeof(_2022.Version_2022122900.UpdateSchema))]
    public class UpdateSchema : Migration
    {

        private readonly SchemaQualifiedObjectName extractTable = new SchemaQualifiedObjectName
        { Name = ExtractMap.TableName, Schema = ExtractMap.SchemaName };

        public override void Up()
        {
            this.Database.AddColumn(extractTable, new Column("COMMENT", DbType.String, 3000));
            this.Database.ExecuteNonQuery(@"ALTER TABLE rosreg.extractegrn ALTER COLUMN type TYPE character varying(500);");
        }

        public override void Down()
        {
            this.Database.RemoveColumn(extractTable, "COMMENT");
        }
    }
}