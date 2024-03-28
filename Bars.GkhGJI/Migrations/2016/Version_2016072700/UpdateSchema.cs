namespace Bars.GkhGji.Migrations._2016.Version_2016072700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016072700")]
    [MigrationDependsOn(typeof(Version_2016061700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("GJI_REMINDER", new RefColumn("GUARANTOR_ID", "GUARANTOR_GKH_DICT_INSPECTOR", "GKH_DICT_INSPECTOR", "ID"));
            this.Database.AddRefColumn(
                    "GJI_REMINDER",
                    new RefColumn("CHECKINGINSPECTOR_ID", "CHECKINGINSPECTOR_GKH_DICT_INSPECTOR", "GKH_DICT_INSPECTOR", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_REMINDER", "GUARANTOR_ID");
            this.Database.RemoveColumn("GJI_REMINDER", "CHECKINGINSPECTOR_ID");
        }
    }
}