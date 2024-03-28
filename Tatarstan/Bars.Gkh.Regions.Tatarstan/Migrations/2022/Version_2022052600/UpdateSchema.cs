namespace Bars.Gkh.Regions.Tatarstan.Migrations._2022.Version_2022052600
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022052600")]
    [MigrationDependsOn(typeof(_2021.Version_2021122300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
                UPDATE B4_PRINT_FORM 
                SET classname = 'GJI Tatarstan.Report.GjiWorkReport' 
                WHERE classname = 'GJI Report.GjiWorkReport'
            ");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.ExecuteNonQuery(@"
                UPDATE B4_PRINT_FORM 
                SET classname = 'GJI Report.GjiWorkReport' 
                WHERE classname = 'GJI Tatarstan.Report.GjiWorkReport'
            ");
        }
    }
}