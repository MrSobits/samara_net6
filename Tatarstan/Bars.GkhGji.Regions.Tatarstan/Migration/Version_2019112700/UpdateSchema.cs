namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019112700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019112700")]
    [MigrationDependsOn(typeof(Version_2019112201.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("GJI_DICT_CONTROL_LIST_TYPICAL_QUESTION", 
                new RefColumn("NORMATIVE_DOC_ID", ColumnProperty.Null, "GJI_DICT_CONTROL_LIST_TYPICAL_QUESTIONS_NORMATIVE_DOC_ID", "GKH_DICT_NORMATIVE_DOC", "ID"));
        }
    }
}
