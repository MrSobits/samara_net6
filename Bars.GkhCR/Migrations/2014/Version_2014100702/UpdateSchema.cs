namespace Bars.GkhCr.Migrations.Version_2014100702
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100702")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014100701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //удаление колонки не требуется
            //Database.RemoveRefColumn("CR_OBJ_PER_ACT_PAYMENT", "DOCUMENT_ID");
        }

        public override void Down()
        {
            //Database.AddRefColumn("CR_OBJ_PER_ACT_PAYMENT", new RefColumn("DOCUMENT_ID", ColumnProperty.Null, "CR_OBJ_PER_ACT_PAYMENT_D", "B4_FILE_INFO", "ID"));
        }
    }
}