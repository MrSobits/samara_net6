namespace Bars.GisIntegration.Base.Migrations.Version_2020100100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2020100100")]
    [MigrationDependsOn(typeof(Version_2020092300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly string tableName = "GI_FRGU_FUNCTION";
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ChangeColumn(this.tableName, new Column("NAME", DbType.String, 1024));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.ChangeColumn(this.tableName, new Column("NAME", DbType.String, 255));
        }
    }
}