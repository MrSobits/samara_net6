namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018061800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018061800")]
   
    [MigrationDependsOn(typeof(Version_2018061500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("CLW_LAWSUIT", "DESCRIPTION", DbType.String, ColumnProperty.None);
        }
        public override void Down()
        {
            this.Database.RemoveColumn("CLW_LAWSUIT", "DESCRIPTION");
        }
    }
}