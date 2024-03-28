namespace Bars.GkhDi.Regions.Tatarstan.Migrations.Version_2020012400
{
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2020012400")]
    [MigrationDependsOn(typeof(Version_2014020600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(
                @"update  DI_DICT_TEMPL_SERVICE set IS_CONSIDER_IN_CALC = true where CODE::int in (1, 2, 6, 7, 8, 9, 10, 11, 12, 13, 14, 16, 27, 28, 29);");
        }
    }
}