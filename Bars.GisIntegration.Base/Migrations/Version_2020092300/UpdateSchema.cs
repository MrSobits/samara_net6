namespace Bars.GisIntegration.Base.Migrations.Version_2020092300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020092300")]
    [MigrationDependsOn(typeof(Version_2016120700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private string TableName = "GI_FRGU_FUNCTION";

        /// <inheritdoc />
        public override void Up()
        {
            var columns = new []
            {
                new Column("NAME", DbType.String),
                new Column("FRGU_ID", DbType.String),
                new Column("GUID", DbType.String)
            };

            this.Database.AddEntityTable(TableName, columns);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.TableName);
        }
    }
}