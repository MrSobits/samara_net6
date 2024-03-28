namespace Bars.GisIntegration.Smev.Migrations.Version_2021040700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021040700")]
    public class UpdateSchema : Migration
    {
        private const string TableName = "GI_ERP_GUID";
        
        /// <inheritdoc />
        public override void Up()
        {
            var columns = new[]
            {
                new Column("ENTITY_ID", DbType.Int64),
                new Column("ENTITY_TYPE", DbType.String),
                new Column("ASSEMBLY_TYPE", DbType.String),
                new Column("GUID", DbType.String),
                new Column("IS_ANSWERED", DbType.Boolean, false),
                new Column("TASK_ID", DbType.Int64)
            };

            this.Database.AddEntityTable(UpdateSchema.TableName, columns);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.TableName);
        }
    }
}