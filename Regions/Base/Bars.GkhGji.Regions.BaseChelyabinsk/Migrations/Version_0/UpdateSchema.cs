namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_0
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("0")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.MigrateToBaseChelyabinsk();
        }

        public override void Down()
        {
        }

        private void MigrateToBaseChelyabinsk()
        {
            this.Database.ExecuteNonQuery(@"
                update schemainfo
                set module_id = 'Bars.GkhGji.Regions.BaseChelyabinsk'
                where module_id = 'Bars.GkhGji.Regions.Chelyabinsk.V2';

                update schemainfodepends
                set module_id = 'Bars.GkhGji.Regions.BaseChelyabinsk'
                where module_id = 'Bars.GkhGji.Regions.Chelyabinsk.V2';

                update schemainfodepends
                set dependency_module_id = 'Bars.GkhGji.Regions.BaseChelyabinsk'
                where dependency_module_id = 'Bars.GkhGji.Regions.Chelyabinsk.V2';
            ");
        }

        private void MigrateToChelyabinskV2()
        {
            this.Database.ExecuteNonQuery(@"
                update schemainfo
                set module_id = 'Bars.GkhGji.Regions.Chelyabinsk.V2'
                where module_id = 'Bars.GkhGji.Regions.BaseChelyabinsk';

                update schemainfodepends
                set module_id = 'Bars.GkhGji.Regions.Chelyabinsk.V2'
                where module_id = 'Bars.GkhGji.Regions.BaseChelyabinsk';

                update schemainfodepends
                set dependency_module_id = 'Bars.GkhGji.Regions.Chelyabinsk.V2'
                where dependency_module_id = 'Bars.GkhGji.Regions.BaseChelyabinsk';
            ");
        }
    }
}