namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017092600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017092600")]
    [MigrationDependsOn(typeof(Version_2017091600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_INDIVIDUAL_ACC_OWN",
                new RefColumn("RO_ID", ColumnProperty.Null, "GKH_REALITY_OBJECT", "RO_ID", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "RO_ID");
        }
    }
}