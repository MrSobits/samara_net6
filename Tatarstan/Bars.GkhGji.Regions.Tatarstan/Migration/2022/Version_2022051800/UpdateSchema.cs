namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022051800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022051800")]
    [MigrationDependsOn(typeof(Version_2022051601.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName[] Tables => new []
        {
            new SchemaQualifiedObjectName { Name = "GJI_KNM_REASON" },
            new SchemaQualifiedObjectName { Name = "GJI_TAT_DISPOSAL_ANNEX" }
        };
        
        private Column[] Columns => new []
        {
            new Column("ERKNM_GUID", DbType.String.WithSize(36)),
            new Column("ATTACHMENT_GUID", DbType.String.WithSize(36))
        };

        /// <inheritdoc />
        public override void Up()
        {
            foreach (var table in this.Tables)
            {
                foreach (var column in this.Columns)
                {
                    this.Database.AddColumn(table, column);
                }
            }
        }

        /// <inheritdoc />
        public override void Down()
        {
            foreach (var table in this.Tables)
            {
                foreach (var column in this.Columns)
                {
                    this.Database.RemoveColumn(table, column.Name);
                }
            }
        }
    }
}