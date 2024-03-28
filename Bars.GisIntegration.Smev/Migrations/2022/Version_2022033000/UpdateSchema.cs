namespace Bars.GisIntegration.Smev.Migrations._2022.Version_2022033000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022033000")]
    [MigrationDependsOn(typeof(Version_2021040700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string TableName = "GI_ERKNM";
        
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
                new RefColumn("TASK_ID", "FK_ERKNM_GI_TASK", "GI_TASK", "ID"),
                new Column("REPLY_DATE", DbType.DateTime)
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