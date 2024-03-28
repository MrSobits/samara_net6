namespace Bars.GkhCr.Migration.Version_2015040900
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015040900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2015032300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("CR_OBJ_PERFOMED_WORK_ACT", new RefColumn("DOC_FILE_ID", "DOCUMENT_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("CR_OBJ_PERFOMED_WORK_ACT", new RefColumn("ADDIT_FILE_ID", "ADDITION_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "ADDIT_FILE_ID");
            Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "DOC_FILE_ID");
        }
    }
}