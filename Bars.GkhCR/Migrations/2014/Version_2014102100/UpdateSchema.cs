namespace Bars.GkhCr.Migrations.Version_2014102100
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014101700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.ColumnExists("CR_OBJ_PER_ACT_PAYMENT", "DOCUMENT_ID"))
            {
                Database.AddRefColumn("CR_OBJ_PER_ACT_PAYMENT", new RefColumn("DOCUMENT_ID", "CR_OBJ_PER_ACT_PAYMENT_D", "B4_FILE_INFO", "ID"));
            }
        }

        public override void Down()
        {
        }
    }
}