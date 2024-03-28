namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014121701
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014121700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_PRESCR_BASE_DOC", new Column("NUM_DOC", DbType.String, 300));
            this.Database.AddColumn("GJI_PRESCR_BASE_DOC", new Column("DATE_DOC", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_PRESCR_BASE_DOC", "NUM_DOC");
            this.Database.RemoveColumn("GJI_PRESCR_BASE_DOC", "DATE_DOC");

        }
    }
}