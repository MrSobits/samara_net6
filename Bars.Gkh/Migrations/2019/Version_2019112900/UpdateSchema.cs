namespace Bars.Gkh.Migrations._2019.Version_2019112900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019112900")]
    
    [MigrationDependsOn(typeof(Version_2019111400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
           this.Database.AddColumn("CLW_LAWSUIT", "COMENT_CONSIDERATION", DbType.String, 500, ColumnProperty.None);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CLW_LAWSUIT", "COMENT_CONSIDERATION");
        }
    }
}